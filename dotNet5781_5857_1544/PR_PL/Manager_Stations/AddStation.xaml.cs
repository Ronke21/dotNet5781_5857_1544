using System;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Device.Location;
using BLApi;
using BO;
using PR_PL;

namespace PR_PL
{
    /// <summary>
    /// Interaction logic for AddStation.xaml
    /// </summary>
    public partial class AddStation : Window
    {
        private readonly IBL bl;
        public AddStation(IBL b)
        {
            InitializeComponent();

            bl=b;
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

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            int.TryParse(TextBoxCode.Text, out int cod);
            string nam=TextBoxName.Text;
            string add = TextBoxAddress.Text;
            double.TryParse(TextBoxLongitude.Text, out double longi);
            double.TryParse(TextBoxLatitude.Text, out double lati);
            bool accessi = (bool)CheckBoxAccessible.IsChecked;
            
            try
            {
                BO.BusStation ToAdd = new BusStation()
                {
                    Code = cod,
                    Name = nam,
                    Address=add,
                    Location=new GeoCoordinate(lati, longi),
                    Accessible=accessi,
                    Active=true
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
    }

}


