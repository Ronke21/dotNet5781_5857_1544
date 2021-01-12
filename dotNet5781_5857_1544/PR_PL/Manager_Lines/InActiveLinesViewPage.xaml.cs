using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace PR_PL.Manager_Lines
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
                MessageBox.Show("Please choose at least one bus and then click activate!");
            }

            else
            {
                var bl = (IEnumerable)(InActiveLinesDataGrid.SelectedItems);

                foreach (BusLine b in bl)
                {
                    //b.Active = true;
                    var updated = new BusLine()
                    {
                        AllAccessible = b.AllAccessible,
                        Active = true,
                        BusArea = b.BusArea,
                        BusLineId = b.BusLineId,
                        FirstStation = b.FirstStation,
                        LastStation = b.LastStation,
                        LineNumber = b.LineNumber
                    };
                    _bl.UpdateBusLine(updated);
                }

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
