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

            _bl=b;
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

            if (cod<=0)
            {
                MessageBox.Show("Station code must be positive number!");
                TextBoxCode.Text = "";
                return;
            }
            if (longi <= 0 || lati<=0)
            {
                MessageBox.Show("Station Location must be positive number!");
                TextBoxLongitude.Text = "";
                TextBoxLatitude.Text = "";
                return;
            }

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
                _bl.AddStation(ToAdd);
            }
            catch (BO.StationAlreadyExistsException ex)
            {
                MessageBox.Show(ex.Message, "Station adding Error!");
                emptyBoxes();
                return;
            }

            Close();
        }

        void emptyBoxes()
        {
            TextBoxCode.Text = "";
            TextBoxName.Text = "";
            TextBoxAddress.Text = "";
            TextBoxLongitude.Text = "";
            TextBoxLatitude.Text = "";
        }
    }

}


