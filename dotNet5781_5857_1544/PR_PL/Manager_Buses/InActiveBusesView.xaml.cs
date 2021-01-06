//using System.Collections;
//using System.Linq;
//using System.Windows;
//using System.Windows.Input;
//using BLApi;
//using BO;
//using PR_PL;
//using PR_PL.Manager_Buses;

//namespace PL
//{
//    /// <summary>
//    /// Interaction logic for InActiveBusesView.xaml
//    /// </summary>
//    public partial class InActiveBusesView : Window
//    {
        
//        private BusesViewPage bv;
//        public InActiveBusesView(IBL b, BusesViewPage v)
//        {
//            InitializeComponent();

//            bl = b;
//            bv = v;

//            InActiveBusesDataGrid.DataContext = bl.GetAllInActiveBuses().ToList();
//        }
//        private void Exit_OnClick(object sender, RoutedEventArgs e)
//        {
//            Close();
//        }

//        private void BusesDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
//        {
//            Bus b = (Bus)InActiveBusesDataGrid.SelectedItem;
//            BusDetails bd = new BusDetails(bl, b);
//            bd.Show();
//        }

//        private void Activate_Click(object sender, RoutedEventArgs e)
//        {
//            if (InActiveBusesDataGrid.SelectedItem == null)
//            {
//                MessageBox.Show("Please choose at least one bus and then click activate!");
//            }
//            else
//            {
//                var lb = (IEnumerable)(InActiveBusesDataGrid.SelectedItems);

//                foreach (Bus b in lb)
//                {
//                    var updated = new Bus
//                    {
//                        LicenseNum = b.LicenseNum,
//                        Fuel = b.Fuel,
//                        Mileage = b.Mileage,
//                        StartTime = b.StartTime,
//                        LastMaint = b.LastMaint,
//                        MileageFromLast = b.MileageFromLast,
//                        Active = true
//                    };
//                    bl.UpdateBus(updated);
//                }

//                InActiveBusesDataGrid.DataContext = bl.GetAllInActiveBuses().ToList();
//                bv.BusesDataGrid.DataContext = bl.GetAllBuses().ToList();
//            }
//        }
//    }
//}
