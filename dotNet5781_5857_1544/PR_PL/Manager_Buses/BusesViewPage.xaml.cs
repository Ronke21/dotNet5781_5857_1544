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
    /// Interaction logic for BusesViewPage.xaml
    /// </summary>
    public partial class BusesViewPage : Page
    {
        private readonly IBL _bl;
        MainWindow wnd = (MainWindow)Application.Current.MainWindow;
        public BusesViewPage(IBL b)
        {
            _bl = b;

            InitializeComponent();

            BusesDataGrid.DataContext = _bl.GetAllBuses().ToList();
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            AddBus ab = new AddBus(_bl);
            ab.ShowDialog();
            BusesDataGrid.DataContext = _bl.GetAllBuses().ToList();
        }

        private void BusesDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Bus b = (Bus)BusesDataGrid.SelectedItem;
            BusDetails bd = new BusDetails(_bl, b);
            bd.Show();
        }

        private void InActive_Click(object sender, RoutedEventArgs e)
        {
            wnd.DataDisplay.Content = new InActiveBusesViewPage(_bl);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (BusesDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please choose at least one bus and then click remove!");
            }
            else
            {
                var lb = (IEnumerable)(BusesDataGrid.SelectedItems);

                foreach (var b in lb)
                {
                    _bl.DeleteBus(((Bus)b).LicenseNum);
                }
                BusesDataGrid.DataContext = _bl.GetAllBuses().ToList(); //update view
            }
        }
    }
}
