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
    /// Interaction logic for ChooseBusWindow.xaml
    /// </summary>
    public partial class ChooseBusWindow : Window
    {
        private Bus b;
        public ChooseBusWindow(Bus bus)
        {
            InitializeComponent();
            b = bus;
        }

        private void ChooseMileage_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                int.TryParse(ChooseMileage.Text, out int mileage);

                if (b.Fuel < mileage)
                {
                    MessageBox.Show("this bus is not qualified for a ride\ntake it to maintenance");
                    Close();
                }

                if (mileage > 1200)
                {
                    MessageBox.Show("ride cannot be over 1200KM");
                    Close();
                }

                if (mileage > b.Fuel)
                {
                    MessageBox.Show("not enough fuel\nplease refuel");
                    Close();
                }

                b.addToMileage(mileage);
                MessageBox.Show("the bus has made the ride successfully");
                Close();
            }
        }
    }
}
