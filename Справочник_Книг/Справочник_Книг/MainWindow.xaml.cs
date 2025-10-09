using Microsoft.Win32;
using System;
using System.CodeDom;
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
using Справочник_Книг.model;

namespace Справочник_Книг
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

        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            int ID;
            string login1 = Login.Text.Trim();
            string pass1 = Pass.Password.Trim();
            if (login1 == "admin" && pass1 == "admin") 
                {
                Admin_pages.Admin_window admin_Window = new Admin_pages.Admin_window();
                this.Close();
                admin_Window.Show();
                return;
                }
            using (var db = new Справочник_книгEntities())
            {
                var user = db.Пользователи.FirstOrDefault(u => u.Логин == login1 && u.Пароль == pass1);
                if (user == null)
                {
                    MessageBox.Show("Неверный логин или пароль.");
                    return;
                }
                else
                {
                    ID = user.ID_Пользователя;
                    User_Pages.User_Window user_Window = new User_Pages.User_Window();
                    user_Window.User(ID);
                    this.Close();
                    user_Window.Show();
                }
            }
        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            Registr registr = new Registr();
            this.Close();
            registr.Show();
        }
    }
}
