using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace dotNet5781_03B_5857_1544
{

    /// <summary>
    /// Interaction logic for listDoubleClick.xaml.
    /// this window opens when the user double clicks on a bus from list box in main window.
    /// </summary>
    public partial class listDoubleClick : Window
    {
        private Bus currentBus; //referernce to chosen bus from main window
        MainWindow wnd = (MainWindow)Application.Current.MainWindow; //reperence to main window in order to update list box items(buses)

        public listDoubleClick(Bus b)
        {
            InitializeComponent();
            currentBus = b; //refer the bus from main window

            PWindow.DataContext = currentBus;

            //update all lables with bus properties:

            lbFuel.DataContext = currentBus.Fuel;
            lbID.DataContext = currentBus.LICENSENUM;
            lbKM.DataContext = currentBus.MILEAGE;
            lbLast.DataContext = currentBus.lastMaintDate;
            lblState.DataContext = currentBus.BUSSTATE;
            lbfromLast.DataContext = currentBus.MileageSinceLastMaint;
        }

        /// <summary>
        /// a button that sends the bus for a maintenance 
        /// it takes 144 seconds (24 hours in real world)
        /// activates an asynchronic task that counts the time for maintaing, updates in real time and shows this in the progres bar in main window.
        /// the task happens in parallel to main thread and alows the user to continue use the program
        /// </summary>
        private async void Maintain_Click(object sender, RoutedEventArgs e)
        {
            Close();

            if (currentBus.BUSSTATE == Status.Refueling) //can't maintain while refuling
            {
                MessageBox.Show("bus is in refuel, wait until it gets back");
            }

            else if (currentBus.BUSSTATE == Status.During) //can't maintain while riding
            {
                MessageBox.Show("bus is in a ride, wait until it gets back");
            }

            else //bus is free
            {
                await MaintainAsync();  //activate the parallel asynchronic task

                //wnd.LbBuses.Items.Refresh();
            }
        }

        /// <summary>
        /// the task activated by the last function in order to maintain bus
        /// </summary>
        /// <returns></returns>
        private async Task MaintainAsync()
        {
            currentBus.BUSSTATE = Status.InMaintenance;
            //wnd.LbBuses.Items.Refresh();

            int amount = currentBus.MileageSinceLastMaint / 100; //the amount of to update in every second - in order to reflect maintenance progress in main window

            for (int i = 0; i < 100; i++)
            {
                await Task.Run(() => currentBus.Maintain(amount));
                //wnd.LbBuses.Items.Refresh();
            }

            //update bus properties
            currentBus.MileageSinceLastMaint = 0;
            currentBus.lastMaintDate = DateTime.Today;
            currentBus.Fuel = 1200;
            currentBus.lastMaintMileage = currentBus.MILEAGE;
            currentBus.SetStatus();
        }


        /// <summary>
        /// a button that sends the bus for a refuel to a level of 1200. 
        /// it takes 12 seconds (2 hours in real world)
        /// activates an asynchronic task that counts the time for refuling, updates in real time and shows this in the progres bar.
        /// the task happens in parallel to main thread and alows the user to continue use the program
        /// </summary>
        private async void Refuel_Click(object sender, RoutedEventArgs e)
        {
            Close();

            //at first - check if bus is busy in ride/maint and cant refuel
            if (currentBus.BUSSTATE == Status.InMaintenance)
            {
                MessageBox.Show("bus is in maintenance, no need to refuel twice");
            }

            else if (currentBus.BUSSTATE == Status.During)
            {
                MessageBox.Show("bus is in a ride, wait until it gets back");
            }

            else
            {
                currentBus.BUSSTATE = dotNet5781_03B_5857_1544.Status.Refueling;

                int amount = (1200 - currentBus.Fuel) / 10; //the amount of fuel to update in each second from the 12 of refuling

                await RefuelAsync(amount, currentBus); //activate the parallel asynchronic task

                //wnd.LbBuses.Items.Refresh();
            }
        }

        /// <summary>
        /// the task activated by the last function in order to update bus fuel amount
        /// </summary>
        /// <param name="amount">fuel amount to add in each second</param>
        /// <param name="b">bus to update</param>
        private async Task RefuelAsync(int amount, Bus b)
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Run(() => b.Refuel(amount));
                //wnd.LbBuses.Items.Refresh();
            }
            b.Fuel = 1200; //dividing the amount may cause a lack of few liters - so update to 1200
            b.SetStatus();
        }
    }
}
