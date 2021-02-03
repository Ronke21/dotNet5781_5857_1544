using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using BLApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationDetailsSimulator.xaml
    /// </summary>


    public partial class StationDetailsSimulator : Window
    {
        BackgroundWorker simulatorWorker;
        private readonly IBL _bl;
        private readonly BusStation currentBusStation;
        MainWindow wnd = (MainWindow)Application.Current.MainWindow;
        private SimulationPage _simulationPage;
        //private IEnumerable<LineTiming> lineTimings = new List<LineTiming>();
        private ObservableCollection<LineTiming> lineTimesObsColl = new ObservableCollection<LineTiming>();
        public StationDetailsSimulator(IBL b, BusStation busStation, SimulationPage sp)
        {
            InitializeComponent();

            DigitalDisplayDataGrid.ItemsSource = lineTimesObsColl;

            // add a new sorting rule, sort by "StartTime"
            DigitalDisplayDataGrid.Items.SortDescriptions.Add(new SortDescription("StartTime", ListSortDirection.Ascending));

            currentBusStation = busStation;
            _simulationPage = sp;

            _bl = b;

            StationDetailsWindow.DataContext = currentBusStation;

            simulatorWorker = new BackgroundWorker() { WorkerSupportsCancellation = true };
            simulatorWorker.DoWork += Worker_UpdatePanels;

            YellowLinesDisplayDataGrid.ItemsSource = _bl.ListForYellowSign(currentBusStation.Code);
            StationDetailsTextBlock.Text = currentBusStation.Name + "\n" + "station number: " + currentBusStation.Code;

            var lines = _bl.LinesInStation(currentBusStation.Code).ToList();
            LineNumbersTextBlock.DataContext = lines.Aggregate("", (current, line) => current + line.LineNumber + ", ");

            simulatorWorker.RunWorkerAsync();
        }

        private void Worker_UpdatePanels(object sender, DoWorkEventArgs e)
        {
            _bl.SetStationPanel(currentBusStation.Code, UpdateArrivingTimes);
        }

        private void UpdateArrivingTimes(LineTiming lineTiming)
        {

            // get lineTimings and add it to the digital sign
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var x = (from lt in lineTimesObsColl
                         where lt.BusLineId == lineTiming.BusLineId &&
                               lt.StartTime == lineTiming.StartTime
                         select lt).FirstOrDefault();

                if (x != null)
                {
                    lineTimesObsColl.Remove(x);
                    //if (x.ArrivalTime == TimeSpan.Zero)
                    {
                        DigitalDisplayLastBus.ItemsSource = new ObservableCollection<LineTiming> { x };
                    }
                }
                if (lineTiming.ArrivalTime != TimeSpan.Zero && lineTiming.ArrivalTime < new TimeSpan(0, 45, 0))
                {
                    lineTimesObsColl.Add(lineTiming);
                }

            }));
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            _bl.StopStationPanel(UpdateArrivingTimes);
            simulatorWorker.CancelAsync();
            Close();
        }

        private void Update_OnClick(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Can't update during simulation\ngo back to simulation page to stop simulation",
                                                "ERROR", MessageBoxButton.YesNo,
                                                MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Close();
                wnd.DataDisplay.Content = _simulationPage;
            }
        }

        private void Map_OnClick(object sender, RoutedEventArgs e)
        {
            var smw = new ShowMapWindow(currentBusStation);
            smw.ShowDialog();
        }

        //private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        DragMove();
        //    }
        //}
    }
}
