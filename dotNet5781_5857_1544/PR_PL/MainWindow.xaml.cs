using System.Linq;
using System.Windows;
using BLApi;
using System.Windows.Navigation;
using PR_PL;

/*
 * todo: change relevant windows to pages                                                           // 
 *
 * todo: how to make sure window is centered any time we open it                                    //
 *
 * todo: double click window showing all bus's details ( + buttons for threads)                     // Amihay   V
 *
 * todo: update bus, inside the double click window                                                 // Amihay   V
 *
 * todo: implement remove by pressing on the whole line                                             // Ron
 *
 * todo: implement active in all crud BO and DO                                                     // Amihay
 *
 * todo: add window showing all inactive buses, implement cRud function to get all inactive buses   // Ron      V - after implementing remove, copy to here
 */

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBL bl;
        //private BO.Bus currentBus;
        public MainWindow()
        {
            InitializeComponent();

            bl = BLFactory.GetBL("1");
        }

        private void ShowBusesList_OnClick(object sender, RoutedEventArgs e)
        {
            BusesView bv = new BusesView(bl);
            bv.Show();
        }
    }
}
