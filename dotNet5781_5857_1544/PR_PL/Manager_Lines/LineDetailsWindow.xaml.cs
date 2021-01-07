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
using System.Windows.Shapes;
using System.Device.Location;
using BLApi;
using BO;

namespace PR_PL.Manager_Lines
{
    /// <summary>
    /// Interaction logic for LineDetailsWindow.xaml
    /// </summary>
    public partial class LineDetailsWindow : Window
    {
        private readonly IBL _bl;
        private readonly BusLine bline;
        public LineDetailsWindow(IBL b, BusLine busLine)
        {
            InitializeComponent();

            _bl = b;
            bline = busLine;

            BusLineDetailsGrid.DataContext = _bl.GetBusLine(bline.BusLineId);
            StationDataGrid.DataContext = _bl.UpdateAndReturnLineStationList(bline.BusLineId);
        }

        private void Map_Click(object sender, RoutedEventArgs e)
        {
            var current = new LineStation();

            if (sender is Button btn)
            {
                current = (LineStation)btn.DataContext;
            }

            var toSend = new BusStation()
            {
                
                Active = current.Active,
                Address = current.Address,
                Code = current.Code,
                Location = current.Location,
                Name = current.Name,
                Accessible=current.Accessible 
            };
            var smw = new PR_PL.Manager_Stations.ShowMapWindow(toSend);
            smw.ShowDialog();
        }
    }
}
