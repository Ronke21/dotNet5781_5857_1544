﻿using System;
using System.Windows;
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
            var time = (DateTime)TimePicker.SelectedTime;
            cs.AverageTravelTime = new TimeSpan(0, time.Hour, time.Minute);
            _bl.UpdateConsecutiveStations(cs);
            wnd.DataDisplay.Content = new ConStatViewPage(_bl);
            Close();
        }
    }
}