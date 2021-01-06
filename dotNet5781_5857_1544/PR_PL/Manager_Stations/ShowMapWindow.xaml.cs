using System;
using System.Windows;
using BO;

namespace PR_PL.Manager_Stations
{
    /// <summary>
    /// Interaction logic for ShowMapWindow.xaml
    /// </summary>
    public partial class ShowMapWindow : Window
    {
        public ShowMapWindow(BusStation bs)
        {
            InitializeComponent();

            var station = bs;

            var longitude = station.Location.Longitude;
            var latitude = station.Location.Latitude;

            var googleMapsAddress = $"https://www.google.co.il//maps/@{longitude},{latitude},18z?hl=iw";
            
            var bingMapsAddress = $"https://www.bing.com/maps?cp={longitude}~{latitude}&lvl=18";
            
            ShowMap.Source = new Uri(googleMapsAddress);

            //var k = "AtbpkGlznerExttC1tAEa7wPmubvzBDQa4Byq33BCkde0PKsuOV2PelJw_Zvnx1-";
            //ShowMap.Source =
            //    new Uri($@"http://dev.virtualearth.net/REST/v1/Locations/{longitude},{latitude}?includeEntityTypes=countryRegion&o=xml&key={k}");
        }
    }
}