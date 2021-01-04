using System;
using System.Collections;
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
using BLApi;
using BO;
using PL;

namespace PR_PL.Manager_Buses
{
    /// <summary>
    /// Interaction logic for BusesViewPage.xaml
    /// </summary>
    public partial class BusesViewPage : Page
    {
        private readonly IBL _bl;
        public BusesViewPage(IBL b)
        {
            _bl = b;

            InitializeComponent();

            BusesDataGrid.DataContext = _bl.GetAllBuses().ToList();
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            AddBus ab = new AddBus(_bl);
            ab.ShowDialog();
            BusesDataGrid.DataContext = _bl.GetAllBuses().ToList();
        }

        private void BusesDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Bus b = (Bus)BusesDataGrid.SelectedItem;
            BusDetails bd = new BusDetails(_bl, b);
            bd.Show();
        }

        private void InActive_Click(object sender, RoutedEventArgs e)
        {
            //InActiveBusesView iabv = new InActiveBusesView(bl, this);
            //iabv.ShowDialog();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (BusesDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please choose at least one bus and then click remove!");
            }
            else
            {
                var lb = (IEnumerable)(BusesDataGrid.SelectedItems);

                foreach (var b in lb)
                {
                    _bl.DeleteBus(((Bus)b).LicenseNum);
                }
                BusesDataGrid.DataContext = _bl.GetAllBuses().ToList(); //update view
            }
        }
    }
}
