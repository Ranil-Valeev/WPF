using Fitness.Model;
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

namespace Fitness
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Журнал _currentClient = new Журнал();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            string phone = Login.Text.Trim();
            
            if (phone == "Admin")
            {
                Admin admin = new Admin();
                admin.Show();
                this.Close();
                return;
            }
            using (var db = new Фитнес_ЗалEntities())
            {
                var user = db.Клиенты.FirstOrDefault(u => u.Логин == phone);
                if (user == null)
                {
                    MessageBox.Show("Неверный логин");
                    return;
                }
                else
                {
                    CurrentUser.Id = user.Id_клиента;
                    Client client = new Client();
                    client.SetUser(CurrentUser.Id);
                    client.Show();


                    _currentClient.Id_клиента = CurrentUser.Id;
                    _currentClient.Действие = "Вход в систему";
                    _currentClient.Дата_и_время = DateTime.Now;

                    Фитнес_ЗалEntities.GetContext().Журнал.Add(_currentClient);
                    Фитнес_ЗалEntities.GetContext().SaveChanges();
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
