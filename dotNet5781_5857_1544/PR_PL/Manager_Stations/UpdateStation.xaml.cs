using BLApi;
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
        }

        private void Update_OnClick(object sender, RoutedEventArgs e)
        {
            //int.TryParse((string) LicenseNumLabel.Content, out var License);
            try
            {
                string nam = TextBoxName.Text;
                string add = TextBoxAddress.Text;
                double.TryParse(TextBoxLongitude.Text, out double longi);
                double.TryParse(TextBoxLatitude.Text, out double lati);
                bool accessi = (bool)CheckBoxAccessible.IsChecked;

                try
                {
                    BO.BusStation ToAdd = new BusStation()
                    {
                        Code = currentBusStation.Code,
                        Name = nam,
                        Address = add,
                        Location = new GeoCoordinate(lati, longi),
                        Accessible = accessi,
                        Active = true
                    };
                    bl.AddStation(ToAdd);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
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
            Regex regex = new Regex("[^0-9]+[.]");
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
