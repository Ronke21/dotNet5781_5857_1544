﻿using System.Linq;
using BLApi;
using BO;
using System.Windows;
using System.Windows.Input;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationDetails.xaml
    /// </summary>
    public partial class StationDetails : Window
    {
        private readonly IBL _bl;
        private readonly BusStation currentBS;
        private SimulationPage _simulationPage;

        public StationDetails(IBL b, BusStation bs, SimulationPage sp)
        {
            InitializeComponent();

            currentBS = bs;

            _bl = b;
            _simulationPage = sp;


            StationDetailsWindow.DataContext = currentBS;

            var lines = _bl.LinesInStation(currentBS.Code).ToList();

            LineNumbersTextBlock.DataContext = lines.Aggregate("", (current, line) => current + line.LineNumber+", ");
        }

        private void Update_OnClick(object sender, RoutedEventArgs e)
        {
            if (currentBS.Active == false)
            {
                MessageBox.Show("Please activate the bus and then update!");
                return;
            }
            var us = new UpdateStation(_bl, currentBS, _simulationPage);
            Close();
            us.ShowDialog();
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
