using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace Тур_агенство.User_Pages
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private int _userId;
        public UserWindow(int id)
        {
            InitializeComponent();
            _userId = id;
            MainFrame.Navigate(new ТурыPage(_userId));
        }
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ToursBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ТурыPage(_userId));
        }

        private void BookingsBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new БронированияPage(_userId));
        }

        private void RecommendationsBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new РекомендацииPage(_userId));
        }

        private void ProfileBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ПрофильPage(_userId));
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
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
