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

            refresh();
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
                    try
                    {
                        _bl.ActivateBus(b.LicenseNum);

                    }
                    catch (BO.BusDoesNotExistsException ex)
                    {
                        MessageBox.Show(ex.Message, "Bus activating Error!");
                    }
                }

                refresh();
            }
        }

        private void refresh()
        {
            try
            {
                InActiveBusesDataGrid.DataContext = _bl.GetAllInActiveBuses();
            }
            catch (BO.EmptyListException ex)
            {
                MessageBox.Show(ex.Message, "Buses Lodaing Error!");
            }
        }
    }
}