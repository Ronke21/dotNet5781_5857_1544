using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BLApi;
using BO;
using PR_PL;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationsView.xaml
    /// </summary>
    public partial class StationsView : Window
    {
        private readonly IBL bl;
        public StationsView(IBL b)
        {
            InitializeComponent();

            bl = b;
            StationsDataGrid.DataContext = bl.GetAllBusStations().ToList();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            AddStation ast = new AddStation(bl);
            ast.ShowDialog();
            StationsDataGrid.DataContext = bl.GetAllBusStations().ToList();
        }

        private void StationsDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BusStation bs = (BusStation)StationsDataGrid.SelectedItem;
            StationDetails bd = new StationDetails(bl,bs);
            bd.Show();
        }

        private void InActive_Click(object sender, RoutedEventArgs e)
        {
            InActiveStationsView iasv = new InActiveStationsView(bl, this);
            iasv.ShowDialog();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (StationsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please choose at least one station and then click remove!");
            }
            else
            {
                var ls = (IEnumerable<BusStation>)(StationsDataGrid.SelectedItems);

                foreach (var s in ls)
                {
                    bl.DeleteBusStation(((BusStation)s).Code);
                }
                StationsDataGrid.DataContext = bl.GetAllBusStations().ToList();
            }
        }

    }
}
