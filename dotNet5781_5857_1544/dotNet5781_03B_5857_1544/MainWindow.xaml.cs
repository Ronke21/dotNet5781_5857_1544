using System;
using System.Collections.Generic;
using System.Windows;

namespace dotNet5781_03B_5857_1544
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Random r = new Random(DateTime.Now.Millisecond);

        private List<Bus> CurrentDisplay;
        public List<Bus> Eged = new List<Bus>(); // a list of buses - our data base!
        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < 10; i++)
            {
                Eged.Add(new Bus(r.Next(1000000, 9999999), new DateTime(r.Next(1948, 2017), r.Next(1, 13), r.Next(1, 31)), r.Next(1200), r.Next(19000), DateTime.Now.AddMonths(r.Next(-20,-1))));
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

        private void Sort_by_last_Maintainance(object sender, RoutedEventArgs e)
        {
            Eged.Sort((bus1, bus2) => bus1.lastMaintDate.CompareTo(bus2.lastMaintDate));
            lbBuses.Items.Refresh();
        }
    }
}
