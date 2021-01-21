using System;
using System.Windows;
using BLApi;
using BO;

namespace PR_PL
{
    /// <summary>
    /// Interaction logic for LineExitWindowDelete.xaml
    /// </summary>
    public partial class LineExitWindowDelete : Window
    {
        private IBL bl;
        public LineExitWindowDelete(IBL b)
        {
            InitializeComponent();

            bl = b;

            var x = bl.GetAllLineExits();

            DataGridLE.ItemsSource = x;
        }

        private void AddLineExit_Onclick(object sender, RoutedEventArgs e)
        {
            var t = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            bl.AddLineExit(new LineExit()
            {
                Active = true,
                StartTime = t,
                Freq = new TimeSpan(0,25,0),
                EndTime = t.Add(new TimeSpan(8,0,0)),
                BusLineId = 21
            });
        }
    }
}
