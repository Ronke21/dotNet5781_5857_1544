using System;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using BLApi;
using MaterialDesignThemes.Wpf;
using PR_PL;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region init decleration
        private readonly IBL _bl;

        private bool _hidden;

        //private readonly BusesViewPage _busesViewPage;
        //private readonly StationsViewPage _stationsViewPage;
        //private readonly LinesViewPage _linesViewPage;
        //private readonly ConStatViewPage _conStatViewPage;
        private readonly SimulationPage _simulationPage;
        #endregion
        public MainWindow()
        {
            var lp = new LoginPage();
            lp.ShowDialog();
            PlaySound(@"..\PR_PL\Icons\hero_simple-celebration-03.wav");

            InitializeComponent();

            #region init
            _bl = BLFactory.GetBL("1");

            //_busesViewPage = new BusesViewPage(_bl);
            //_stationsViewPage = new StationsViewPage(_bl);
            //_linesViewPage = new LinesViewPage(_bl);
            //_conStatViewPage = new ConStatViewPage(_bl);
            _simulationPage = new SimulationPage(_bl);
            #endregion

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
        private void Exit_OnMouseDown(object sender, MouseEventArgs e)
        {
            Close();
        }

        private static void PlaySound(string path)
        {
            var sp = new SoundPlayer(path);
            sp.Load();
            sp.Play();
        }

        #endregion

        #region open close menu and drag window

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (_hidden)
            {
                PlaySound(@"..\PR_PL\Icons\navigation_forward-selection-minimal.wav");
                var sb = Resources["OpenMenu"] as Storyboard;
                sb?.Begin(SideBar);
                _hidden = false;
                OpenCloseButtonIcon.Kind = PackIconKind.MenuOpen;
            }
            else
            {
                PlaySound(@"..\PR_PL\Icons\navigation_backward-selection-minimal.wav");
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
        private void BusesSidePanel_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_hidden)
            {
                ButtonBase_OnClick(sender, e);
            }

            if (!(DataDisplay.Content is BusesViewPage))
            {
                DataDisplay.Content = new BusesViewPage(_bl);
            }
        }
        #endregion

        #region Stations button
        private void StationsSidePanel_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_hidden)
            {
                ButtonBase_OnClick(sender, e);
            }

            if (!(DataDisplay.Content is StationsViewPage))
            {
                DataDisplay.Content = new StationsViewPage(_bl, _simulationPage);
            }
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

            if (!(DataDisplay.Content is LinesViewPage))
            {
                DataDisplay.Content = new LinesViewPage(_bl);
            }
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

        #region  Consecutive stations
        private void ConsecutiveStationsSidePanel_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_hidden)
            {
                ButtonBase_OnClick(sender, e);
            }

            if (!(DataDisplay.Content is ConStatViewPage))
            {
                DataDisplay.Content = new ConStatViewPage(_bl);
            }

        }
        private void ConsecutiveStationsSidePanel_OnMouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            ConsecutiveStationsSidePanel.Background = (Brush)bc.ConvertFrom("#30ABFF");
        }
        private void ConsecutiveStationsSidePanel_OnMouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            ConsecutiveStationsSidePanel.Background = (Brush)bc.ConvertFrom("#FF0064A6");
        }
        #endregion

        #region Simulator
        private void SimulatorSidePanel_OnMouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            SimulatorSidePanel.Background = (Brush)bc.ConvertFrom("#30ABFF");
        }
        private void SimulatorSidePanel_OnMouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            SimulatorSidePanel.Background = (Brush)bc.ConvertFrom("#FF0064A6");
        }
        private void SimulatorSidePanel_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_hidden)
            {
                ButtonBase_OnClick(sender, e);
            }

            if (!(DataDisplay.Content is SimulationPage))
            {
                DataDisplay.Content = _simulationPage;
            }
        }

        #endregion

        private void InfoButton_OnClick(object sender, RoutedEventArgs e)
        {
            var info = new InfoWindow();
            info.ShowDialog();
        }
    }
}
