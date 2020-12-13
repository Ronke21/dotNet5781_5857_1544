/*
 * Course mini project in .Net framework
 * Exercise number 3A
 * Lecturer - David kidron
 * Student - Amihay Hassan, Ron Keinan
 * 
 * this programs shows a managing system for bus lines and their stations */


using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using dotNet5781_02_5857_1544;

namespace dotNet5781_03A_5857_1544_v2_DNFW
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Random r = new Random(DateTime.Now.Millisecond);

        BusLineCollection Eged = new BusLineCollection(); //the bus lines in the system

        private BusLine currentDisplayBusLine; //the line number the user chose

        /// <summary>
        /// func to add a bus to system - used for initalization
        /// </summary>
        /// <param name="Eged">The collection of buses</param>
        private void AddBus(BusLineCollection Eged)
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

            while (Eged.Count() < 2) //start system with 10 buses with different numbers
            {
                try
                {
                    AddBus(Eged);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            CbBusLines.ItemsSource = Eged;
            CbBusLines.DisplayMemberPath = " BUSLINEID ";
            CbBusLines.SelectedIndex = 0;
            ShowBusLine(dotNet5781_02_5857_1544.BusLineCollection.Eged[0].BUSLINEID); //call func to print busline details
        }

        /// <summary>
        /// func to present bus details that is chosen in combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbBusLines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowBusLine(((BusLine) CbBusLines.SelectedValue).BUSLINEID);//print stations
            ShowArea(((BusLine) CbBusLines.SelectedValue).BUSLINEID);//print ares
        }

        /// <summary>
        /// prints the area in text box
        /// </summary>
        /// <param name="index">bus number</param>
        private void ShowArea(int index)
        {
            currentDisplayBusLine = Eged[index];
            tbArea.Text = Convert.ToString(currentDisplayBusLine.AREA); //print to text box
        }

        /// <summary>
        /// print stations of bus in listbox
        /// </summary>
        /// <param name="index">bus number</param>
        private void ShowBusLine(int index)
        {
            currentDisplayBusLine = Eged[index];
            UpGrid.DataContext = currentDisplayBusLine;
            LbBusLineStations.DataContext = currentDisplayBusLine.Stations;
        }
    }
}
