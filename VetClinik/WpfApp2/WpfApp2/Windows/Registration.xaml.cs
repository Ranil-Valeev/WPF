using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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

namespace WpfApp2.Windows
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public int ID = 1; 
        private User _currentClient = new User();

        private bool PhonePlaceholder = true;
        public Registration()
        {
            InitializeComponent();
        }

        private void Minimize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private StringBuilder ValidateFields()
        {
            StringBuilder s = new StringBuilder();
            if (Login.Text == null)
                s.AppendLine("Ошибка 1");
            if (Pass.Password == null)
                s.AppendLine("Ошибка 2");
            if (Pass1.Password == null)
                s.AppendLine("Ошибка 3");
            return s;
        }
        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder s = ValidateFields();

            if (s.Length > 0)
            {
                MessageBox.Show(s.ToString());
                return;
            }

            try
            {
                // Заполняем данные сотрудника
                _currentClient.Номер_телефона = Login.Text;
                _currentClient.Пароль = Pass.Password;
                _currentClient.ID_роли = ID; 
                // Добавляем сотрудника в БД
                VetClinikumEntities.GetContext().Users.Add(_currentClient);
                VetClinikumEntities.GetContext().SaveChanges();

                MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
    }
}
