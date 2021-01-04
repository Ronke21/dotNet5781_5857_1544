using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using BLApi;
using System.Windows.Navigation;
using PR_PL;
using PR_PL.Manager_Buses;
using PR_PL.Manager_Lines;

/*
 * todo: change relevant windows to pages                                                           // 
 *
 * todo: how to make sure window is centered any time we open it                                    //
 *
 * todo: double click window showing all bus's details ( + buttons for threads)                     // Amihay   V
 *
 * todo: update bus, inside the double click window                                                 // Amihay   V
 *
 * todo: implement remove by pressing on the whole line                                             // Ron      V
 *
 * todo: add window showing all inactive buses, implement cRud function to get all inactive buses   // Ron      V 
 *
 * todo: logic of buses                                                                             // Amihay
 * 
 * todo: implement Stations Dal and BL                                                              // Ron      V
 *
 * todo: implement BulLines Dal and BL                                                              // Both
 *
 * todo: stations windows                                                                           // Ron      V
 *
 * todo: learn how to work with pages and resources                                                 // Amihay
 * 
 * todo: logic of stations
 * 
 * todo: decide what can be updated!
 */

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBL bl;

        private bool _hidden = false;
        //private BO.Bus currentBus;
        public MainWindow()
        {
            InitializeComponent();

            bl = BLFactory.GetBL("1");
        }

        //private void ShowBusesList_OnClick(object sender, RoutedEventArgs e)
        //{
        //    BusesView bv = new BusesView(bl);
        //    bv.Show();
        //}

        private void BusLines_OnClick(object sender, RoutedEventArgs e)
        {
            LinesView lv = new LinesView(bl);
            lv.Show();
        }

        private void ShowStationsList_Click(object sender, RoutedEventArgs e)
        {
            StationsView sv = new StationsView(bl);
            sv.Show();
        }

        #region mouse effects and functionality for exit button
        private void Exit_OnMouseEnter(object sender, MouseEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            Exit.Background = (Brush)bc.ConvertFrom("#F1707A");
        }
        private void Exit_OnMouseLeave(object sender, MouseEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            Exit.Background = (Brush)bc.ConvertFrom("#E81123");
        }
        private void Exit_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        #endregion

        #region open close menu

        private void OpenCloseMenu_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_hidden)
            {
                Storyboard sb = Resources["OpenMenu"] as Storyboard;
                sb?.Begin(SideBar);
                _hidden = false;
                OpenCloseMenu.Source = new BitmapImage(new Uri("Icons/menu_close.png", UriKind.Relative));
            }
            else
            {
                Storyboard sb = Resources["CloseMenu"] as Storyboard;
                sb?.Begin(SideBar);
                _hidden = true;
                OpenCloseMenu.Source = new BitmapImage(new Uri("Icons/menu_open.png", UriKind.Relative));
            }
        }

        #endregion

        #region Buses button

        #region colors
        private void BusesSidePanel_OnMouseEnter(object sender, MouseEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            BusesSidePanel.Background = (Brush)bc.ConvertFrom("#30ABFF");
        }

        private void BusesSidePanel_OnMouseLeave(object sender, MouseEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            BusesSidePanel.Background = (Brush)bc.ConvertFrom("#FF0064A6");
        }

        #endregion

        private void BusesSidePanel_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!_hidden)
            {
                OpenCloseMenu_OnMouseDown(sender, e);
            }

            //show buses list
            DataDisplay.Content = new BusesViewPage(bl);
        }
        #endregion

    }
}
