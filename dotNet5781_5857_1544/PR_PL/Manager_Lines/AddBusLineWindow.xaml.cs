using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLApi;
using BO;
using PL;

namespace PR_PL.Manager_Lines
{
    /// <summary>
    /// Interaction logic for AddBusLineWindow.xaml
    /// </summary>


    public partial class AddBusLineWindow : Window
    {
        private readonly IBL _bl;

        MainWindow wnd = (MainWindow)Application.Current.MainWindow; //reperence to main window in order to update list box items(buses)
        
        private ObservableCollection<BO.BusStation> _chosen = new ObservableCollection<BusStation>();
        private ObservableCollection<BO.BusStation> _chooseFrom;
        public AddBusLineWindow(IBL b)
        {
            InitializeComponent();

            _bl = b;

            _chooseFrom = new ObservableCollection<BusStation>(_bl.GetAllBusStations().OrderBy(s => s.Code));

            StationsDataGrid.DataContext = _chooseFrom;
            ChosenStationsDataGrid.DataContext = _chosen;
            RefreshDataGrids();

            LineAreaComboBox.ItemsSource = Enum.GetValues(typeof(Area));
            LineAreaComboBox.SelectedItem = Area.General;
        }

        private void RefreshDataGrids()
        {
            StationsDataGrid.DataContext = _bl.GetAllMatches(SearchLinesTextBox.Text, _chooseFrom).OrderBy(s=>s.Code);
            StationsDataGrid.Items.Refresh();
            ChosenStationsDataGrid.Items.Refresh();
        }

        private void SearchLinesTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            StationsDataGrid.DataContext = _bl.GetAllMatches(SearchLinesTextBox.Text, _chooseFrom);
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            if (StationsDataGrid.SelectedItem is null) return;
            _chosen.Add((BusStation)StationsDataGrid.SelectedItem);
            _chooseFrom.Remove((BusStation)StationsDataGrid.SelectedItem);
            RefreshDataGrids();
        }

        private void Remove_OnClick(object sender, RoutedEventArgs e)
        {
            if (ChosenStationsDataGrid.SelectedItem is null) return;
            _chooseFrom.Add((BusStation)ChosenStationsDataGrid.SelectedItem);
            _chosen.Remove((BusStation)ChosenStationsDataGrid.SelectedItem);
            RefreshDataGrids();
        }

        private void AddLine_OnClick(object sender, RoutedEventArgs e)
        {
            int.TryParse(LineNumberBox.Text, out var lineNum);
            var lineArea = (Area)LineAreaComboBox.SelectedItem;
            var accessible = _chosen.All(station => station.Accessible);

            try
            {
                _bl.AddBusLine(new BO.BusLine() {
                    LineNumber = lineNum,
                    Active = true,
                    AllAccessible = accessible,
                    BusArea = lineArea,
                    FirstStation = _chosen.ToList()[0].Code,
                    LastStation = _chosen.ToList()[_chosen.Count - 1].Code,
                }, _chosen.ToList());

                wnd.DataDisplay.Content = new LinesViewPage(_bl);

                Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }

        }
    }
}
