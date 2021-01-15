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

            refresh();
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
                MessageBox.Show("Please choose at least one station and then click remove!", "Selection Error");
            }
            else
            {
                foreach (var s in StationsDataGrid.SelectedItems)
                {
                    try
                    {
                        _bl.DeleteBusStation(((BusStation)s).Code);
                    }
                    catch (BO.StationBelongsToActiveBusLine ex)
                    {
                        MessageBox.Show(ex.Message, "Station deleting Error!");
                    }
                    catch (BO.DoesNotExistException ex)
                    {
                        MessageBox.Show(ex.Message, "Station deleting Error!");
                    }

                }

                refresh();
            }
        }

        private void InActive_OnClick(object sender, RoutedEventArgs e)
        {
            wnd.DataDisplay.Content =new InActiveStationsViewPage(_bl);
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            var ast = new AddStation(_bl);
            ast.ShowDialog();

        }
        private void refresh()
        {
            try
            {
                StationsDataGrid.DataContext = _bl.GetAllBusStations().ToList();
            }
            catch (BO.EmptyListException e)
            {
                MessageBox.Show(e.Message, "Station Loading Error!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unknown ERROR!" + ex.Message, "Station Loading Error!");
            }
        }
    }
}
