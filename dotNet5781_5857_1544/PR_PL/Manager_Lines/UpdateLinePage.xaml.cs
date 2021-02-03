using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BLApi;
using BO;
using PR_PL.Manager_Lines;

namespace PL
{
    /// <summary>
    /// Interaction logic for UpdateLinePage.xaml
    /// </summary>
    public partial class UpdateLinePage : Page
    {

        MainWindow wnd = (MainWindow)Application.Current.MainWindow; //reference to main window in order to update list box items(buses)

        private List<BO.BusStation> _chosen;
        private List<BO.BusStation> _chooseFrom;

        private readonly IBL _bl;
        private readonly BusLine bline;
        private readonly Window father;

        public UpdateLinePage(IBL b, BusLine busLine, Window w)
        {
            InitializeComponent();

            father = w;

            _bl = b;
            bline = busLine;

            _chosen = new List<BusStation>(_bl.GetLineBusStations(bline.BusLineId));
            _chooseFrom = new List<BusStation>(_bl.GetAllBusStations().Where(s => _chosen.All(x => s.Code != x.Code)).OrderBy(s => s.Code));

            StationsDataGrid.ItemsSource = _chooseFrom;
            ChosenStationsDataGrid.ItemsSource = _chosen;
            ExitsDataGrid.DataContext = _bl.GetAllLineExitsByLine(bline.BusLineId);


            RefreshDataGrids();

            LineAreaComboBox.ItemsSource = Enum.GetValues(typeof(Area));
            LineAreaComboBox.SelectedItem = bline.BusArea;
            LineNumberBox.Text = (bline.LineNumber).ToString(); // how to do it in xaml
        }

        private void RefreshDataGrids()
        {
            StationsDataGrid.ItemsSource = _bl.GetAllMatches(SearchLinesTextBox.Text, _chooseFrom).OrderBy(s => s.Code);
            StationsDataGrid.Items.Refresh();
            ChosenStationsDataGrid.Items.Refresh();
            ExitsDataGrid.DataContext = _bl.GetAllLineExitsByLine(bline.BusLineId);
            Colors();
        }

        private void Colors()
        {
            if (_chosen.Count < 2)
            {
                ChosenBorder.BorderBrush = Brushes.Red;
                ChosenBorder.BorderThickness = new Thickness(5);
            }
            else
            {
                var bc = new BrushConverter();
                ChosenBorder.BorderBrush = (Brush)bc.ConvertFrom("#007ACC");
                ChosenBorder.BorderThickness = new Thickness(1);
            }
        }

        private void Update_OnClick(object sender, RoutedEventArgs e)
        {
            //var fromDG = (ChosenStationsDataGrid.Items.OfType<BusStation>()).ToList();

            var from = _chosen.ToList();

            int.TryParse(LineNumberBox.Text, out var lineNum);
            var lineArea = (Area)LineAreaComboBox.SelectedItem;
            var accessible = _chosen.All(station => station.Accessible);

            //if (_chosen.Count < 2 || lineNum == 0 || lineNum > 999) return;

            try
            {
                var newBus = new BO.BusLine()
                {
                    BusLineId = bline.BusLineId,
                    LineNumber = lineNum,
                    Active = true,
                    AllAccessible = accessible,
                    BusArea = lineArea,
                    //FirstStation = _chosen.ToList()[0].Code,
                    //LastStation = _chosen.ToList()[_chosen.Count > 1 ? _chosen.Count - 1 : 0].Code
                };

                var index = 0;

                var stations = from station in _chosen
                               select new LineStation()
                               {
                                   Code = station.Code,
                                   Accessible = station.Accessible,
                                   Active = station.Active,
                                   Address = station.Address,
                                   StationIndex = index++,
                                   Location = station.Location,
                                   Name = station.Name,
                                   BusLineId = bline.BusLineId,
                               };

                newBus.ListOfLineStations = stations;

                _bl.UpdateBusLine(newBus, _chosen);

                wnd.DataDisplay.Content = new LinesViewPage(_bl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "can't update ");
            }


            father.Close();
        }

        private void Remove_OnClick(object sender, RoutedEventArgs e)
        {
            if (ChosenStationsDataGrid.SelectedItem is null) return;
            _chooseFrom.Add((BusStation)ChosenStationsDataGrid.SelectedItem);
            _chosen.Remove((BusStation)ChosenStationsDataGrid.SelectedItem);
            RefreshDataGrids();
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            if (StationsDataGrid.SelectedItem is null) return;
            _chosen.Add((BusStation)StationsDataGrid.SelectedItem);
            _chooseFrom.Remove((BusStation)StationsDataGrid.SelectedItem);
            RefreshDataGrids();
        }

        private void SearchLinesTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            StationsDataGrid.DataContext = _bl.GetAllMatches(SearchLinesTextBox.Text, _chooseFrom);
            RefreshDataGrids();
        }

        private void AddLineExit_OnClick(object sender, RoutedEventArgs e)
        {
            var ale = new AddLineExit(_bl, bline);
            ale.ShowDialog();
            ExitsDataGrid.DataContext = _bl.GetAllLineExitsByLine(bline.BusLineId);

        }
        private void DeleteLineExit_OnClick(object sender, RoutedEventArgs e)
        {
            LineExit exitL;
            if (ExitsDataGrid.SelectedItem is null)
            {
                MessageBox.Show("Please choose line exit to delete");
                return;
            }
            else
            {
                exitL = (LineExit)ExitsDataGrid.SelectedItem;
            }

            try
            {
                _bl.DeleteLineExit(exitL.BusLineId, exitL.StartTime);
            }

            catch( BO.BadUpdateException ex)
            {
                MessageBox.Show(ex.Message, "Can not delete line exit");
            }

            ExitsDataGrid.DataContext = _bl.GetAllLineExitsByLine(bline.BusLineId);

        }
    }
}