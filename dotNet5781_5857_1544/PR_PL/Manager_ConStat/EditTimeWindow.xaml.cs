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
using PL;

namespace PR_PL.Manager_ConStat
{
    /// <summary>
    /// Interaction logic for EditTimeWindow.xaml
    /// </summary>
    public partial class EditTimeWindow : Window
    {
        MainWindow wnd = (MainWindow)Application.Current.MainWindow; //reference to main window in order to update list box items(buses)

        private IBL _bl;
        private ConsecutiveStations cs;
        public EditTimeWindow(IBL b, ConsecutiveStations c)
        {
            InitializeComponent();

            _bl = b;
            cs = c;
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Update_OnClick(object sender, RoutedEventArgs e)
        {
            var time = (DateTime)TimePicker.SelectedTime;
            cs.AverageTravelTime = new TimeSpan(0, time.Hour, time.Minute);
            _bl.UpdateConsecutiveStations(cs);
            wnd.DataDisplay.Content = new ConStatViewPage(_bl);
            Close();
        }
    }
}
