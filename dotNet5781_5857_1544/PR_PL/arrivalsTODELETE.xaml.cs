using System.Windows;
using BLApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for arrivalsTODELETE.xaml
    /// </summary>
    public partial class arrivalsTODELETE : Window
    {
        private IBL bl;
        public arrivalsTODELETE(IBL b)
        {
            InitializeComponent();

            bl = b;

            //var x = bl.GetListForDigitalSign(38831);

            //ArrivalsDataGrid.ItemsSource = x;
        }
    }
}
