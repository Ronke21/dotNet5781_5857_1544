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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static Random r = new Random(DateTime.Now.Millisecond);

        private Bus CurrentDisplay;
        public static List<Bus> Eged = new List<Bus>(); // a list of buses - our data base!

        //BackgroundWorker worker;

        public MainWindow()
        {
            InitializeComponent();

            //worker = new BackgroundWorker();
            //worker.DoWork += Worker_DoWork;
            //worker.ProgressChanged += Worker_ProgressChanged;
            //worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            //worker.WorkerReportsProgress = true;
            //worker.WorkerSupportsCancellation = true; ;


            for (int i = 0; i < 5; i++)
            {
                Eged.Add(new Bus(r.Next(1000000, 9999999), new DateTime(r.Next(1948, 2018), r.Next(1, 13), 1), r.Next(1201), r.Next(19000), DateTime.Now.AddMonths(r.Next(-20, -1))));
                Eged.Add(new Bus(r.Next(10000000, 99999999), new DateTime(r.Next(2018, 2021), r.Next(1, 13), 1), r.Next(1201), r.Next(19000), DateTime.Now.AddMonths(r.Next(-20, -1))));
            }

            Eged[0].lastMaintDate = DateTime.Now.AddMonths(-13);
            Eged[0].setStatus();

            Eged[1].lastMaintDate = DateTime.Now.AddMonths(-11);
            Eged[1].setStatus();

            Eged[2].Fuel = 10;
            Eged[2].setStatus();

            Eged[3].lastMaintDate = (DateTime.Today).AddDays(-3);
            Eged[3].setStatus(); // = MaintainSoon

            Eged[4].lastMaintDate = (DateTime.Today).AddMonths(-11).AddDays(-5);
            Eged[4].setStatus(); // = MaintainSoon

            LbBuses.ItemsSource = Eged;
        }


        #region Sort
        private void Sort_by_ID(object sender, RoutedEventArgs e)
        {
            Eged.Sort((bus1, bus2) => bus1.LICENSENUMINT.CompareTo(bus2.LICENSENUMINT));
            LbBuses.Items.Refresh();
        }
        //private void Sort_by_last_maint(object sender, RoutedEventArgs e)
        //{
        //    Eged.Sort((bus1, bus2) => (bus1.MILEAGE - bus1.lastMaintMileage).CompareTo((bus2.MILEAGE - bus2.lastMaintMileage)));
        //    LbBuses.Items.Refresh();
        //}
        private void Sort_by_fuel_Amount(object sender, RoutedEventArgs e)
        {
            Eged.Sort((bus1, bus2) => bus2.Fuel.CompareTo(bus1.Fuel));
            LbBuses.Items.Refresh();
        }
        //private void Sort_by_Mileage(object sender, RoutedEventArgs e)
        //{
        //    Eged.Sort((bus1, bus2) => bus1.MILEAGE.CompareTo(bus2.MILEAGE));
        //    LbBuses.Items.Refresh();
        //}
        //private void Sort_by_last_Maintenance(object sender, RoutedEventArgs e)
        //{
        //    // sort by the time passed from the last maintenance
        //    Eged.Sort((bus1, bus2) => bus1.lastMaintDate.CompareTo(bus2.lastMaintDate));
        //    LbBuses.Items.Refresh();
        //}
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

        private async void Refuel(object sender, RoutedEventArgs e)
        {
            if (sender != null && sender is Button btn) CurrentDisplay = (Bus)btn.DataContext;
            LbBuses.SelectedItem = null;

            if (CurrentDisplay.BUSSTATE == dotNet5781_03B_5857_1544.Status.InMaintenance)
            {
                MessageBox.Show("bus is in maintenance, no need to refuel twice");
            }

            else if (CurrentDisplay.BUSSTATE == dotNet5781_03B_5857_1544.Status.During)
            {
                MessageBox.Show("bus is in a ride, wait until it gets back");
            }

            else
            {
                CurrentDisplay.BUSSTATE = dotNet5781_03B_5857_1544.Status.Refueling;
                LbBuses.Items.Refresh();

                int amount = (1200 - CurrentDisplay.Fuel) / 10;

                await RefuelAsync(amount, CurrentDisplay);

                LbBuses.Items.Refresh();
            }
        }

        private async Task RefuelAsync(int amount, Bus b)
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Run(() => b.Refuel(amount));
                LbBuses.Items.Refresh();
            }

            b.Fuel = 1200;
            
            b.setStatus();
        }

        private void Add_Bus_to_Eged(object sender, RoutedEventArgs e)
        {
            AddBusWindow addingWin = new AddBusWindow();
            addingWin.ShowDialog();
            LbBuses.Items.Refresh();
        }
        
        void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender != null && sender is Button btn) CurrentDisplay = (Bus)btn.DataContext;
            ChooseBusWindow chooseBus = new ChooseBusWindow(CurrentDisplay);
            LbBuses.SelectedItem = null;

            if (!CurrentDisplay.qualifiedDate())
            {
                MessageBox.Show("this bus is not qualified for a ride\ntake it to maintenance");
            }

            else
            {
                chooseBus.ShowDialog();
                LbBuses.Items.Refresh();
            }
        }


        private void LbBuses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CurrentDisplay = (Bus)LbBuses.SelectedItem;
            listDoubleClick doubleC = new listDoubleClick(CurrentDisplay);
            LbBuses.SelectedItem = null;
            doubleC.ShowDialog();
            LbBuses.Items.Refresh();
        }

        private void EXIT_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

