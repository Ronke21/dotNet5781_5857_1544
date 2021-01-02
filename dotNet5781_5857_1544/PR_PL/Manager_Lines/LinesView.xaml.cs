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

namespace PR_PL.Manager_Lines
{
    /// <summary>
    /// Interaction logic for LinesView.xaml
    /// </summary>
    public partial class LinesView : Window
    {
        private readonly IBL bl;
        public LinesView(IBL b)
        {
            InitializeComponent();
            
            bl = b;

            LinesDataGrid.DataContext = bl.GetAllActiveBusLines();
        }
    }
}
