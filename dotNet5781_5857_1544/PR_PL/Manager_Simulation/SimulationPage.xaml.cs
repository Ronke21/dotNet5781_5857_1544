using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;

namespace PR_PL.Manager_Simulation
{
    /// <summary>
    /// Interaction logic for SimulationPage.xaml
    /// </summary>
    public partial class SimulationPage : Page
    {
        private bool running = false;
        private BackgroundWorker simulationWorker = null;
        public SimulationPage()
        {
            InitializeComponent();

            simulationWorker = new BackgroundWorker {WorkerSupportsCancellation = true};
        }
        

        private void StartStop_OnClick(object sender, RoutedEventArgs e)
        {
            if (running)
            {
                running = false;
                StartStopIcon.Kind = PackIconKind.Play;
                RateSlider.IsEnabled = true;
                Clock.IsEnabled = true;
                simulationWorker.CancelAsync();
            }
            else
            {
                running = true;
                StartStopIcon.Kind = PackIconKind.Stop;
                RateSlider.IsEnabled = false;
                Clock.IsEnabled = false;
                simulationWorker.RunWorkerAsync();
            }
        }
    }
}
