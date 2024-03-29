﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for LinesViewPage.xaml
    /// </summary>

    public partial class LinesViewPage : Page
    {
        MainWindow wnd = (MainWindow)Application.Current.MainWindow;
        private readonly IBL _bl;
        public LinesViewPage(IBL b)
        {
            InitializeComponent();

            _bl = b;

            try
            {
                Refresh();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        private void LinesDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid x && !(x.SelectedItem is BusLine)) return;
            var ldc = new LineDoubleClick(_bl, (BusLine)LinesDataGrid.SelectedItem);
            ldc.ShowDialog();
        }

        private void Remove_OnClick(object sender, RoutedEventArgs e)
        {
            if (LinesDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please choose at least one bus and then click remove!");
            }
            else
            {
                var lb = (IEnumerable)(LinesDataGrid.SelectedItems);

                foreach (var b in lb)
                {
                    _bl.DeleteBusLine(((BusLine)b).BusLineId);
                }

                Refresh();
            }
        }

        private void InActive_OnClick(object sender, RoutedEventArgs e)
        {
            wnd.DataDisplay.Content = new InActiveLinesViewPage(_bl);
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            var abl = new AddBusLineWindow(_bl);
            abl.ShowDialog();
            Refresh();
        }

        private void Refresh()
        {
            LinesDataGrid.ItemsSource = _bl.GetAllActiveBusLines();
            AreaCountIC.ItemsSource = _bl.groupLineByAreas();
        }
    }

}
