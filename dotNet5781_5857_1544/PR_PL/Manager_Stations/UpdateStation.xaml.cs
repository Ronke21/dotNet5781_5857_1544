﻿using BLApi;
using BO;
using System;
using System.Device.Location;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace PL
{
    /// <summary>
    /// Interaction logic for UpdateStation.xaml
    /// </summary>
    public partial class UpdateStation : Window
    {
        private readonly IBL bl;
        private readonly BusStation currentBusStation;
        public UpdateStation(IBL b, BusStation bs)
        {
            InitializeComponent();

            bl = b;
            currentBusStation = bs;

            DetailsGrid.DataContext = currentBusStation;
            TextBoxLongitude.Text = currentBusStation.Location.Longitude.ToString();
            TextBoxLatitude.Text = currentBusStation.Location.Latitude.ToString();
        }

        private void Update_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var nam = TextBoxName.Text;
                var add = TextBoxAddress.Text;
                double.TryParse(TextBoxLongitude.Text, out var lon);
                double.TryParse(TextBoxLatitude.Text, out var lat);
                var access = CheckBoxAccessible.IsChecked != null && (bool)CheckBoxAccessible.IsChecked;

                var updated = new BusStation
                {
                    Code = currentBusStation.Code,
                    Name = nam,
                    Address = add,
                    Location = new GeoCoordinate(lat, lon),
                    Accessible = access,
                    Active = currentBusStation.Active
                };

                bl.UpdateBusStation(updated);
                Close();
            }


            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }

            Close();
        }


        private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+[.]");
            e.Handled = regex.IsMatch(e.Text);
            if (e.Handled)
            {
                MessageBox.Show($"digits only\n'{e.Text}' is not a digit");
            }
        }

        //protected override void OnPreviewKeyDown(KeyEventArgs e)
        //{
        //    if (e.Key == Key.Space)
        //    {
        //        e.Handled = true;
        //        MessageBox.Show("Space is not allowed");
        //    }
        //}

    }


}

