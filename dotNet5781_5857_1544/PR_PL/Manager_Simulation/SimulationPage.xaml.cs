using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using BLApi;
using MaterialDesignThemes.Wpf;
using PL;

namespace PR_PL.Manager_Simulation
{
    /// <summary>
    /// Interaction logic for SimulationPage.xaml
    /// </summary>
    public partial class SimulationPage : Page
    {
        BackgroundWorker simulatorWorker;

        private bool running;
        //private BackgroundWorker simulationWorker = null;
        private TimeSpan myTimeSpan;
        private int ratio;

        private readonly IBL _bl;
        MainWindow wnd = (MainWindow)Application.Current.MainWindow;

        public SimulationPage(IBL b)
        {
            InitializeComponent();

            _bl = b;

            simulatorWorker = new BackgroundWorker() { WorkerSupportsCancellation = true };
            simulatorWorker.DoWork += Worker_StartSimulation;
        }

        private void Worker_StartSimulation(object sender, DoWorkEventArgs e)
        {
            _bl.StartSimulator(myTimeSpan, ratio, UpdateClock); // UpdateClock is 'Action' Delegate
        }


        private void UpdateClock(TimeSpan ts)
        {
            myTimeSpan = ts;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                timerTextBlock.Text = myTimeSpan.ToString(@"hh\:mm\:ss");
                ClockExp.Time = new DateTime(1, 1, 1, 0, 0, 0) + myTimeSpan;
                TimePicker.SelectedTime = ClockExp.Time;
            }));
        }

        private void StartStop_OnClick(object sender, RoutedEventArgs e)
        {
            if (running)
            {
                running = false;
                StartStopIcon.Kind = PackIconKind.Play;
                RateSlider.IsEnabled = true;
                TimePicker.IsEnabled = true;


                simulatorWorker.CancelAsync();
                _bl.StopSimulator();
            }
            else
            {
                running = true;
                StartStopIcon.Kind = PackIconKind.Stop;
                RateSlider.IsEnabled = false;
                TimePicker.IsEnabled = false;

                // activated by GUI thread, no need for dispatcher!
                myTimeSpan = TimePicker.SelectedTime.Value.TimeOfDay;
                ratio = (int) RateSlider.Value;
                simulatorWorker.RunWorkerAsync();
            }
        }
    }
}
