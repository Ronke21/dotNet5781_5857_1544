using BLApi;
using BO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationsView.xaml
    /// </summary>
    public partial class StationsView : Window
    {
        private readonly IBL _bl;
        public StationsView(IBL b)
        {
            InitializeComponent();

            _bl = b;

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

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            AddStation ast = new AddStation(_bl);
            ast.ShowDialog();
            try
            {
                StationsDataGrid.DataContext = _bl.GetAllBusStations().ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't load the list of active stations! \n" + ex.Message, "Station Loading Error!");
            }
        }

        private void StationsDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var bd = new StationDetails(_bl, (BusStation)StationsDataGrid.SelectedItem);
            bd.Show();
        }

        private void InActive_Click(object sender, RoutedEventArgs e)
        {
            InActiveStationsView iasv = new InActiveStationsView(_bl, this);
            iasv.ShowDialog();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
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
                    catch (Exception ex)
                    {
                        MessageBox.Show("Can't delete Bus station number " + (((BusStation)s).Code).ToString() + ex.Message, "Station deleting Error!");
                    }
                }

                try
                {
                StationsDataGrid.DataContext = _bl.GetAllBusStations().ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Can't load the list of active stations! \n" + ex.Message, "Station Loading Error!");
                }
            }
        }

    }
}
