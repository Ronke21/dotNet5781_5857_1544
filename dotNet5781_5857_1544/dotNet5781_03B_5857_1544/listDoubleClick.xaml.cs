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
    /// Interaction logic for listDoubleClick.xaml
    /// </summary>
    public partial class listDoubleClick : Window
    {
        private Bus currentBus;
        public listDoubleClick(Bus b)
        {
            InitializeComponent();
            currentBus = b;

            lbFuel.DataContext = currentBus.FUELTSTR;
            lbID.DataContext= currentBus.LICENSENUMSTR;
            lbKM.DataContext = currentBus.MILEAGESTR;
            lbLast.DataContext = currentBus.LASTMAINTDATESTR;
            lblState.DataContext = currentBus.BUSSTATESTR;
            lbfromLast.DataContext = currentBus.MILAGESINCELASTMAINTSTR;
        }

        private void Maintain_Click(object sender, RoutedEventArgs e)
        {
            currentBus.lastMaintDate = DateTime.Now;
            currentBus.Fuel = 1200;
            currentBus.setStatus();
            lbFuel.DataContext = currentBus.FUELTSTR;
            lbLast.DataContext = currentBus.LASTMAINTDATESTR;
            lblState.DataContext = currentBus.BUSSTATESTR;
            MessageBox.Show("Last Maintenance updated for today!\nfuel tank is full!");
            Close();
        }

        private void Refuel_Click(object sender, RoutedEventArgs e)
        {
            currentBus.Fuel = 1200;
            currentBus.setStatus();
            lbFuel.DataContext = currentBus.FUELTSTR;
            lblState.DataContext = currentBus.BUSSTATESTR;
            MessageBox.Show("Fuel tank is full!");

            Close();
        }
    }
}
