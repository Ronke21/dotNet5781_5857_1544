using System;
using BLApi;
using BO;
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
        private readonly IBL _bl;
        private readonly BusStation currentBusStation;
        MainWindow wnd = (MainWindow)Application.Current.MainWindow; //reference to main window in order to update list box items(buses)
        private SimulationPage _simulationPage;

        public UpdateStation(IBL b, BusStation bs, SimulationPage sp)
        {
            InitializeComponent();

            _bl = b;
            currentBusStation = bs;
            _simulationPage = sp;

            DetailsGrid.DataContext = currentBusStation;
        }

        private void Update_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var nam = TextBoxName.Text;
                var add = TextBoxAddress.Text;
                double.TryParse(TextBoxLongitude.Text, out var lon);
                double.TryParse(TextBoxLatitude.Text, out var lat);
                var access = CheckBoxAccessible.IsChecked != null && (bool) CheckBoxAccessible.IsChecked;

                var updated = new BusStation
                {
                    Code = currentBusStation.Code,
                    Name = nam,
                    Address = add,
                    Location = new GeoCoordinate(lat, lon),
                    Accessible = access,
                    Active = currentBusStation.Active
                };

                _bl.UpdateBusStation(updated);
                Close();
            }
            catch (BO.BadUpdateException ex)
            {
                MessageBox.Show(ex.Message, "Station updating Error!");
                return;
            }
            catch (BO.GeneralErrorException ex)
            {
                MessageBox.Show(ex.Message, "Station updating Error!");
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Unknown error", "Station updating Error!");
                return;
            }
            finally
            {
                TextBoxLongitude.Text = currentBusStation.Location.Longitude.ToString();
                TextBoxLatitude.Text = currentBusStation.Location.Latitude.ToString();
            }

            wnd.DataDisplay.Content = new StationsViewPage(_bl,_simulationPage);

            Close();
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
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
    }


}

