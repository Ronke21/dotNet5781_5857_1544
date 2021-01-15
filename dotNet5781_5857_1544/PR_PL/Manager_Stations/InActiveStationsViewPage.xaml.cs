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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BLApi;
using BO;
using PL;

namespace PR_PL.Manager_Stations
{
    /// <summary>
    /// Interaction logic for InActiveStationsViewPage.xaml
    /// </summary>
    public partial class InActiveStationsViewPage : Page
    {
        private readonly IBL _bl;
        MainWindow wnd = (MainWindow)Application.Current.MainWindow;
        public InActiveStationsViewPage(IBL b)
        {
            InitializeComponent();

            _bl = b;

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
            wnd.DataDisplay.Content = new StationsViewPage(_bl);
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
            var bd = new StationDetails(_bl, InActiveStationsDataGrid.SelectedItem as BusStation);
            bd.Show();
        }
    }
}
