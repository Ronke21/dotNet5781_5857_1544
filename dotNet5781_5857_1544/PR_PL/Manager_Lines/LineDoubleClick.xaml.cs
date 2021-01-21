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
    /// Interaction logic for LineDoubleClick.xaml
    /// </summary>
    public partial class LineDoubleClick : Window
    {

        private readonly IBL _bl;
        private readonly BusLine bline;

        public LineDoubleClick(IBL b, BusLine busLine)
        {
            InitializeComponent();

            _bl = b;
            bline = busLine;

            DoubleClickFrame.Content = new LineDetailsPage(_bl, bline);
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Update_OnClick(object sender, RoutedEventArgs e)
        {
            DoubleClickFrame.Content = new UpdateLinePage(_bl, bline, this);
            Update.Visibility = Visibility.Collapsed;
            CancelUpdate.Visibility = Visibility.Visible;
        }

        private void CancelUpdate_OnClick(object sender, RoutedEventArgs e)
        {
            DoubleClickFrame.Content = new LineDetailsPage(_bl, bline);
            Update.Visibility = Visibility.Visible;
            CancelUpdate.Visibility = Visibility.Collapsed;
        }
    }
}
