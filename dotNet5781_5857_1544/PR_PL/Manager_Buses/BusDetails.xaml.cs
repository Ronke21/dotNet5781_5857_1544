using System.Windows;
using BLApi;
using BO;

namespace PL
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
            Close();
        }
    }
}
