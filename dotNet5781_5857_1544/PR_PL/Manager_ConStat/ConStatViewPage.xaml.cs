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
    /// Interaction logic for ConStatViewPage.xaml
    /// </summary>
    public partial class ConStatViewPage : Page
    {
        private readonly IBL _bl;
        MainWindow wnd = (MainWindow)Application.Current.MainWindow;

        public ConStatViewPage(IBL b)
        {
            InitializeComponent();

            _bl = b;

            Refresh();
        }

        private void Refresh()
        {
            try
            {
                ConStatDataGrid.ItemsSource = _bl.GetAllConsecutiveStations();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't load the list of consecutive stations! \n" + ex.Message, "Station Loading Error!");
            }
        }

        private void Update_OnClick(object sender, RoutedEventArgs e)
        {
            var cs = (ConsecutiveStations)ConStatDataGrid.SelectedItem;
            
            if(cs is null) return;

            var edw = new EditDistanceWindow(_bl, cs);
            edw.ShowDialog();
        }

        private void SearchLinesTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ConStatDataGrid.ItemsSource = _bl.GetAllConsecutiveStationsByCode(SearchLinesTextBox.Text).ToList();
        }
    }
}
