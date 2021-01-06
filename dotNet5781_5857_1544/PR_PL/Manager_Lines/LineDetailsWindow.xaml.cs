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
        public LineDetailsWindow(IBL b, BusLine busLine)
        {
            InitializeComponent();

            _bl = b;

            BusLineDetailsGrid.DataContext = _bl.GetBusLine(busLine.BusLineId);

            StationDataGrid.DataContext = busLine.ListOfLineStations.ToList();
        }
    }
}
