using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace dotNet5781_03B_5857_1544
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
  
        public static Random r = new Random(DateTime.Now.Millisecond);

        private Bus CurrentDisplay;
        public List<Bus> Eged = new List<Bus>(); // a list of buses - our data base!
        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < 10; i++)
            {
                Eged.Add(new Bus(r.Next(1000000, 9999999), new DateTime(r.Next(1948, 2017), r.Next(1, 13), r.Next(1, 31)), r.Next(1200), r.Next(19000), DateTime.Now.AddMonths(r.Next(-20, -1))));
                Eged.Add(new Bus(r.Next(10000000, 99999999), new DateTime(r.Next(2018, 2020), r.Next(1, 13), r.Next(1, 31)), r.Next(1200), r.Next(19000), DateTime.Now.AddMonths(r.Next(-20, -1))));
            }
            Eged[0].lastMaintDate = DateTime.Now.AddMonths(-13);
            Eged[1].lastMaintDate = DateTime.Now.AddMonths(-11);
            Eged[2].Fuel = 10;

            lbBuses.ItemsSource = Eged;
        }


        private void Sort_by_ID(object sender, RoutedEventArgs e)
        {
            Eged.Sort((bus1, bus2) => bus1.LICENSENUM.CompareTo(bus2.LICENSENUM));
            lbBuses.Items.Refresh();
        }

        private void Sort_by_last_maint(object sender, RoutedEventArgs e)
        {
            Eged.Sort((bus1, bus2) => (bus1.MILEAGE - bus1.lastMaintMileage).CompareTo((bus2.MILEAGE - bus2.lastMaintMileage)));
            lbBuses.Items.Refresh();
        }

        private void Sort_by_fuel_Amount(object sender, RoutedEventArgs e)
        {
            Eged.Sort((bus1, bus2) => bus2.Fuel.CompareTo(bus1.Fuel));
            lbBuses.Items.Refresh();
        }

        private void Sort_by_Mileage(object sender, RoutedEventArgs e)
        {
            Eged.Sort((bus1, bus2) => bus1.MILEAGE.CompareTo(bus2.MILEAGE));
            lbBuses.Items.Refresh();
        }

        private void Sort_by_last_Maintenance(object sender, RoutedEventArgs e)
        {
            // sort by the time passed from the last maintenance
            Eged.Sort((bus1, bus2) => bus1.lastMaintDate.CompareTo(bus2.lastMaintDate));
            lbBuses.Items.Refresh();
        }

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
            lbBuses.Items.Refresh();
        }
        private void list_click1(object sender, RoutedEventArgs e)
        {
            CurrentDisplay = (Bus)lbBuses.SelectedItem;
            CurrentDisplay.Fuel = 0;
            lbBuses.Items.Refresh();
        }
        private void list_click2(object sender, RoutedEventArgs e)
        {
            CurrentDisplay = (Bus)lbBuses.SelectedItem;
            CurrentDisplay.lastMaintDate = DateTime.Now;
            lbBuses.Items.Refresh();
        }

        private void Add_Bus_to_Eged(object sender, RoutedEventArgs e)
        {
            /*
            DateTime start = getStartingDate();

            int id;
            if (start.Year > 2017) //so the ID is 8 digit
            {
                id = get8DigitsLineID(Eged);
            }
            else //7 digit ID
            {
                id = get7DigitsLineID(Eged);
            }

            int fuel = getFuelAmount();

            int km = getMileage();

            DateTime lastMaint = getLastMaint();

            Eged.Add(new Bus(id, start, fuel, km, lastMaint)); //send details to constructor
            */
            AddBusWindow addingWin = new AddBusWindow();
            addingWin.Show();
        }
    }
}
