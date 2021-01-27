using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLApi;
using BO;
using PL;
using PR_BL.BO;
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
            YellowLinesDisplayDataGrid.ItemsSource = _bl.ListForYellowSign(currentBS.Code);
            #endregion

            simulatorWorker.RunWorkerAsync();
        }

        private void Worker_UpdatePanels(object sender, DoWorkEventArgs e)
        {
            _bl.UpdateStationDigitalSign(currentBS.Code, UpdateArrivingTimes);
        }

        private void UpdateArrivingTimes(IEnumerable<LineNumberAndFinalDestination> l)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                DigitalDisplayDataGrid.ItemsSource = l.ToList();
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
