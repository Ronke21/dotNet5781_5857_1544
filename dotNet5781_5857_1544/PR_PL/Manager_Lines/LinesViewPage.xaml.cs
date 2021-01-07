using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using BLApi;
using BO;

namespace PR_PL.Manager_Lines
{
    /// <summary>
    /// Interaction logic for LinesViewPage.xaml
    /// </summary>

    public partial class LinesViewPage : Page
    {
        private readonly IBL _bl;
        public LinesViewPage(IBL b)
        {
            InitializeComponent();

            _bl = b;

            LinesDataGrid.ItemsSource = _bl.GetAllActiveBusLines().ToList();

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
            throw new System.NotImplementedException();
        }

        private void InActive_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            var abl = new AddBusLineWindow(_bl);
            abl.ShowDialog();
        }
    }

}
