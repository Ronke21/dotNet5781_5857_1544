﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace PR_PL
{
    /// <summary>
    /// Interaction logic for UpdateBus.xaml
    /// </summary>
    public partial class UpdateBus : Window
    {
        private Bus bus;
        private IBL bl;
        public UpdateBus(IBL b, Bus bu)
        {
            InitializeComponent();

            bus = bu;
            bl = b;

            UpdateBusWindow.DataContext = bus;
        }

        private void Update_OnClick(object sender, RoutedEventArgs e)
        {
            //int.TryParse((string) LicenseNumLabel.Content, out var License);
            try
            {
                int.TryParse(TextBoxFuel.Text, out var fuel);
                int.TryParse(TextBoxMileage.Text, out var mileage);
                //DateTime.TryParse((string)StartLabel.Content, out var start);
                DateTime.TryParse(DpLastMaint.Text, out var last);
                int.TryParse(TextBoxMileageFromLast.Text, out var mileageFromLast);

                var updated = new Bus
                {
                    LicenseNum = bus.LicenseNum,
                    Fuel = fuel,
                    Mileage = mileage,
                    StartTime = bus.StartTime,
                    LastMaint = last,
                    MileageFromLast = mileageFromLast
                };
                bl.UpdateBus(updated);
                Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            if (e.Handled)
            {
                MessageBox.Show($"digits only\n'{e.Text}' is not a digit");
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                MessageBox.Show("Space is not allowed");
            }
        }
    }
}
