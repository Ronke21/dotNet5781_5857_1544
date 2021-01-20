using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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

            var x = bl.getAllLineExits();

            DataGridLE.ItemsSource = x;
        }

        private void AddLineExit_Onclick(object sender, RoutedEventArgs e)
        {
            var t = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            bl.AddLineExit(new LineExit()
            {
                Active = true,
                StartTime = t,
                Freq = 25,
                EndTime = t.Add(new TimeSpan(8,0,0)),
                BusLineId = 12
            });
        }
    }
}
