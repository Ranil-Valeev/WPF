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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2.Models;
using WpfApp2.Windows;
using static WpfApp2.Windows.Login;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int _currentUserId;
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.NavigationService.Navigate(new Pages.ClientService());
        }
        public void SetUser(int userId)
        {
            _currentUserId = userId;
            // Получаем имя пользователя из базы данных
            using (var db = new РКИСEntities())
            {
                var user = db.Клиент.FirstOrDefault(к => к.ID_Клиента == userId);
                if (user != null)
                {
                    // Если имя есть - используем его, иначе логин
                    string displayName = !string.IsNullOrEmpty(user.Имя)
                        ? user.Имя
                        : user.Login;
                    lblWelcome.Content = $"Добро пожаловать, {displayName}!";
                }
            }
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            Close();
            login.Show();          
        }
        private void Open_Z_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.Records());
        }
        private void profile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.Pofile());
        }
        private void MyRecords_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.Records1());
        }
        private void Servise_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.ClientService());
        }
    }
}       

