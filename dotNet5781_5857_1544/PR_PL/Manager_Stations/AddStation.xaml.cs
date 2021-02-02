using System;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Device.Location;
using BLApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for AddStation.xaml
    /// </summary>
    public partial class AddStation : Window
    {
        private readonly IBL _bl;
        public AddStation(IBL b)
        {
            InitializeComponent();

            _bl = b;
        }
        private void TextBoxWithPeriod_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
            if (e.Handled)
            {
                MessageBox.Show($"digits only\n'{e.Text}' is not a digit");
            }
        }

        private void TextBoxNumbersOnly_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            if (e.Handled)
            {
                MessageBox.Show($"digits only\n'{e.Text}' is not a digit");
            }
        }

        //protected override void OnPreviewKeyDown(KeyEventArgs e)
        //{
        //    var a = e;

        //    if (e.Key == Key.Space)
        //    {
        //        e.Handled = true;
        //        MessageBox.Show("Space is not allowed");
        //    }
        //}

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {

            int.TryParse(TextBoxCode.Text, out var code);
            var name = TextBoxName.Text;
            var address = TextBoxAddress.Text;
            double.TryParse(TextBoxLongitude.Text, out var longitude);
            double.TryParse(TextBoxLatitude.Text, out var latitude);
            var accessible = (bool)CheckBoxAccessible.IsChecked;

            if (code <= 0)
            {
                MessageBox.Show("Station code must be positive number!");
                TextBoxCode.Text = "";
                return;
            }
            if (longitude <= 0 || latitude <= 0)
            {
                MessageBox.Show("Station Location must be positive number!");
                TextBoxLongitude.Text = "";
                TextBoxLatitude.Text = "";
                return;
            }

            try
            {
                _bl.AddStation(new BusStation()
                {
                    Code = code,
                    Name = name,
                    Address = address,
                    Location = new GeoCoordinate(latitude, longitude),
                    Accessible = accessible,
                    Active = true
                });
            }
            catch (BO.StationAlreadyExistsException ex)
            {
                MessageBox.Show(ex.Message, "Station adding Error!");
                EmptyBoxes();
                return;
            }
            catch (BO.BadAdditionException ex)
            {
                MessageBox.Show(ex.Message, "Station adding Error!");
                EmptyBoxes();
                return;
            }
            catch (BO.GeneralErrorException ex)
            {
                MessageBox.Show(ex.Message, "Station adding Error!");
                EmptyBoxes();
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Unknown error!");
                EmptyBoxes();
                return;
            }
            finally
            {

            }
            Close();
        }

        private void EmptyBoxes()
        {
            TextBoxCode.Text = "";
            TextBoxName.Text = "";
            TextBoxAddress.Text = "";
            TextBoxLongitude.Text = "";
            TextBoxLatitude.Text = "";
            CheckBoxAccessible.IsChecked = false;
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


