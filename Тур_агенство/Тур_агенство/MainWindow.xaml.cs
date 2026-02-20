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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Тур_агенство.model;

namespace Тур_агенство
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }

            if (login == "admin" && password == "admin")
            {
                Admin.AdminPages adminPages = new Admin.AdminPages();
                adminPages.Show();
                this.Close();
                return;
            }

            var user = GetContext().Клиенты.FirstOrDefault(u => u.ЭлектроннаяПочта == login && u.Телефон == password);

            if (user != null)
            {
                MessageBox.Show($"Добро пожаловать, {user.ФИО}!");
                User_Pages.UserWindow userWindow = new User_Pages.UserWindow(user.Id);
                userWindow.Show();
                this.Close();
               
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Регистрация registrationWindow = new Регистрация();
            registrationWindow.Show();
            this.Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private static ТурАгентствоEntities _context;
        public static ТурАгентствоEntities GetContext()
        {
            if (_context == null)
                _context = new ТурАгентствоEntities();
            return _context;
        }
    }
}
