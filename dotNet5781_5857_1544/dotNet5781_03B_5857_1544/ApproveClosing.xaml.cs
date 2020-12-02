using System.Windows;
using System.Windows.Input;

namespace dotNet5781_03B_5857_1544
{
    /// <summary>
    /// Interaction logic for ApproveClosing.xaml
    /// </summary>
    public partial class ApproveClosing : Window
    {
        MainWindow wnd = (MainWindow)Application.Current.MainWindow;
        public ApproveClosing()
        {
            InitializeComponent();
        }

        private void CheckIfYES(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string ans = (Answer.Text).ToString();

                if (ans == "Y" || ans == "y")
                {
                    Close();
                    wnd.Close();
                }
            }
        }
    }
}
