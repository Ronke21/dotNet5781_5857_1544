using System;
using System.Collections.Generic;
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
        private readonly BusStation currentBS;
        MainWindow wnd = (MainWindow)Application.Current.MainWindow;
        private SimulationPage _simulationPage;
        private List<LineTiming> lineTimings = new List<LineTiming>();
        public StationDetailsSimulator(IBL b, BusStation bs, SimulationPage sp)
        {
            InitializeComponent();

            currentBS = bs;
            _simulationPage = sp;

            _bl = b;

            StationDetailsWindow.DataContext = currentBS;

            simulatorWorker = new BackgroundWorker() { WorkerSupportsCancellation = true };
            simulatorWorker.DoWork += Worker_UpdatePanels;

            YellowLinesDisplayDataGrid.ItemsSource = _bl.ListForYellowSign(currentBS.Code);
            StationDetailsTextBlock.Text = currentBS.Name + "\n" + "station number: " + currentBS.Code;

            var lines = _bl.LinesInStation(currentBS.Code).ToList();
            LineNumbersTextBlock.DataContext = lines.Aggregate("", (current, line) => current + line.LineNumber + ", ");

            simulatorWorker.RunWorkerAsync();
        }

        private void Worker_UpdatePanels(object sender, DoWorkEventArgs e)
        {
            _bl.SetStationPanel(currentBS.Code, UpdateArrivingTimes);
        }

        private void UpdateArrivingTimes(LineTiming lineTiming)
        {
            lineTimings.Remove((from lt in lineTimings
                                where lt.BusLineId == lineTiming.BusLineId &&
                                      lt.StartTime == lineTiming.StartTime
                                select lt).First());
            if (lineTiming.ArrivalTime != TimeSpan.Zero) lineTimings.Add(lineTiming);

            // get lineTimings and add it to the digital sign
            Dispatcher.BeginInvoke(new Action(() =>
            {
                DigitalDisplayDataGrid.ItemsSource = lineTimings.OrderBy(l => l.ArrivalTime);
            }));
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
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
            var smw = new ShowMapWindow(currentBS);
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
