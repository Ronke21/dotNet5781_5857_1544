using System;
using System.Windows;
using System.Windows.Input;
using BLApi;
using BO;

namespace PL
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

            OpenEditTimeWindow();
        }

        private void Update_OnClick(object sender, RoutedEventArgs e)
        {
            var dist = Convert.ToDouble(DistanceTextBox.Text);
            cs.Distance = dist;
            _bl.UpdateConsecutiveStations(cs);
            wnd.DataDisplay.Content = new ConStatViewPage(_bl);
            Close();

            OpenEditTimeWindow();
        }

        private void OpenEditTimeWindow()
        {
            var etw = new EditTimeWindow(_bl, cs);
            etw.ShowDialog();
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
