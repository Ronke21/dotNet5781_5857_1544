﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BLApi;
using BO;
using PR_PL;

namespace PL
{
    /// <summary>
    /// Interaction logic for InActiveBusesView.xaml
    /// </summary>
    public partial class InActiveBusesView : Window
    {
        private readonly IBL bl;
        public InActiveBusesView(IBL b)
        {
            InitializeComponent();

            bl = b;

            BusesDataGrid.DataContext = bl.GetAllInActiveBuses().ToList();
        }
        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BusesDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Bus b = (Bus)BusesDataGrid.SelectedItem;
            BusDetails bd = new BusDetails(bl, b);
            bd.Show();
        }

        private void Activate(object sender, RoutedEventArgs e)
        {
            Bus b = (Bus)BusesDataGrid.SelectedItem;
            var updated = new Bus
            {
                LicenseNum = b.LicenseNum,
                Fuel = b.fuel,
                Mileage = b.mileage,
                StartTime = b.StartTime,
                LastMaint = b.last,
                MileageFromLast = b.mileageFromLast,
                Active = True
            };
            bl.UpdateBus(updated);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}