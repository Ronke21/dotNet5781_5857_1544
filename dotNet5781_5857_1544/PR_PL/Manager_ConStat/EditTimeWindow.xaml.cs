using System;
using System.Windows;
using System.Windows.Input;
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

            // TimePicker.SelectedTime = new DateTime(1, 1, 1, cs.AverageTravelTime.Minutes, cs.AverageTravelTime.Seconds,0);
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Update_OnClick(object sender, RoutedEventArgs e)
        {
            var time = (DateTime)Clock.Time;
            cs.AverageTravelTime = new TimeSpan(time.Hour, time.Minute, time.Second);
            _bl.UpdateConsecutiveStations(cs);
            wnd.DataDisplay.Content = new ConStatViewPage(_bl);
            Close();
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
