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
using Университет.model;

namespace Университет
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

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            string log = login.Text.Trim();
            string pas = Password.Text.Trim();
            using (var db = new УниверситетEntities())
            {
                var users = db.Users.FirstOrDefault(u => u.Login == log && u.Password == pas);
                if (users == null)
                {
                    MessageBox.Show("Неверный логин или пароль.");
                    return;
                }
                else
                {
                    Teachers teachers = new Teachers();
                    teachers.Show();
                    this.Close();
                }
            }
        }

        private void Cansel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
