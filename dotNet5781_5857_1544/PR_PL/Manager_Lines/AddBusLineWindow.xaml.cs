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
    /// Interaction logic for AddBusLineWindow.xaml
    /// </summary>
    public partial class AddBusLineWindow : Window
    {
        private IBL _bl;

        private readonly List<BO.LineStation> _chosen = new List<LineStation>();
        public AddBusLineWindow(IBL b)
        {
            InitializeComponent();
            
            _bl = b;

            refresh();

            //StationsDataGrid.DataContext = _bl.GetAllBusStations().Except(_chosen);

            //ChosenStationsDataGrid.DataContext = _chosen;
        }

        private void refresh()
        {
            StationsDataGrid.DataContext = _bl.GetAllBusStations().Except(_chosen);

            ChosenStationsDataGrid.DataContext = _chosen;
        }

        private void SearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SearchLinesTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Remove_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
