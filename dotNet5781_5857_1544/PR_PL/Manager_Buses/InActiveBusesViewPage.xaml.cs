using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLApi;
using BO;
using PL;

namespace PR_PL.Manager_Buses
{
    /// <summary>
    /// Interaction logic for InActiveBusesViewPage.xaml
    /// </summary>

    public partial class InActiveBusesViewPage : Page
    {
        MainWindow wnd = (MainWindow)Application.Current.MainWindow;
        private readonly IBL _bl;
        public InActiveBusesViewPage(IBL b)
        {
            InitializeComponent();

            _bl = b;

            InActiveBusesDataGrid.DataContext = _bl.GetAllInActiveBuses();
        }

        private void InActiveBusesDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var b = (Bus)InActiveBusesDataGrid.SelectedItem;
            var bd = new BusDetails(_bl, b);
            bd.Show();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            wnd.DataDisplay.Content = new BusesViewPage(_bl);
        }

        private void Activate_OnClick(object sender, RoutedEventArgs e)
        {
            if (InActiveBusesDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please choose at least one bus and then click activate!");
            }
            else
            {
                var lb = (IEnumerable)(InActiveBusesDataGrid.SelectedItems);

                foreach (Bus b in lb)
                {
                    var updated = new Bus
                    {
                        LicenseNum = b.LicenseNum,
                        Fuel = b.Fuel,
                        Mileage = b.Mileage,
                        StartTime = b.StartTime,
                        LastMaint = b.LastMaint,
                        MileageFromLast = b.MileageFromLast,
                        Active = true
                    };
                    _bl.UpdateBus(updated);
                }

                InActiveBusesDataGrid.DataContext = _bl.GetAllInActiveBuses().ToList();
            }
        }
    }
}