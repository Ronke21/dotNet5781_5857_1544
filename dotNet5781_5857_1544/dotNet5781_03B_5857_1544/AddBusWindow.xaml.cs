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

        public static DateTime getStartingDate()
        {
            DateTime start;
            do
            {
                Console.WriteLine("Please enter the bus starting date (dd/mm/yyyy): ");
                DateTime.TryParse(Console.ReadLine(), out start);
            }
            while (start.Year < 1948); //recieve a valid starting date since the establishment of Israel
            return start;
        }
        public static DateTime getLastMaint()
        {
            DateTime lastMaint;
            do
            {
                Console.WriteLine("Please enter the last maintenance date (dd/mm/yyyy): ");
                DateTime.TryParse(Console.ReadLine(), out lastMaint);
            } while (lastMaint.Year < 1948);
            return lastMaint;
        }
        public static int get8DigitsLineID(List<Bus> DB)
        {
            int id;
            bool exist = false;
            do
            {
                exist = false;
                Console.WriteLine("Please enter the bus license number (8 digits): ");
                Int32.TryParse(Console.ReadLine(), out id);
                foreach (var bus in DB)
                {
                    if (id == bus.LICENSENUM)
                    {
                        exist = true;
                        break;
                    }
                }
            } while ((id < 10000000) || (id > 99999999) || exist); //valid id 8 digits
            return id;
        }

        public static int get7DigitsLineID(List<Bus> DB)
        {
            int id;
            bool exist = false;
            do
            {
                Console.WriteLine("Please enter the bus license number (7 digits): ");
                Int32.TryParse(Console.ReadLine(), out id);
                foreach (var bus in DB)
                {
                    if (id == bus.LICENSENUM)
                    {
                        exist = true;
                        break;
                    }
                }
            }
            while ((id < 1000000) || (id > 9999999) || exist); //valid id 7 digits
            return id;
        }

        public static int getFuelAmount()
        {
            int fuel;
            do
            {
                Console.WriteLine("Please enter the bus fuel amount (how many KM left in tank): ");
                Int32.TryParse(Console.ReadLine(), out fuel);
            } while (fuel < 0 || fuel > 1200); //valid fuel amount according to the tank size
            return fuel;
        }

        public static int getMileage()
        {
            int km;
            do
            {
                Console.WriteLine("Please enter the bus milage amount (how many KM he drived): ");
                Int32.TryParse(Console.ReadLine(), out km);
            } while (km < 0); //valid km is positive
            return km;
        }

        public AddBusWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void add_bus_button(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }
    }
}
