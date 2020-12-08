using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace dotNet5781_03B_5857_1544
{
    /// <summary>
    /// Interaction logic for ChooseBusWindow.xaml
    /// this window opens when the user decides to send a bus to a ride,
    /// the window recieves the length of ride.
    /// </summary>
    public partial class ChooseBusWindow : Window
    {
        private Bus currentBus; //reference to the current bus chosen to ride from the list
        MainWindow wnd = (MainWindow)Application.Current.MainWindow; //reperence to main window in order to update list box items(buses)

        /// <summary>
        /// initialize the window and connect the reference of current bus to the bus recieved from main window
        /// </summary>
        /// <param name="bus">bus to send to ride</param>
        public ChooseBusWindow(Bus bus)
        {
            InitializeComponent();
            this.currentBus = bus;
        }


        /// <summary>
        /// sends the bus for a ride -  get its length from user in the textbox
        /// activates an asynchronic task that counts the time for riding, updates in real time and shows this in the progres bar in the main window.
        /// the task happens in parallel to main thread and alows the user to continue use the program
        /// </summary>
        private async void ChooseMileage_OnKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Return) //the ride length is set by pressing "ENTER" - as the exercise Bonus demanded
            {
                int.TryParse(ChooseMileage.Text, out var mileage);

                //if (mileage == 0)
                //{
                //    Close();
                //    MessageBox.Show("distance is not valid");
                //}

                /*else*/ if (currentBus.MILEAGE - currentBus.LastMaintMileage + mileage > 20000) //bus canwt do the ride because it need maintenance
                {
                    Close();
                    MessageBox.Show("this bus is not qualified for a ride\ntake it to maintenance");
                }

                else if (mileage > 1200) //fuel is enought for only 1200 rides
                {
                    Close();
                    MessageBox.Show("ride cannot be over 1200KM");
                }

                else if (mileage > currentBus.Fuel) //not enough fuel in bus
                {
                    Close();
                    MessageBox.Show("not enough fuel\nplease refuel");
                }

                else //bus can take the ride!
                {
                    Close();

                    double mil = (double)((double)mileage / 10); //set the amount of advance for each second the bus is in the ride

                    currentBus.MaxRide = mileage; //set the maximuo range for the progress bar

                    await RideAsync(mil); //activate asynchronic task for ride

                    wnd.LbBuses.Items.Refresh();
                }
            }
        }

        /// <summary>
        ///  the task activated by the last function in order to send bus to ride
        /// </summary>
        /// <param name="mil">advance in km of eac second of a ride</param>
        /// <returns></returns>
        private async Task RideAsync(double mil)
        {

            for (int i = 0; i < 10; i++)
            {
                currentBus.BUSSTATE = Status.During;
                await Task.Run(() => currentBus.Ride(mil)); //the ride func in bus class
                wnd.LbBuses.Items.Refresh();
            }
            currentBus.RIDE = 0;
            currentBus.Fuel = (int)Math.Round(currentBus.Fuel);
            currentBus.SetStatus();
            wnd.LbBuses.Items.Refresh();
        }


        /// <summary>
        /// this functs makes sure that the text box for the ride length receives only digits (Bonus)
        /// </summary>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            if (e.Handled)
            {
                ChooseMileage.BorderBrush = Brushes.Red;
                ChooseMileage.BorderThickness = new Thickness(3);
                MessageBox.Show($"digits only\n'{e.Text}' is not a digit");
            }

            else
            {
                ChooseMileage.BorderBrush = Brushes.DarkGray;
                ChooseMileage.BorderThickness = new Thickness(1);
            }
        }
    }
}
