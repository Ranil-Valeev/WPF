using Fitness.Model;
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
using static Fitness.Client;

namespace Fitness
{
    /// <summary>
    /// Логика взаимодействия для Registr.xaml
    /// </summary>
    public partial class Registr : Window
    {
        private Клиенты _currentClient = new Клиенты();
        private bool PhonePlaceholder = true;
        private bool PhonePlaceholder2 = true;
        public Registr()
        {
            InitializeComponent();
            
        }

        private StringBuilder ValidateFields()
        {
            StringBuilder s = new StringBuilder();
            if (Login.Text == null)
                s.AppendLine("Ошибка 1");
            if (Email.Text == null)
                s.AppendLine("Ошибка 2");
            return s;
        }
        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void Minimize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
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
                // Заполняем данные User
                _currentClient.Логин = Login.Text;
                _currentClient.Email = Email.Text;
                _currentClient.Дата_регистрации = DateTime.Now;
                _currentClient.Последний_вход = DateTime.Now;
                // Добавляем сотрудника в БД
                Фитнес_ЗалEntities.GetContext().Клиенты.Add(_currentClient);
                Фитнес_ЗалEntities.GetContext().SaveChanges();

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
    }
}
