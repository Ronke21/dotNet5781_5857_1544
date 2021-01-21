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

namespace PR_PL
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

            var x = bl.ListForDigitalSign(38831);

            ArrivalsDataGrid.ItemsSource = x;
        }
    }
}
