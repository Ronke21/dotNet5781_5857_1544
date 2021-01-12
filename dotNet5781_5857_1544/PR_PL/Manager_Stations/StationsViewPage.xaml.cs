using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLApi;
using BO;
using PL;
using PR_PL.Manager_Buses;

namespace PR_PL.Manager_Stations
{
    /// <summary>
    /// Interaction logic for StationsViewPage.xaml
    /// </summary>
    public partial class StationsViewPage : Page
    {
        private readonly IBL _bl;
        MainWindow wnd = (MainWindow)Application.Current.MainWindow;

        public StationsViewPage(IBL b)
        {
            InitializeComponent();

            _bl = b;

            StationsDataGrid.ItemsSource = _bl.GetAllBusStations().OrderBy(s => s.Code);
        }

        private void StationsDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var bd = new StationDetails(_bl, StationsDataGrid.SelectedItem as BusStation);
            bd.Show();
        }

        private void Remove_OnClick(object sender, RoutedEventArgs e)
        {
            if (StationsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please choose at least one station and then click remove!");
            }
            else
            {
                foreach (var s in StationsDataGrid.SelectedItems)
                {
                    _bl.DeleteBusStation(((BusStation)s).Code);
                }
                StationsDataGrid.DataContext = _bl.GetAllBusStations().ToList();
            }
        }

        private void InActive_OnClick(object sender, RoutedEventArgs e)
        {
            // wnd.DataDisplay.Content =
            // CREATE NEW INACTIVE STATIONS PAGE
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            var ast = new AddStation(_bl);
            ast.ShowDialog();
            StationsDataGrid.DataContext = _bl.GetAllBusStations().ToList();
        }
    }
}
