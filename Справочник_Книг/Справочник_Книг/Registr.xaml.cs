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
using System.Windows.Shapes;
using Справочник_Книг.model;

namespace Справочник_Книг
{
    /// <summary>
    /// Логика взаимодействия для Registr.xaml
    /// </summary>
    public partial class Registr : Window
    {
        private Справочник_книгEntities _context = new Справочник_книгEntities();
        public Registr()
        {
            InitializeComponent();
        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация данных
                if (string.IsNullOrWhiteSpace(Login.Text) ||
                    string.IsNullOrWhiteSpace(Nomer.Text) ||
                    string.IsNullOrWhiteSpace(Email.Text) ||
                    string.IsNullOrWhiteSpace(Pass.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все обязательные поля!", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var newUser = new Пользователи
                {
                    Логин = Login.Text,
                    Телефон = Nomer.Text,
                    Электронная_Почта = Email.Text,
                    Пароль = Pass.Text
                };
                _context.Пользователи.Add(newUser);
                _context.SaveChanges();
                MessageBox.Show("Регистрация прошла успешно!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindow mainWindow = new MainWindow();
                this.Close();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
