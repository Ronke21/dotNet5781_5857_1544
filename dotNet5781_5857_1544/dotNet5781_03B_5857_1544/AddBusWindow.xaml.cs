using System;
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

namespace dotNet5781_03B_5857_1544
{
    /// <summary>
    /// Interaction logic for AddBusWindow.xaml
    /// </summary>
    public partial class AddBusWindow : Window
    {
        public AddBusWindow()
        {
            InitializeComponent();
        }

        private void TextBoxBusLicenseNumber_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(TextBoxBusLicenseNumber.Text, out int num);
            if (num > 99999999)
            {
                MessageBox.Show("not valid");
            }
        }
        private void TextBoxBusLicenseNumber_OnMouseLeave(object sender, MouseEventArgs e)
        {
            int.TryParse(TextBoxBusLicenseNumber.Text, out int num);
            if (TextBoxBusLicenseNumber.Text != String.Empty)
            {
                if (num < 1000000)
                {
                    MessageBox.Show("not valid");
                }
            }
        }
        private void TextBoxFuelAmount_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(TextBoxFuelAmount.Text, out int fuel);
            if (fuel < 0 || fuel > 1200)
            {
                MessageBox.Show("not valid");
            }
        }
        private void TextBoxMileage_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(TextBoxMileage.Text, out int mileage);
            if (mileage < 0 || mileage > 1000000)
            {
                MessageBox.Show("not valid");
            }
        }
        private void DatePickerStart_OnCalendarClosed(object sender, RoutedEventArgs e)
        {
            if (DatePickerStart.Text == String.Empty)
            {
                DatePickerStart.SelectedDate = DateTime.Today;
            }
            DateTime.TryParse(DatePickerStart.Text, out DateTime last);
            if (DatePickerStart.SelectedDate != DateTime.Today)
            {
                if (last > DateTime.Today || last < new DateTime(2000, 1, 1))
                {
                    MessageBox.Show("not valid");
                }
            }
        }
        private void DatePickerLastMaintenance_OnCalendarClosed(object sender, RoutedEventArgs e)
        {
            if (DatePickerLastMaintenance.Text == String.Empty)
            {
                DatePickerLastMaintenance.SelectedDate = DateTime.Today;
            }
            DateTime.TryParse(DatePickerLastMaintenance.Text, out DateTime last);
            if (DatePickerLastMaintenance.SelectedDate != DateTime.Today)
            {
                if (last > DateTime.Today || last < new DateTime(2000, 1, 1))
                {
                    MessageBox.Show("not valid");
                }
            }
        }
        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            DateTime.TryParse(DatePickerStart.Text, out DateTime start);
            DateTime.TryParse(DatePickerLastMaintenance.Text, out DateTime last);

            int.TryParse(TextBoxBusLicenseNumber.Text, out int license);

            int.TryParse(TextBoxFuelAmount.Text, out int fuel);
            int.TryParse(TextBoxMileage.Text, out int mileage);

            if (start.Year > 2017)
            {
                if (license < 1000000)
                {
                    MessageBox.Show("");
                }
            }
            else if (license > 99999999)
            {
                MessageBox.Show("");
            }

            MainWindow.Eged.Add(new Bus(license, start, fuel, mileage, last));

            Close();
        }
    }
}
