using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
        private List<LineTiming> lineTimings = new List<LineTiming>();
        private ObservableCollection<LineTiming> lineTimesObsColl = new ObservableCollection<LineTiming>();
        public StationDetailsSimulator(IBL b, BusStation busStation, SimulationPage sp)
        {
            InitializeComponent();

            DigitalDisplayDataGrid.ItemsSource = lineTimesObsColl;
            


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
                var x = ((from lt in lineTimings
                          where lt.BusLineId == lineTiming.BusLineId &&
                                lt.StartTime == lineTiming.StartTime
                          select lt).FirstOrDefault());
                if (x!=null)
                {
                    lineTimesObsColl.Remove(x);
                }
                if (lineTiming.ArrivalTime != TimeSpan.Zero)
                {
                    lineTimesObsColl.Add(lineTiming);
                }


                /*
                if (lineTimings.Count >0)
                {
                lineTimings.Remove((from lt in lineTimings
                                    where lt.BusLineId == lineTiming.BusLineId &&
                                          lt.StartTime == lineTiming.StartTime
                                    select lt).First());
                }

                if (lineTiming.ArrivalTime != TimeSpan.Zero)
                {
                    lineTimings.Add(lineTiming);
                }

                DigitalDisplayDataGrid.ItemsSource = lineTimings.OrderBy(l => l.ArrivalTime);
            */
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

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
