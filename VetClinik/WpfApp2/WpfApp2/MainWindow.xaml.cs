using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using WpfApp2.Models;
using WpfApp2.Windows;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool PhonePlaceholder = true;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            this.Close();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            string phone = Login.Text.Trim();
            string password = Pass.Password.Trim();

            //if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(password))
            //{
            //    MessageBox.Show("Введите номер телефона и пароль.");
            //    return;
            //}

            if (phone == "Admin" && password == "Admin")
            {
                Admin admin = new Admin();
                admin.Show();
                this.Close();
                return;
            }
            using (var db = new VetClinikumEntities())
            {
                var user = db.Users.FirstOrDefault(u => u.Номер_телефона == phone && u.Пароль == password);
                if (user == null)
                {
                    MessageBox.Show("Неверный логин или пароль.");
                    return;
                }

                // Проверка: сотрудник или клиент
                if (user.ID_роли == 2)
                {
                    CurrentUser.Id = user.ID_user;
                    Sotrudnik empWin = new Sotrudnik();
                    empWin.SetUser(CurrentUser.Id);
                    empWin.Show();
                }   
                else if (user.ID_роли == 1)
                {
                    CurrentUser.Id = user.ID_user;
                    Client client = new Client();
                    client.SetUser(CurrentUser.Id);
                    client.Show();
                    this.Close();
                    return;
                }
                this.Close();
            }
        }
        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
        private void Minimize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Login_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PhonePlaceholder)
            {
                Login.Text = "7";
                Login.Foreground = Brushes.White;
                PhonePlaceholder = false;
            }
        }
        private void Login_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Login.Text) || Login.Text == "7")
            {
                Login.Text = "Номер телефона";
                Login.Foreground = Brushes.Gray;
                PhonePlaceholder = true;
            }
        }
        public static class CurrentUser
        {
            public static int Id { get; set; }
           

        }
    }
}
