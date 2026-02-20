using Magnit.Model;
using Microsoft.Win32;
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

namespace Magnit
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
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            string phone = Login.Text.Trim();
            string password = Pass.Password.Trim();
            if (phone == "Admin" && password == "Admin")
            {
                Admin admin = new Admin();
                admin.Show();
                this.Close();
                return;
            }
            using (var db = new MagnitEntities())
            {
                var user = db.Пользователь.FirstOrDefault(u => u.электронная_почта == phone && u.Пароль == password);
                if (user == null)
                {
                    MessageBox.Show("Неверный логин или пароль.");
                    return;
                }
                else
                {
                    CurrentUser.Id = user.ID_пользователя;
                    Client client = new Client();
                    client.SetUser(CurrentUser.Id);
                    client.Show();
                    this.Close();
                    return;
                }
            }
        }
        public static class CurrentUser
        {
            public static int Id { get; set; }


        }
        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            Registr registr = new Registr();
            this.Close();
            registr.Show();
        }

        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void Minimize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
