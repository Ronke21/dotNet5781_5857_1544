using System.Threading;
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

        private void Maintain_Click(object sender, RoutedEventArgs e)
        {
            Close();

            Thread maintainThread = new Thread(currentBus.Maintain);
            maintainThread.Start();
            
            wnd.LbBuses.Items.Refresh();
            MessageBox.Show("maintenance started");

            while (maintainThread.IsAlive) { }
            wnd.LbBuses.Items.Refresh();

            //MessageBox.Show($@"Last Maintenance for {currentBus.LICENSENUMSTR} updated for today!\nfuel tank is full!");
        }

        private void Refuel_Click(object sender, RoutedEventArgs e)
        {
            Close();
            
            Thread refuelThread = new Thread(currentBus.Refuel);
            refuelThread.Start();
            
            wnd.LbBuses.Items.Refresh();
            MessageBox.Show("refuel started");

            while (refuelThread.IsAlive) { }
            wnd.LbBuses.Items.Refresh();
            
            //MessageBox.Show("Fuel tank is full!");
        }
    }
}
