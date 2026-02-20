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
using static WpfApp2.Windows.Login;

namespace WpfApp2.Windows
{
    /// <summary>
    /// Логика взаимодействия для Sotrudnik.xaml
    /// </summary>
    public partial class Sotrudnik : Window
    {
        public Sotrudnik()
        {
            InitializeComponent();
            MainFrame.NavigationService.Navigate(new Pages.Record_sotrudnik());
        }

        public void SetSotrudnik(int userId)
        {
            CurrentUser.Id = userId;
            //lblWelcome.Content = $"Добро пожаловать, пользователь #{userId}";
            // Получаем имя пользователя из базы данных
            using (var db = new РКИСEntities())
            {
                var user = db.Сотрудник.FirstOrDefault(к => к. ID_Сотрудника== userId);
                if (user != null)
                {
                    // Если имя есть - используем его, иначе логин
                    string displayName = !string.IsNullOrEmpty(user.Имя)
                        ? user.Имя
                        : user.Login_s;
                    lblWelcome.Content = $"Добро пожаловать, {displayName}!";
                }
            }
        }
        private void profile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.Profile_sotrudnik());
        }
        private void MyRecords_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.Record_sotrudnik());
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            Close();
            login.Show();
        }
    }
}
