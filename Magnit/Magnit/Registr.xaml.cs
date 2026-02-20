using Magnit.Model;
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

namespace Magnit
{
    /// <summary>
    /// Логика взаимодействия для Registr.xaml
    /// </summary>
    public partial class Registr : Window
    {
        private readonly MagnitEntities _db = new MagnitEntities();
        private MagnitEntities _context = new MagnitEntities();
        public Registr()
        {
            InitializeComponent();
        }

        // Обработчик кнопки "Сохранить"
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация данных
                if (string.IsNullOrWhiteSpace(LastName.Text) ||
                    string.IsNullOrWhiteSpace(FirstName.Text) ||
                    string.IsNullOrWhiteSpace(Phone.Text) ||
                    string.IsNullOrWhiteSpace(Email.Text) ||
                    Password.SecurePassword.Length == 0)
                {
                    MessageBox.Show("Пожалуйста, заполните все обязательные поля!", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var newUser = new Пользователь
                {
                    Фамилия = LastName.Text,
                    Имя = FirstName.Text,
                    Отчество = MiddleName.Text,
                    Номер_телефона = Phone.Text,
                    электронная_почта = Email.Text,
                    пол = ((ComboBoxItem)Gender.SelectedItem).Content.ToString()[0].ToString(), // "М" или "Ж"
                    Возраст = BirthDate.SelectedDate ?? DateTime.Now.AddYears(-18),
                    Пароль = Password.Password // В реальном проекте нужно хэшировать!
                };
                _context.Пользователь.Add(newUser);
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
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
