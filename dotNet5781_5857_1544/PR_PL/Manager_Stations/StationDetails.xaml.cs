using BLApi;
using BO;
using System.Windows;

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

            lbAccess.DataContext = currentBS.Accessible;
            lbCode.DataContext = currentBS.Code;
            lbName.DataContext = currentBS.Name;
        //    lbLocation.DataContext = currentBS.Location.ToString();
            lbAddress.DataContext = currentBS.Address;
    //        StationDetailsWindow.DataContext = currentBS;
        }

        private void Update_OnClick(object sender, RoutedEventArgs e)
        {
   //         UpdateStation us = new UpdateStation(bl, currentBS);
  //          us.ShowDialog();
            Close();
        }
    }
}
