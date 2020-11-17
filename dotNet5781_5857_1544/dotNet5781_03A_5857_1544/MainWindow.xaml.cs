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
using System.Windows.Navigation;
using System.Windows.Shapes;
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
        private void AddBuses(ref BusLineCollection Eged)
        {
            for (int i = 0; i < 10; i++)
            {
                int line = r.Next(1, 300);
                Eged.AddBusLine(line);
                int num_of_stat = r.Next(3, 7);
                for (int j = 0; j < num_of_stat; j++)
                {
                    Eged.AddStationToBusLine(line, j, new BusLineStation());
                }
            }
        }
        public MainWindow()
        {

            InitializeComponent();

            Eged = new BusLineCollection(); //the bus lines in the system
            try
            {
                AddBuses(ref Eged);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            CbBusLines.ItemsSource = Eged;
            CbBusLines.DisplayMemberPath = " BUSLINEID ";
            CbBusLines.SelectedIndex = 0;
            ShowBusLine(dotNet5781_02_5857_1544.BusLineCollection.Eged[0].BUSLINEID);

        }

        private void cbBusLines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowBusLine((CbBusLines.SelectedValue as BusLine).BUSLINEID);
        }

        private void ShowBusLine(int index)
        {
            currentDisplayBusLine = Eged[index];
            UpGrid.DataContext = currentDisplayBusLine;
            LbBusLineStations.DataContext = currentDisplayBusLine.Stations;
        }

        //private void TbArea_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    TbArea.Text = Convert.ToString((CbBusLines.SelectedValue as BusLine).AREA);
        //}

        //private void TbArea_OnSourceUpdated(object? sender, DataTransferEventArgs e)
        //{
        //    TbArea.Text = Convert.ToString((CbBusLines.SelectedValue as BusLine).AREA);
        //}
    }
}
