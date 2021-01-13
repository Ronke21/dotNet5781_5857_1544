using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BLApi;
using BO;
using PL;

namespace PR_PL.Manager_Lines
{
    /// <summary>
    /// Interaction logic for UpdateLinePage.xaml
    /// </summary>
    public partial class UpdateLinePage : Page
    {

        MainWindow wnd = (MainWindow)Application.Current.MainWindow; //reference to main window in order to update list box items(buses)

        private ObservableCollection<BO.BusStation> _chosen;
        private ObservableCollection<BO.BusStation> _chooseFrom;

        private readonly IBL _bl;
        private readonly BusLine bline;
        private readonly Window father;

        public UpdateLinePage(IBL b, BusLine busLine, Window w)
        {
            InitializeComponent();

            father = w;

            _bl = b;
            bline = busLine;

            _chosen = new ObservableCollection<BusStation>(_bl.GetLineBusStations(bline.BusLineId));
            _chooseFrom = new ObservableCollection<BusStation>(_bl.GetAllBusStations().Where(s => _chosen.All(x => s.Code != x.Code)).OrderBy(s => s.Code));

            StationsDataGrid.ItemsSource = _chooseFrom;
            ChosenStationsDataGrid.ItemsSource = _chosen;

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
        }
        private void Update_OnClick(object sender, RoutedEventArgs e)
        {
            int.TryParse(LineNumberBox.Text, out var lineNum);
            var lineArea = (Area)LineAreaComboBox.SelectedItem;
            var accessible = _chosen.All(station => station.Accessible);

            var newBus = new BO.BusLine()
            {
                BusLineId = bline.BusLineId,
                LineNumber = lineNum,
                Active = true,
                AllAccessible = accessible,
                BusArea = lineArea,
                FirstStation = _chosen.ToList()[0].Code,
                LastStation = _chosen.ToList()[_chosen.Count - 1].Code
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


            try
            {
                _bl.UpdateBusLine(newBus);

                wnd.DataDisplay.Content = new LinesViewPage(_bl);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
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
        }
    }
}