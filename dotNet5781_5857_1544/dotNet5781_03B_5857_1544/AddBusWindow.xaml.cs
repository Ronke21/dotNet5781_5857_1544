using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace dotNet5781_03B_5857_1544
{
    /// <summary>
    /// Interaction logic for AddBusWindow.xaml
    /// This window opens when the user asks to add a new bus - (The + button in main window)
    /// </summary>
    public partial class AddBusWindow : Window
    {
        public AddBusWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This function makes sure the text box recives a valid bus number - not more then 8 digits
        /// </summary>
        private void TextBoxBusLicenseNumber_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(TextBoxBusLicenseNumber.Text, out int num);
            if (num > 99999999)
            {
                MessageBox.Show("not valid");
            }
        }

        /// <summary>
        /// This function makes sure the text box recives a valid bus number - only digits
        /// </summary>
        private void TextBoxBusLicenseNumber_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            if (e.Handled)
            {
                MessageBox.Show($"digits only\n'{e.Text}' is not a digit");
            }
        }

        /// <summary>
        /// This function makes sure the text box recives a valid bus number - not less then 7 digits
        /// </summary>
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

        /// <summary>
        /// This function makes sure the text box recives a valid fuel number - between 0-1200
        /// </summary>
        private void TextBoxFuelAmount_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(TextBoxFuelAmount.Text, out int fuel);
            if (fuel < 0 || fuel > 1200)
            {
                MessageBox.Show("not valid");
            }
        }

        /// <summary>
        /// This function makes sure the text box recives a valid fuel number - only digits
        /// </summary>
        private void TextBoxFuelAmount_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            if (e.Handled)
            {
                MessageBox.Show($"digits only\n'{e.Text}' is not a digit");
            }
        }

        /// <summary>
        /// This function makes sure the text box recives a valid mileage number - between 0-1000000
        /// </summary> 
        private void TextBoxMileage_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(TextBoxMileage.Text, out int mileage);
            if (mileage < 0 || mileage > 1000000)
            {
                MessageBox.Show("not valid");
            }
        }

        /// <summary>
        /// This function makes sure the text box recives a valid mileage number - only digits
        /// </summary> 
        private void TextBoxMileage_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            if (e.Handled)
            {
                MessageBox.Show($"digits only\n'{e.Text}' is not a digit");
            }
        }

        /// <summary>
        /// This function makes sure the text box recives a valid starting date - not in future, and not before 2000
        /// </summary> 
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

        /// <summary>
        /// This function makes sure the text box recives a valid last maint date - not in future, and not before 2000
        /// </summary>
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

        /// <summary>
        /// This function activated when the user fills all details in boxes and want to add the bus to the system.
        /// </summary>
        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            //get all values from text boxes and date pickers
            DateTime.TryParse(DatePickerStart.Text, out DateTime start);
            DateTime.TryParse(DatePickerLastMaintenance.Text, out DateTime last);
            int.TryParse(TextBoxBusLicenseNumber.Text, out int license);
            int.TryParse(TextBoxFuelAmount.Text, out int fuel);
            int.TryParse(TextBoxMileage.Text, out int mileage);

            if ((start > DateTime.Today) || (last > DateTime.Today)) //check validity of dates
            {
                MessageBox.Show("can't enter future date!");
                Close();
                return;
            }
            else //check validity of bus license num - according to year
            {
                if (start.Year > 2017) 
                {
                    if (license < 1000000)
                    {
                        MessageBox.Show("New buses (after 2017) use 8 digits id!");
                    }
                }
                else if (license > 99999999)
                {
                    MessageBox.Show("Old buses (before 2017) use 7 digits id!");
                }
            }

            if ((fuel < 0) || (fuel > 1200)) //check validity of fuel
            {
                MessageBox.Show("Fuel only between 0-1200!");
                Close();
                return;
            }
            if ((mileage < 0) || (mileage > 500000)) //check validity of mileage
            {
                MessageBox.Show("mileage only between 0-500,000!");
                Close();
                return;
            }

            if ((license > 99999999) || (license < 1000000)) //check validity of id - digit number
            {
                MessageBox.Show("Bus id only 7 or 8 digits");
                Close();
                return;
            }

            MainWindow.Eged.Add(new Bus(license, start, fuel, mileage, last)); //add to bus list in main window

            Close();
        }

    }
}
