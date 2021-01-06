using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using BLApi;
using System.Windows.Navigation;
using MaterialDesignThemes.Wpf;
using PR_PL;
using PR_PL.Manager_Buses;
using PR_PL.Manager_Lines;
using PR_PL.Manager_Stations;

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
        private readonly IBL bl;

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

        //private void BusLines_OnClick(object sender, RoutedEventArgs e)
        //{
        //    LinesView lv = new LinesView(bl);
        //    lv.Show();
        //}

        //private void ShowStationsList_Click(object sender, RoutedEventArgs e)
        //{
        //    StationsView sv = new StationsView(bl);
        //    sv.Show();
        //}

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

        #region open close menu and drag window

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (_hidden)
            {
                var sb = Resources["OpenMenu"] as Storyboard;
                sb?.Begin(SideBar);
                _hidden = false;
                OpenCloseButtonIcon.Kind = PackIconKind.MenuOpen;
            }
            else
            {
                var sb = Resources["CloseMenu"] as Storyboard;
                sb?.Begin(SideBar);
                _hidden = true;
                OpenCloseButtonIcon.Kind = PackIconKind.Menu;
            }
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        #endregion

        #region Buses button

        #region colors
        private void BusesSidePanel_OnMouseEnter(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            BusesSidePanel.Background = (Brush)bc.ConvertFrom("#30ABFF");
        }

        private void BusesSidePanel_OnMouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            BusesSidePanel.Background = (Brush)bc.ConvertFrom("#FF0064A6");
        }

        #endregion
        private void BusesSidePanel_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_hidden)
            {
                ButtonBase_OnClick(sender, e);
            }

            //show buses list
            if (DataDisplay.Content != null)
            {
                if (DataDisplay.Content is BusesViewPage) { }
                else DataDisplay.Content = new BusesViewPage(bl);
            }
            else DataDisplay.Content = new BusesViewPage(bl);
        }
        #endregion

        #region Stations button
        private void StationsSidePanel_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_hidden)
            {
                ButtonBase_OnClick(sender, e);
            }

            if (DataDisplay.Content != null)
            {
                if (DataDisplay.Content is StationsViewPage) { }
                else DataDisplay.Content = new StationsViewPage(bl);
            }

            else DataDisplay.Content = new StationsViewPage(bl);
        }

        #region colors
        private void StationsSidePanel_OnMouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            StationsSidePanel.Background = (Brush)bc.ConvertFrom("#30ABFF");
        }

        private void StationsSidePanel_OnMouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            StationsSidePanel.Background = (Brush)bc.ConvertFrom("#FF0064A6");
        }

        #endregion
        #endregion

        #region Lines button
        private void LinesSidePanel_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_hidden)
            {
                ButtonBase_OnClick(sender, e);
            }

            if (DataDisplay.Content != null)
            {
                if (DataDisplay.Content is LinesViewPage) { }
                else DataDisplay.Content = new LinesViewPage(bl);
            }

            else DataDisplay.Content = new LinesViewPage(bl);
        }

        private void LinesSidePanel_OnMouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            LinesSidePanel.Background = (Brush)bc.ConvertFrom("#FF0064A6");
        }

        private void LinesSidePanel_OnMouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            LinesSidePanel.Background = (Brush)bc.ConvertFrom("#30ABFF");
        }

        #endregion

    }
}
