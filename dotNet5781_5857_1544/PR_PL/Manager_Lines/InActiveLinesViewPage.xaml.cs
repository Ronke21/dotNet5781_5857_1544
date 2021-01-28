using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for InActiveLinesViewPage.xaml
    /// </summary>
    public partial class InActiveLinesViewPage : Page
    {
        MainWindow wnd = (MainWindow)Application.Current.MainWindow;
        private readonly IBL _bl;
        public InActiveLinesViewPage(IBL b)
        {
            InitializeComponent();

            _bl = b;

            InActiveLinesDataGrid.ItemsSource = _bl.GetAllInActiveBusLines();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            wnd.DataDisplay.Content = new LinesViewPage(_bl);
        }

        private void Activate_OnClick(object sender, RoutedEventArgs e)
        {
            if (InActiveLinesDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please choose at least one bus line and then click activate!");
            }

            else
            {
                _bl.ActivateBusLine(((BusLine)InActiveLinesDataGrid.SelectedItem).BusLineId);
                InActiveLinesDataGrid.ItemsSource = _bl.GetAllInActiveBusLines();
            }
        }

        private void InActiveLinesDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid x && !(x.SelectedItem is BusLine)) return;
            var ldc = new LineDoubleClick(_bl, (BusLine)InActiveLinesDataGrid.SelectedItem);
            ldc.Show();
        }
    }

}
