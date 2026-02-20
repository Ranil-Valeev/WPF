using Microsoft.Win32.SafeHandles;
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
using WpfApp2.Models;

namespace WpfApp2.Windows
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult x = MessageBox.Show("Вы действительно хотите закрыть", "Выйти", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (x == MessageBoxResult.Cancel)
                e.Cancel = true;

        }
        private void BtnCansel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            string Text = TbLogin.Text;
            string Password = TbPass.Password.Trim();

            if (CheckUser(Text, Password))
            {
                MessageBox.Show("Авторизация успешна!");
                // Открываем главное окно
            } else if (Checkadmin(Text, Password))
            {
                MessageBox.Show("Вы вошли от имени Сотрудника");
            }
            else if (Text == "admin" && Password == "admin")
            {
                MessageBox.Show("Вы вошли от имени Admin");
                Admin admin = new Admin();
                admin.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
        private void Gost_Click(object sender, RoutedEventArgs e)
        {
            Gost gost = new Gost();
            gost.Show();
            this.Close();

        }
        public static class CurrentUser
        {
            public static int Id { get; set; }
            public static string Login { get; set; }
            // Другие свойства пользователя
        }
        public bool CheckUser(string login, string password)
        {
            using (var db = new РКИСEntities())
            {         
                var user = db.Клиент.FirstOrDefault(к => к.Login == login && к.Password == password);
                    if (user != null)
                    {
                        CurrentUser.Id = user.ID_Клиента;
                        CurrentUser.Login = user.Login;

                    // Открываем главное окно
                    var mainWindow = new MainWindow();
                    mainWindow.SetUser(CurrentUser.Id);
                    mainWindow.Show();
                    this.Close();
                    return true;
                    }
                    else
                    {
                        return false;
                    }                  
            }
        }
        public bool Checkadmin(string login, string password)
        {
            using (var db = new РКИСEntities())
            {

                var user = db.Сотрудник.FirstOrDefault(к => к.Login_s == login && к.Password_s == password);

                if (user != null)
                {
                    CurrentUser.Id = user.ID_Сотрудника;
                    CurrentUser.Login = user.Login_s;

                    // Открываем главное окно
                    var sotrudnik = new Sotrudnik();
                    sotrudnik.SetSotrudnik(CurrentUser.Id);
                    sotrudnik.Show();
                    this.Close();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
