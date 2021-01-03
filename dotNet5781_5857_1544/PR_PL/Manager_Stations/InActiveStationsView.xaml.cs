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
using System.Device.Location;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BLApi;
using BO;
using PR_PL;

namespace PL
{
    /// <summary>
    /// Interaction logic for InActiveStationsView.xaml
    /// </summary>
    public partial class InActiveStationsView : Window
    {
        private readonly IBL bl;
        private StationsView sv;
        public InActiveStationsView(IBL b, StationsView v)
        {
            InitializeComponent();

            bl = b;
            sv = v;

            InActiveStationsDataGrid.DataContext = bl.GetAllInActiveBusStations().ToList(); 
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void InActiveStations_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BusStation bs = (BusStation)InActiveStationsDataGrid.SelectedItem;
            StationDetails sd = new StationDetails(bl, bs);
            sd.Show();
        }

        private void Activate_Click(object sender, RoutedEventArgs e)
        {
            if (InActiveStationsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please choose at least one station and then click activate!");
            }
            else
            {
                var lb = (IEnumerable<BusStation>)(InActiveStationsDataGrid.SelectedItems);

                foreach (BusStation bs in lb)
                {
                    var updated = new BusStation
                    {
                        Code=bs.Code,
                        Name=bs.Name,
                        Location=bs.Location,
                        Address=bs.Address,
                        Accessible=bs.Accessible,
                        Active=true
                    };
                    bl.UpdateBusStation(updated);
                }

                InActiveStationsDataGrid.DataContext = bl.GetAllInActiveBusStations().ToList(); //update view
                sv.StationsDataGrid.DataContext = bl.GetAllBusStations().ToList();
            }
        }
    }
}
