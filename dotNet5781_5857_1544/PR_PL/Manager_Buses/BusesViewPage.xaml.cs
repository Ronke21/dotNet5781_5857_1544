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
        private readonly MainWindow _wnd = (MainWindow)Application.Current.MainWindow;
        public BusesViewPage(IBL b)
        {
            _bl = b;

            InitializeComponent();

            Refresh();
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            var ab = new AddBus(_bl);
            ab.ShowDialog();
            Refresh();
        }

        private void BusesDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var b = (Bus)BusesDataGrid.SelectedItem;
            var bd = new BusDetails(_bl, b);
            bd.Show();
        }

        private void InActive_Click(object sender, RoutedEventArgs e)
        {
            _wnd.DataDisplay.Content = new InActiveBusesViewPage(_bl);
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
                    try
                    {
                        _bl.DeleteBus(((Bus)b).LicenseNum);
                    }
                    catch (BO.DoesNotExistException ex)
                    {
                        MessageBox.Show(ex.Message, "Buses Loading Error!");
                    }
                }

                Refresh();

            }
        }

        private void Refresh()
        {
            try
            {
                BusesDataGrid.DataContext = _bl.GetAllBuses();
            }
            catch (BO.EmptyListException ex)
            {
                MessageBox.Show(ex.Message, "Buses Loading Error!");
            }
        }
    }
}
