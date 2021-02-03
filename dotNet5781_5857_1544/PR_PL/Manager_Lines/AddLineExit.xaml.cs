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
namespace PR_PL.Manager_Lines
{
    /// <summary>
    /// Interaction logic for AddLineExit.xaml
    /// </summary>
    public partial class AddLineExit : Window
    {
        private IBL _bl;
        private BusLine bline;
        public AddLineExit(IBL b, BusLine bl)
        {
            InitializeComponent();

            _bl = b;
            bline = bl;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            DateTime s = (DateTime)StartTimePicker.SelectedTime;
            DateTime f = (DateTime)FreqPicker.SelectedTime;
            DateTime en = (DateTime)EndTimePicker.SelectedTime;


            LineExit toAdd = new LineExit()
            {
                StartTime = new TimeSpan(s.Hour, s.Minute, s.Second),
                Freq = new TimeSpan(f.Hour, f.Minute, f.Second),
                EndTime = new TimeSpan(en.Hour, en.Minute, en.Second),
                LineNumber = bline.LineNumber,
                BusLineId = bline.BusLineId,
                Active = true
            };
            try
            {
                _bl.AddLineExit(toAdd);
            }
            catch (BO.BadAdditionException ex)
            {
                MessageBox.Show(ex.Message, "can't add the line exit");
            }
            Close();
        }
    }

}
