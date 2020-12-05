using System;
using System.Threading.Tasks;
using System.Windows;

namespace dotNet5781_03B_5857_1544
{

    /// <summary>
    /// Interaction logic for listDoubleClick.xaml
    /// </summary>
    public partial class listDoubleClick : Window
    {
        private Bus currentBus;
        MainWindow wnd = (MainWindow)Application.Current.MainWindow;

        public listDoubleClick(Bus b)
        {
            InitializeComponent();
            currentBus = b;

            lbFuel.DataContext = currentBus.FUELTSTR;
            lbID.DataContext = currentBus.LICENSENUMSTR;
            lbKM.DataContext = currentBus.MILEAGESTR;
            lbLast.DataContext = currentBus.LASTMAINTDATESTR;
            lblState.DataContext = currentBus.BUSSTATESTR;
            lbfromLast.DataContext = currentBus.MILAGESINCELASTMAINTSTR;
        }

        private async void Maintain_Click(object sender, RoutedEventArgs e)
        {
            Close();

            if (currentBus.BUSSTATE == Status.Refueling)
            {
                MessageBox.Show("bus is in refuel, wait until it gets back");
            }

            else if (currentBus.BUSSTATE == Status.During)
            {
                MessageBox.Show("bus is in a ride, wait until it gets back");
            }

            else
            {
                await MaintainAsync();

                wnd.LbBuses.Items.Refresh();
            }
        }

        private async Task MaintainAsync()
        {
            currentBus.BUSSTATE = Status.InMaintenance;
            wnd.LbBuses.Items.Refresh();

            int amount = currentBus.mileageSinceLastMain / 100;

            for (int i = 0; i < 100; i++)
            {
                await Task.Run(() => currentBus.Maintain(amount));
                wnd.LbBuses.Items.Refresh();
            }

            currentBus.mileageSinceLastMain = 0;
            currentBus.lastMaintDate = DateTime.Today;
            currentBus.Fuel = 1200;
            currentBus.lastMaintMileage = currentBus.MILEAGE;
            currentBus.setStatus();
        }

        private async void Refuel_Click(object sender, RoutedEventArgs e)
        {
            Close();

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

                int amount = (1200 - currentBus.Fuel) / 10;

                await RefuelAsync(amount, currentBus);

                wnd.LbBuses.Items.Refresh();
            }
        }

        private async Task RefuelAsync(int amount, Bus b)
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Run(() => b.Refuel(amount));
                wnd.LbBuses.Items.Refresh();
            }
            b.Fuel = 1200;
            b.setStatus();
        }
    }
}
