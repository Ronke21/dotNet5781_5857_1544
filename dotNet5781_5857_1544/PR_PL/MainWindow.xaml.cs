using System.Linq;
using System.Windows;
using BLApi;

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

            BusesDataGrid.DataContext = bl.GetAllBuses().ToList();

        }
    }
}
