using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLApi;
using BO;
namespace PL
{
    /// <summary>
    /// Interaction logic for AddLineExit.xaml
    /// </summary>
    public partial class AddLineExit : Window
    {
        private IBL _bl;
        private BusLine bline;
        public AddLineExit(IBL b, BusLine bl)
        {
            InitializeComponent();

            _bl = b;
            bline = bl;

            FreqPicker.ItemsSource = new List<int> { 0, 5, 10, 15, 20, 30, 45, 60 };
            FreqPicker.SelectedItem = 0;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            DateTime s = (DateTime)StartTimePicker.SelectedTime;
            int f = (int)FreqPicker.SelectedItem;
            DateTime en = (DateTime)EndTimePicker.SelectedTime;


            LineExit toAdd = new LineExit()
            {
                StartTime = new TimeSpan(s.Hour, s.Minute, s.Second),
                Freq = new TimeSpan(0, f, 0),
                EndTime = new TimeSpan(en.Hour, en.Minute, en.Second),
                LineNumber = bline.LineNumber,
                BusLineId = bline.BusLineId,
                Active = true
            };
            try
            {
                _bl.AddLineExit(toAdd);
            }
            catch (BO.BadAdditionException ex)
            {
                MessageBox.Show(ex.Message, "can't add the line exit");
            }
            Close();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void FreqPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((int)FreqPicker.SelectedItem == 0)
            {
                EndTimePicker.SelectedTime = StartTimePicker.SelectedTime;
                EndTimePicker.IsEnabled = false;
            }
            else
            {
                EndTimePicker.IsEnabled = true;
            }
        }

        private void StartTimePicker_OnSelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            if ((int)FreqPicker.SelectedItem == 0)
            {
                EndTimePicker.SelectedTime = StartTimePicker.SelectedTime;
            }
        }
    }

}
