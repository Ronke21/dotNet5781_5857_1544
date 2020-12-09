/*
 * Course mini project in .Net framework
 * Exercise number 3B
 * Lecturer - David kidron
 * Student - Amihay Hassan, Ron Keinan
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace dotNet5781_03B_5857_1544
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml. 
    /// The main window of the Buses sytem, shows list of all buses and main details. allows to send to refuel or ride, and add new bus.
    /// allows also to double click on a bus for extra information
    /// </summary>
    public partial class MainWindow : Window
    {

        public static Random r = new Random(DateTime.Now.Millisecond); //static random variable to initiallize buses randomly

        private Bus CurrentDisplay; //kind of reference for the current chosen bus in the list box
        
        public static List<Bus> Eged = new List<Bus>(); // a list of buses - our data base!
        
        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < 10; i++) //initiallize Eged with random buses
            {
                Eged.Add(new Bus(r.Next(1000000, 9999999), new DateTime(r.Next(1948, 2018), r.Next(1, 13), 1), r.Next(1201), r.Next(19000), DateTime.Now.AddMonths(r.Next(-20, -1))));
                Eged.Add(new Bus(r.Next(10000000, 99999999), new DateTime(r.Next(2018, 2021), r.Next(1, 13), 1), r.Next(1201), r.Next(19000), DateTime.Now.AddMonths(r.Next(-20, -1))));
            }

            //change properties of buses according to the exercise demandes
            Eged[0].lastMaintDate = DateTime.Now.AddMonths(-13);
            Eged[0].SetStatus();

            Eged[1].lastMaintDate = DateTime.Now.AddMonths(-11);
            Eged[1].SetStatus();

            Eged[2].Fuel = 10;
            Eged[2].SetStatus();

            Eged[3].lastMaintDate = (DateTime.Today).AddDays(-3);
            Eged[3].SetStatus(); // = MaintainSoon

            Eged[4].lastMaintDate = (DateTime.Today).AddMonths(-11).AddDays(-5);
            Eged[4].SetStatus(); // = MaintainSoon

            LbBuses.ItemsSource = Eged; //relate the listbox to the list of Eged
        }

        //a region for the headers of the listbox - they are buttons that allow to sort the list! (bonus)
        #region Sort

        /// <summary>
        /// a button that sorts the bus list according to their license number
        /// </summary>
        private void Sort_by_ID(object sender, RoutedEventArgs e)
        {
            Eged.Sort((bus1, bus2) => bus1.LICENSENUM.CompareTo(bus2.LICENSENUM));
            LbBuses.Items.Refresh();
        }

        /// <summary>
        /// a button that sorts the bus list according to their fuel amount
        /// </summary>
        private void Sort_by_fuel_Amount(object sender, RoutedEventArgs e)
        {
            Eged.Sort((bus1, bus2) => bus2.Fuel.CompareTo(bus1.Fuel));
            LbBuses.Items.Refresh();
        }

        /// <summary>
        /// a button that sorts the bus list according to their status (and colors)
        /// </summary>
        private void Sort_by_status(object sender, RoutedEventArgs e)
        {
            // different solution in order to implement stable sort, because many buses are likely to share status

            IEnumerable<Bus> b = Eged.OrderBy(bus => bus.BUSSTATE).ToList();

            if (Eged.SequenceEqual(b)) return;
            Eged.Clear();
            foreach (var bus in b)
            {
                Eged.Add(bus);
            }
            LbBuses.Items.Refresh();
        }


        #endregion

        /// <summary>
        /// a button that sends the bus for a refuel to a level of 1200. 
        /// it takes 12 seconds (2 hours in real world)
        /// activates an asynchronic task that counts the time for refuling, updates in real time and shows this in the progres bar.
        /// the task happens in parallel to main thread and alows the user to continue use the program
        /// </summary>
        private async void Refuel(object sender, RoutedEventArgs e)
        {
            if (sender != null && sender is Button btn) CurrentDisplay = (Bus)btn.DataContext;
            LbBuses.SelectedItem = null;

            //at first - check if bus is busy in ride/maint and cant refuel

            if (CurrentDisplay.Fuel == 1200)
            {
                MessageBox.Show("Tank is full, no need to refuel");
            }

            else if (CurrentDisplay.BUSSTATE == dotNet5781_03B_5857_1544.Status.InMaintenance) 
            {
                MessageBox.Show("bus is in maintenance, no need to refuel twice");
            }
            
            else if (CurrentDisplay.BUSSTATE == dotNet5781_03B_5857_1544.Status.During)
            {
                MessageBox.Show("bus is in a ride, wait until it gets back");
            }

            else if (CurrentDisplay.BUSSTATE == dotNet5781_03B_5857_1544.Status.Refueling)
            {
                MessageBox.Show("bus is already refueling");
            }

            else //bus is free
            {
                CurrentDisplay.BUSSTATE = dotNet5781_03B_5857_1544.Status.Refueling;
                //LbBuses.Items.Refresh();

                double amount = (1200 - CurrentDisplay.Fuel) / 10; //the amount of fuel to update in each second from the 12 of refuling

                await RefuelAsync(amount, CurrentDisplay); //activate the parallel asynchronic task

                //LbBuses.Items.Refresh();
            }
        }

        /// <summary>
        /// the task activated by the last function in order to update bus fuel amount
        /// </summary>
        /// <param name="amount">fuel amount to add in each second</param>
        /// <param name="b">bus to update</param>
        private async Task RefuelAsync(double amount, Bus b)
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Run(() => b.Refuel(amount));
            }

            b.Fuel = 1200; //dividing the amount may cause a lack of few liters - so update to 1200
            
            b.SetStatus();
        }

        /// <summary>
        /// opens a new window to enter details for a new bus and add it to the list.
        /// </summary>
        private void Add_Bus_to_Eged(object sender, RoutedEventArgs e)
        {
            AddBusWindow addingWin = new AddBusWindow();
            addingWin.ShowDialog();
            LbBuses.Items.Refresh();
        }

        /// <summary>
        /// a button that sends the bus for a ride - opens a new window to get its length from user 
        /// activates an asynchronic task that counts the time for riding, updates in real time and shows this in the progres bar.
        /// the task happens in parallel to main thread and alows the user to continue use the program
        /// </summary>
        void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender != null && sender is Button btn) CurrentDisplay = (Bus)btn.DataContext;
            ChooseBusWindow chooseBus = new ChooseBusWindow(CurrentDisplay);
            LbBuses.SelectedItem = null;

            if (!CurrentDisplay.AllQuailified()) //check if bus can make the ride
            {
                MessageBox.Show("this bus is not qualified for a ride\ntake it to maintenance");
            }

            else if (CurrentDisplay.BUSSTATE ==dotNet5781_03B_5857_1544.Status.InMaintenance)
            {
                MessageBox.Show("You cant send to a ride bus during maintenance");
            }

            else if (CurrentDisplay.BUSSTATE == dotNet5781_03B_5857_1544.Status.Refueling)
            {
                MessageBox.Show("You cant send to a ride bus during refueling");
            }

            else if (CurrentDisplay.BUSSTATE == dotNet5781_03B_5857_1544.Status.During)
            {
                MessageBox.Show("During ride, wait until it gets back");
            }

            else
            {
                chooseBus.ShowDialog();
                //LbBuses.Items.Refresh();
            }
        }

        /// <summary>
        /// double clickign on a bus from the list - opens a new window with extended details and options to refuel/maintain.
        /// </summary>
        private void LbBuses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CurrentDisplay = (Bus)LbBuses.SelectedItem;
            listDoubleClick doubleC = new listDoubleClick(CurrentDisplay);
            LbBuses.SelectedItem = null;
            doubleC.ShowDialog();
            //LbBuses.Items.Refresh();
        }

        /// <summary>
        /// close the main window
        /// /// </summary>

        private void EXIT_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

