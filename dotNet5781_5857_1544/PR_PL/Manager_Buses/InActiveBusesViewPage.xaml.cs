using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for InActiveBusesViewPage.xaml
    /// </summary>

    public partial class InActiveBusesViewPage : Page
    {
        private readonly MainWindow _wnd = (MainWindow)Application.Current.MainWindow;
        private readonly IBL _bl;
        public InActiveBusesViewPage(IBL b)
        {
            InitializeComponent();

            _bl = b;

            Refresh();
        }

        private void InActiveBusesDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var b = (Bus)InActiveBusesDataGrid.SelectedItem;
            var bd = new BusDetails(_bl, b);
            bd.Show();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            _wnd.DataDisplay.Content = new BusesViewPage(_bl);
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

                Refresh();
            }
        }

        private void Refresh()
        {
            try
            {
                InActiveBusesDataGrid.DataContext = _bl.GetAllInActiveBuses();
            }
            catch (BO.EmptyListException)
            {
                MessageBox.Show("Empty list", "");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Unknown error");
            }
        }

        private void SearchLinesTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            InActiveBusesDataGrid.DataContext = _bl.GetAllInActiveBusesByCode(SearchLinesTextBox.Text).ToList();
        }

    }
}