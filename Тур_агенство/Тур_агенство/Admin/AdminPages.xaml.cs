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
using Тур_агенство.User_Pages;

namespace Тур_агенство.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdminPages.xaml
    /// </summary>
    public partial class AdminPages : Window
    {
        public AdminPages()
        {
            InitializeComponent();
        }
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void CityBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new City());
        }

        private void CountryBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Country());
        }
        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }

        private void ToursBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Tours());
        }

        private void ProfileBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Client());
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button btn)
            {
                btn.Background = new SolidColorBrush(Color.FromRgb(52, 73, 94));
            }
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Button btn)
            {
                btn.Background = Brushes.Transparent;
            }
        }
    }
}
