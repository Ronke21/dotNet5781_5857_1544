using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLApi;
using BO;
using PL;

namespace PR_PL.Manager_Lines
{
    /// <summary>
    /// Interaction logic for LinesViewPage.xaml
    /// </summary>

    public partial class LinesViewPage : Page
    {
        MainWindow wnd = (MainWindow)Application.Current.MainWindow;
        private readonly IBL _bl;
        public LinesViewPage(IBL b)
        {
            InitializeComponent();

            _bl = b;

            LinesDataGrid.ItemsSource = _bl.GetAllActiveBusLines();
        }
        private void LinesDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var x = sender as DataGrid;

            if (x != null && !(x.SelectedItem is BusLine))
            {
                // do nothing
            }

            else
            {
                var ldw = new LineDetailsWindow(_bl, (BusLine)LinesDataGrid.SelectedItem);
                ldw.Show();
            }
        }

        private void Remove_OnClick(object sender, RoutedEventArgs e)
        {
            if (LinesDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please choose at least one bus and then click remove!");
            }
            else
            {
                var lb = (IEnumerable)(LinesDataGrid.SelectedItems);

                foreach (var b in lb)
                {
                    _bl.DeleteBusLine(((BusLine)b).BusLineId);
                }
                LinesDataGrid.ItemsSource = _bl.GetAllActiveBusLines();
            }
        }

        private void InActive_OnClick(object sender, RoutedEventArgs e)
        {
            wnd.DataDisplay.Content = new InActiveLinesViewPage(_bl);
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            var abl = new AddBusLineWindow(_bl);
            abl.ShowDialog();
        }
    }

}
