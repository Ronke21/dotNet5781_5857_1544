using System;
using System.Windows;
using BLApi;
using BO;
using PL;

namespace PR_PL.Manager_ConStat
{
    /// <summary>
    /// Interaction logic for EditDistanceWindow.xaml
    /// </summary>
    public partial class EditDistanceWindow : Window
    {
        MainWindow wnd = (MainWindow)Application.Current.MainWindow; //reference to main window in order to update list box items(buses)

        private IBL _bl;
        private ConsecutiveStations cs;
        public EditDistanceWindow(IBL b, ConsecutiveStations c)
        {
            InitializeComponent();

            _bl = b;
            cs = c;

            DistanceTextBox.Text = (cs.Distance).ToString();
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Update_OnClick(object sender, RoutedEventArgs e)
        {
            var dist = Convert.ToDouble(DistanceTextBox.Text);
            cs.Distance = dist;
            _bl.UpdateConsecutiveStations(cs);
            wnd.DataDisplay.Content = new ConStatViewPage(_bl);
            Close();
        }
    }
}
