using BLApi;
using BO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

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
                foreach (BusStation bs in InActiveStationsDataGrid.SelectedItems)
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
