﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLApi;
using BO;

namespace PR_PL.Manager_Lines
{
    /// <summary>
    /// Interaction logic for LineDetailsPage.xaml
    /// </summary>
    public partial class LineDetailsPage : Page
    {
        private readonly IBL _bl;
        private readonly BusLine bline;

        public LineDetailsPage(IBL b, BusLine busLine)
        {
            InitializeComponent();

            _bl = b;
            bline = busLine;

            BusLineDetailsGrid.DataContext = _bl.GetBusLine(bline.BusLineId);
            StationDataGrid.DataContext = _bl.UpdateAndReturnLineStationList(bline.BusLineId);
        }

        private void Map_Click(object sender, RoutedEventArgs e)
        {
            var current = new LineStation();

            if (sender is Button btn)
            {
                current = (LineStation)btn.DataContext;
            }

            var toSend = new BusStation()
            {

                Active = current.Active,
                Address = current.Address,
                Code = current.Code,
                Location = current.Location,
                Name = current.Name,
                Accessible = current.Accessible
            };
            var smw = new PR_PL.Manager_Stations.ShowMapWindow(toSend);
            smw.ShowDialog();
        }
    }
}
