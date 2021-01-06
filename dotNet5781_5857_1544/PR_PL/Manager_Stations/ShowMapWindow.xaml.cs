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

            var address = $"https://www.google.co.il//maps/@{longitude},{latitude},18z?hl=iw";

            ShowMap.Source = new Uri(address);
        }
    }
    }