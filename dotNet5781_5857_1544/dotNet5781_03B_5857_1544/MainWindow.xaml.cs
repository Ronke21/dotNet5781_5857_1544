using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;
using System.ComponentModel;
using System.Diagnostics;

namespace dotNet5781_03B_5857_1544
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static Random r = new Random(DateTime.Now.Millisecond);

        private Bus CurrentDisplay;
        public static List<Bus> Eged = new List<Bus>(); // a list of buses - our data base!

        //BackgroundWorker worker;

        public MainWindow()
        {
            InitializeComponent();

            //worker = new BackgroundWorker();
            //worker.DoWork += Worker_DoWork;
            //worker.ProgressChanged += Worker_ProgressChanged;
            //worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            //worker.WorkerReportsProgress = true;
            //worker.WorkerSupportsCancellation = true; ;


            for (int i = 0; i < 5; i++)
            {
                Eged.Add(new Bus(r.Next(1000000, 9999999), new DateTime(r.Next(1948, 2018), r.Next(1, 13), 1), r.Next(1201), r.Next(19000), DateTime.Now.AddMonths(r.Next(-20, -1))));
                Eged.Add(new Bus(r.Next(10000000, 99999999), new DateTime(r.Next(2018, 2021), r.Next(1, 13), 1), r.Next(1201), r.Next(19000), DateTime.Now.AddMonths(r.Next(-20, -1))));
            }

            Eged[0].lastMaintDate = DateTime.Now.AddMonths(-13);
            Eged[0].setStatus();

            Eged[1].lastMaintDate = DateTime.Now.AddMonths(-11);
            Eged[1].setStatus();

            Eged[2].Fuel = 10;
            Eged[2].setStatus();

            Eged[3].lastMaintDate = (DateTime.Today).AddDays(-3);
            Eged[3].setStatus(); // = MaintainSoon

            LbBuses.ItemsSource = Eged;

        }


        #region Sort
        private void Sort_by_ID(object sender, RoutedEventArgs e)
        {
            Eged.Sort((bus1, bus2) => bus1.LICENSENUMINT.CompareTo(bus2.LICENSENUMINT));
            LbBuses.Items.Refresh();
        }
        //private void Sort_by_last_maint(object sender, RoutedEventArgs e)
        //{
        //    Eged.Sort((bus1, bus2) => (bus1.MILEAGE - bus1.lastMaintMileage).CompareTo((bus2.MILEAGE - bus2.lastMaintMileage)));
        //    LbBuses.Items.Refresh();
        //}
        private void Sort_by_fuel_Amount(object sender, RoutedEventArgs e)
        {
            Eged.Sort((bus1, bus2) => bus2.Fuel.CompareTo(bus1.Fuel));
            LbBuses.Items.Refresh();
        }
        //private void Sort_by_Mileage(object sender, RoutedEventArgs e)
        //{
        //    Eged.Sort((bus1, bus2) => bus1.MILEAGE.CompareTo(bus2.MILEAGE));
        //    LbBuses.Items.Refresh();
        //}
        //private void Sort_by_last_Maintenance(object sender, RoutedEventArgs e)
        //{
        //    // sort by the time passed from the last maintenance
        //    Eged.Sort((bus1, bus2) => bus1.lastMaintDate.CompareTo(bus2.lastMaintDate));
        //    LbBuses.Items.Refresh();
        //}
        private void Sort_by_status(object sender, RoutedEventArgs e)
        {
            // different solution in order to implement stable sort, because many buses are likely to share status

            IEnumerable<Bus> b = Eged.OrderBy(bus => bus.BUSSTATE).ToList();

            if (Eged.SequenceEqual(b)) return;
            Eged.Clear();
            foreach (var bus in b)
            {
                Eged.Add(bus);
            }
            LbBuses.Items.Refresh();
        }


        #endregion

        //       private void Refuel(object sender, RoutedEventArgs e)
        //       {
        //           if (sender != null && sender is Button btn) CurrentDisplay = (Bus)btn.DataContext;
        //            LbBuses.SelectedItem = null;
        //           Button cmd = (Button)sender;
        //           var p = cmd.Parent as Grid;
        //           var s = p.Children[6] as ProgressBar;
        //           s.Value = 0;
        ////           Bus CurrentBus = (Bus)cmd.DataContext;
        //           Thread t2 = new Thread(() =>
        //           {
        //               if (CurrentDisplay.Fuel < 1200)
        //               {
        //                   CurrentDisplay.BUSSTATE = dotNet5781_03B_5857_1544.Status.Refueling;
        //                   for (int i = 0; i < 12; i++)
        //                   {
        //                       s.Value += 1;
        //                       Thread.Sleep(1000);
        //                   }
        //                   CurrentDisplay.Fuel = 1200;
        //                   CurrentDisplay.setStatus();
        //               }
        //           }
        //           );

        //           t2.Start();
        //       }


        private async void Refuel(object sender, RoutedEventArgs e)
        {
            if (sender != null && sender is Button btn) CurrentDisplay = (Bus)btn.DataContext;
            LbBuses.SelectedItem = null;

            Progress<Reporter> reportProgress = new Progress<Reporter>();
            //    reportProgress.ProgressChanged += reportFuelAmount;

            CurrentDisplay.BUSSTATE = dotNet5781_03B_5857_1544.Status.Refueling;
            LbBuses.Items.Refresh();

            if (CurrentDisplay.BUSSTATE == dotNet5781_03B_5857_1544.Status.InMaintenance)
            {
                MessageBox.Show("bus is in maintenance, no need to refuel twice");
            }

            else if (CurrentDisplay.BUSSTATE == dotNet5781_03B_5857_1544.Status.During)
            {
                MessageBox.Show("bus is in a ride, wait until it gets back");
            }

            else
            {
                //if (worker.IsBusy != true) 
                //    worker.RunWorkerAsync(12); // Start the asynchronous operation

                await RefuelAsync(reportProgress);

                LbBuses.Items.Refresh();
            }
        }

        private async Task RefuelAsync(IProgress<Reporter> progress)
        {
            Reporter reporter = new Reporter();
            await Task.Run(() => CurrentDisplay.Refuel());

            reporter.PercentageComplete = CurrentDisplay.Fuel;
            progress.Report(reporter);
        }

        private void Add_Bus_to_Eged(object sender, RoutedEventArgs e)
        {
            AddBusWindow addingWin = new AddBusWindow();
            addingWin.ShowDialog();
            LbBuses.Items.Refresh();
        }

        void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender != null && sender is Button btn) CurrentDisplay = (Bus)btn.DataContext;
            ChooseBusWindow chooseBus = new ChooseBusWindow(CurrentDisplay);
            LbBuses.SelectedItem = null;

            if (!CurrentDisplay.qualifiedDate())
            {
                MessageBox.Show("this bus is not qualified for a ride\ntake it to maintenance");
            }

            else
            {
                chooseBus.ShowDialog();
                LbBuses.Items.Refresh();
            }
        }


        private void LbBuses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CurrentDisplay = (Bus)LbBuses.SelectedItem;
            listDoubleClick doubleC = new listDoubleClick(CurrentDisplay);
            LbBuses.SelectedItem = null;
            doubleC.ShowDialog();
            LbBuses.Items.Refresh();
        }
        
        private void EXIT_OnClick(object sender, RoutedEventArgs e)
        {
            /*
            ApproveClosing AC = new ApproveClosing();
            var ans = AC.ShowDialog();
            if((bool)ans) Close();
            */
            Close();
        }

        //private void Worker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    Stopwatch stopwatch = new Stopwatch(); 
        //    stopwatch.Start();
        //    int length = (int)e.Argument;
        //    for (int i = 1; i <= length; i++)
        //    {
        //        if (worker.CancellationPending == true)
        //        {
        //            e.Cancel = true;
        //            e.Result = stopwatch.ElapsedMilliseconds; // Unnecessary
        //            break;
        //        }
        //        else
        //        {             // Perform a time consuming operation and report progress.       
        //            System.Threading.Thread.Sleep(500);
        //            worker.ReportProgress(i * 100 / length);
        //        }
        //    }
        //    e.Result = stopwatch.ElapsedMilliseconds;
        //}

        //private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    int progress = e.ProgressPercentage;
        //    resultLabel.Content = (progress + "%");
        //    resultProgressBar.Value = progress;
        //}

        //private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    if (e.Cancelled == true)
        //    {                 // e.Result throw System.InvalidOperationException                 
        //        resultLabel.Content = "Canceled!";
        //    }
        //    else if (e.Error != null)
        //    {                 // e.Result throw System.Reflection.TargetInvocationException   
        //        resultLabel.Content = "Error: " + e.Error.Message; // Exception Message            
        //    }
        //    else
        //    {
        //        long result = (long)e.Result;
        //        if (result < 1000)
        //            resultLabel.Content = "Done after " + result + " ms.";
        //        else
        //            resultLabel.Content = "Done after " + result / 1000 + " sec.";
        //    }


        //}
    }

}

