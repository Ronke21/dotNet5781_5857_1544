﻿using BLApi;
using BO;
using System.Windows;
using System.Windows.Input;
using PR_PL.Manager_Stations;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationDetails.xaml
    /// </summary>
    public partial class StationDetails : Window
    {
        private readonly IBL _bl;
        private readonly BusStation currentBS;
        public StationDetails(IBL b, BusStation bs)
        {
            InitializeComponent();

            currentBS = bs;

            _bl = b;

            StationDetailsWindow.DataContext = currentBS;
        }

        private void Update_OnClick(object sender, RoutedEventArgs e)
        {
            if (currentBS.Active == false)
            {
                MessageBox.Show("Please activate the bus and then update!");
                return;
            }
            UpdateStation us = new UpdateStation(_bl, currentBS);
            us.ShowDialog();
            Close();
        }

        private void Map_OnClick(object sender, RoutedEventArgs e)
        {
            var smw = new ShowMapWindow(currentBS);
            smw.ShowDialog();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
