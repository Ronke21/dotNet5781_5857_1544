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
    /// Interaction logic for InActiveStationsViewPage.xaml
    /// </summary>
    public partial class InActiveStationsViewPage : Page
    {
        private readonly IBL _bl;
        MainWindow wnd = (MainWindow)Application.Current.MainWindow;
        private SimulationPage _simulationPage;

        public InActiveStationsViewPage(IBL b, SimulationPage sp)
        {
            InitializeComponent();

            _bl = b;
            _simulationPage = sp;

            refresh();
        }

        private void refresh()
        {
            try
            {
                InActiveStationsDataGrid.DataContext = _bl.GetAllInActiveBusStations().ToList();
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

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            wnd.DataDisplay.Content = new StationsViewPage(_bl, _simulationPage);
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

                    try
                    {
                        _bl.ActivateBusStation(bs.Code);
                    }
                    catch (BO.StationDoesNotExistException ex)
                    {
                        MessageBox.Show(ex.Message, "Station activating Error!");
                    }
                }

                refresh();

            }
        }

        private void InActiveStationsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var bd = new StationDetails(_bl, InActiveStationsDataGrid.SelectedItem as BusStation, _simulationPage);
            bd.Show();
        }

        private void SearchLinesTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            InActiveStationsDataGrid.DataContext = _bl.GetAllInActiveBusStationsByCodeOrName(SearchLinesTextBox.Text).ToList();
        }
    }
}
