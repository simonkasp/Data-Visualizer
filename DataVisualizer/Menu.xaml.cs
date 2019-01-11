using System.Windows;

namespace DataVisualizer
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void RealTimeBtn(object sender, RoutedEventArgs e)
        {
            RealTime mw = new RealTime();
            mw.Show();
            Close();
        }

        private void NonRealTimeBtn(object sender, RoutedEventArgs e)
        {
            NonRealTime nrt = new NonRealTime();
            nrt.Show();
            Close();
        }

        private void ExitBtn(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

            