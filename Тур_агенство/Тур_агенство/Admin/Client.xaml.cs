using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Тур_агенство.model;
using System.Data.Entity;

namespace Тур_агенство.Admin
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Page
    {
        private ТурАгентствоEntities _context = new ТурАгентствоEntities();
        private Клиенты _currentClient;

        public Client()
        {
            InitializeComponent();
            LoadClients();
        }

        private void LoadClients()
        {
            _context.Клиенты.Load();
            ClientComboBox.ItemsSource = _context.Клиенты.Local;
        }

        private void ClientComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentClient = ClientComboBox.SelectedItem as Клиенты;

            if (_currentClient != null)
            {
                FIOTextBox.Text = _currentClient.ФИО ?? "";
                EmailTextBox.Text = _currentClient.ЭлектроннаяПочта ?? "";
                PhoneTextBox.Text = _currentClient.Телефон ?? "";
            }
            else
            {
                ClearFields();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FIOTextBox.Text))
            {
                MessageBox.Show("Введите ФИО клиента!", "Ошибка");
                return;
            }

            if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                MessageBox.Show("Введите электронную почту!", "Ошибка");
                return;
            }

            // Проверка формата email
            if (!IsValidEmail(EmailTextBox.Text))
            {
                MessageBox.Show("Введите корректный email!", "Ошибка");
                return;
            }

            if (string.IsNullOrWhiteSpace(PhoneTextBox.Text))
            {
                MessageBox.Show("Введите телефон!", "Ошибка");
                return;
            }

            if (_currentClient == null)
            {
                _currentClient = new Клиенты();
                _context.Клиенты.Add(_currentClient);
            }

            _currentClient.ФИО = FIOTextBox.Text.Trim();
            _currentClient.ЭлектроннаяПочта = EmailTextBox.Text.Trim();
            _currentClient.Телефон = PhoneTextBox.Text.Trim();

            _context.SaveChanges();
            LoadClients();
            MessageBox.Show("Клиент сохранён!");
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            ClientComboBox.SelectedIndex = -1;
            _currentClient = null;
            ClearFields();
        }

        private void ClearFields()
        {
            FIOTextBox.Clear();
            EmailTextBox.Clear();
            PhoneTextBox.Clear();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentClient == null)
            {
                MessageBox.Show("Выберите клиента для удаления!");
                return;
            }

            // Проверка на наличие связанных бронирований
            if (_currentClient.Бронирования.Any())
            {
                MessageBox.Show("Невозможно удалить клиента, так как есть связанные бронирования!", "Ошибка");
                return;
            }

            if (MessageBox.Show($"Удалить клиента '{_currentClient.ФИО}'?", "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.Клиенты.Remove(_currentClient);
                _context.SaveChanges();
                LoadClients();
                NewButton_Click(null, null);
                MessageBox.Show("Клиент удалён!");
            }
        }
    }
}
