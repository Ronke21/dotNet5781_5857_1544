using BLApi;
using BO;
using System.Windows;
using PR_PL.Manager_Stations;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationDetails.xaml
    /// </summary>
    public partial class StationDetails : Window
    {
        private readonly IBL bl;
        private readonly BusStation currentBS;
        public StationDetails(IBL b, BusStation bs)
        {
            InitializeComponent();

            currentBS = bs;

            bl = b;

            StationDetailsWindow.DataContext = currentBS;
        }

        private void Update_OnClick(object sender, RoutedEventArgs e)
        {
            UpdateStation us = new UpdateStation(bl, currentBS);
            us.ShowDialog();
            Close();
        }

        private void Map_OnClick(object sender, RoutedEventArgs e)
        {
            var smw = new ShowMapWindow(currentBS);
            smw.ShowDialog();
        }
    }
}
