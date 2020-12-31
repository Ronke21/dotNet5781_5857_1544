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
using BLApi;
using BO;

namespace PR_PL
{
    /// <summary>
    /// Interaction logic for BusDetails.xaml
    /// </summary>
    public partial class BusDetails : Window
    {
        private readonly IBL bl;
        private readonly Bus bus;
        public BusDetails(IBL b, Bus bu)
        {
            InitializeComponent();

            bus = bu;
            bl = b;

            BusDetailsWindow.DataContext = bus;
        }

        private void Update_OnClick(object sender, RoutedEventArgs e)
        {
            UpdateBus ub = new UpdateBus(bl, bus);
            ub.ShowDialog();
        }
    }
}
