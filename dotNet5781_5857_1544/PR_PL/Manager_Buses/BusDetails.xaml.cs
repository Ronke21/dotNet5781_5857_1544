using System.Windows;
using System.Windows.Input;
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
            Close();
            ub.ShowDialog();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
