using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using dotNet5781_02_5857_1544;

namespace dotNet5781_03A_5857_1544
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Random r = new Random(DateTime.Now.Millisecond);

        private BusLine currentDisplayBusLine;

        BusLineCollection Eged;
        private void AddBus(ref BusLineCollection Eged)
        {
            int line = r.Next(1, 300);
            Eged.AddBusLine(line);
            int num_of_stat = r.Next(5, 10);
            for (int j = 0; j < num_of_stat; j++)
            {
                Eged.AddStationToBusLine(line, j, new BusLineStation());
            }
        }

        public MainWindow()
        {

            InitializeComponent();

            Eged = new BusLineCollection(); //the bus lines in the system
            while (Eged.Count() < 10)
            {
                try
                {
                    AddBus(ref Eged);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            CbBusLines.ItemsSource = Eged;
            CbBusLines.DisplayMemberPath = " BUSLINEID ";
            CbBusLines.SelectedIndex = 0;
            ShowBusLine(dotNet5781_02_5857_1544.BusLineCollection.Eged[0].BUSLINEID);
        }
         
        private void cbBusLines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowBusLine((CbBusLines.SelectedValue as BusLine).BUSLINEID);
            ShowArea((CbBusLines.SelectedValue as BusLine).BUSLINEID);
        }

        private void ShowArea(int index)
        {
            currentDisplayBusLine = Eged[index];
            tbArea.Text = Convert.ToString(currentDisplayBusLine.AREA);
        }

        private void ShowBusLine(int index)
        {
            currentDisplayBusLine = Eged[index];
            UpGrid.DataContext = currentDisplayBusLine;
            LbBusLineStations.DataContext = currentDisplayBusLine.Stations;
        }
    }
}
