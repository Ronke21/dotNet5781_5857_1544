using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationsViewPage.xaml
    /// </summary>
    public partial class StationsViewPage : Page
    {
        private readonly IBL _bl;
        MainWindow wnd = (MainWindow)Application.Current.MainWindow;
        private SimulationPage _simulationPage;
        private bool simulation;
        public StationsViewPage(IBL b, SimulationPage sp)
        {
            InitializeComponent();

            _bl = b;
            _simulationPage = sp;

            Refresh();
        }

        private void StationsDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_bl.IsSimulatorRunning())
            {
                var sds = new StationDetailsSimulator(_bl, StationsDataGrid.SelectedItem as BusStation, _simulationPage);
                sds.ShowDialog();
            }
            else
            {
                var sd = new StationDetails(_bl, StationsDataGrid.SelectedItem as BusStation, _simulationPage);
                sd.ShowDialog();
            }
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

                Refresh();
            }
        }

        private void InActive_OnClick(object sender, RoutedEventArgs e)
        {
            wnd.DataDisplay.Content = new InActiveStationsViewPage(_bl, _simulationPage);
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            var ast = new AddStation(_bl);
            ast.ShowDialog();
        }
        private void Refresh()
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

        private void SearchLinesTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            StationsDataGrid.DataContext = _bl.GetAllBusStationsByCodeOrName(SearchLinesTextBox.Text).ToList();          
        }
    }
}
