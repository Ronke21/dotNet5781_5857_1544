using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLApi;
using BO;
using PL;
using PR_PL.Manager_Simulation;

namespace PR_PL.Manager_Stations
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

        public StationDetailsSimulator(IBL b, BusStation bs, SimulationPage sp)
        {
            InitializeComponent();

            currentBS = bs;
            _simulationPage = sp;

            _bl = b;

            StationDetailsWindow.DataContext = currentBS;

            simulatorWorker = new BackgroundWorker() { WorkerSupportsCancellation = true };
            simulatorWorker.DoWork += Worker_UpdatePanels;

            #region list of lines
            var lines = _bl.ListForYellowSign(currentBS.Code);
            YellowLinesDisplayDataGrid.ItemsSource = lines;
            DigitalDisplayDataGrid.ItemsSource = lines;
            #endregion

            simulatorWorker.RunWorkerAsync();
        }

        private void Worker_UpdatePanels(object sender, DoWorkEventArgs e)
        {
            //_bl.(, UpdateArrivingTimes);
        }

        private void UpdateArrivingTimes()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                DigitalDisplayDataGrid.ItemsSource = _bl.ListForYellowSign(currentBS.Code);
            }));
        }


        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            simulatorWorker.CancelAsync();
            Close();
        }

        private void Update_OnClick(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Can't update during simulation\ngo back to simulation page to stop simulation", "ERROR", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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
