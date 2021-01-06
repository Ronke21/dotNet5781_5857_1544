﻿using BLApi;
using BO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationsView.xaml
    /// </summary>
    public partial class StationsView : Window
    {
        private readonly IBL _bl;
        public StationsView(IBL b)
        {
            InitializeComponent();

            _bl = b;

            StationsDataGrid.DataContext = _bl.GetAllBusStations().ToList();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            AddStation ast = new AddStation(_bl);
            ast.ShowDialog();
            StationsDataGrid.DataContext = _bl.GetAllBusStations().ToList();
        }

        private void StationsDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var bd = new StationDetails(_bl, (BusStation)StationsDataGrid.SelectedItem);
            bd.Show();
        }

        private void InActive_Click(object sender, RoutedEventArgs e)
        {
            InActiveStationsView iasv = new InActiveStationsView(_bl, this);
            iasv.ShowDialog();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (StationsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please choose at least one station and then click remove!");
            }
            else
            {
                foreach (var s in StationsDataGrid.SelectedItems)
                {
                    _bl.DeleteBusStation(((BusStation)s).Code);
                }
                StationsDataGrid.DataContext = _bl.GetAllBusStations().ToList();
            }
        }

    }
}