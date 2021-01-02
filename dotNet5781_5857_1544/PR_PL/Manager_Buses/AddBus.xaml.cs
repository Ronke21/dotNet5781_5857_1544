using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using BLApi;
using BO;

namespace PR_PL
{
    /// <summary>
    /// Interaction logic for AddBus.xaml
    /// </summary>
    public partial class AddBus : Window
    {
        private IBL bl;
        public AddBus(IBL b)
        {
            InitializeComponent();

            bl = b;
        }
        
        private void DatePicker_OnCalendarClosed(object sender, RoutedEventArgs e)
        {
            if (DatePickerStart.Text == String.Empty)
            {
                DatePickerStart.SelectedDate = DateTime.Today;
            }
            DateTime.TryParse(DatePickerStart.Text, out DateTime last);
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

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            DateTime.TryParse(DatePickerStart.Text, out DateTime start);
            DateTime.TryParse(DatePickerLastMaintenance.Text, out DateTime last);
            int.TryParse(TextBoxBusLicenseNumber.Text, out int license);
            double.TryParse(TextBoxFuelAmount.Text, out double fuel);
            double.TryParse(TextBoxMileage.Text, out double mileage);
            double.TryParse(TextBoxMileageSinceLastMaint.Text, out double mileageSinceLast);

            try
            {
                BO.Bus ToAdd = new Bus()
                {
                    LicenseNum = license,
                    Fuel = fuel,
                    Mileage = mileage,
                    Active = true,
                    StartTime = start,
                    LastMaint = last,
                    MileageFromLast = mileageSinceLast
                };
                bl.AddBus(ToAdd);
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
