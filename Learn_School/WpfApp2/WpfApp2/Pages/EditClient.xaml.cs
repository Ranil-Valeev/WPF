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
using WpfApp2.Models;

namespace WpfApp2.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditClient.xaml
    /// </summary>
    public partial class EditClient : Page
    {
        private Клиент _selectedClient;

        public EditClient()
        {
            InitializeComponent();
            ClientSelector.ItemsSource = РКИСEntities.GetContext().Клиент.ToList();
        }

        private void ClientSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedClient = ClientSelector.SelectedItem as Клиент;

            if (_selectedClient != null)
            {
                LastNameBox.Text = _selectedClient.Фамилия;
                FirstNameBox.Text = _selectedClient.Имя;
                MiddleNameBox.Text = _selectedClient.Отчество;
                PhoneBox.Text = _selectedClient.Телефон;
                EmailBox.Text = _selectedClient.Email;
                BirthDatePicker.SelectedDate = _selectedClient.Дата_рождения;
                LoginBox.Text = _selectedClient.Login;
                PasswordBox.Password = _selectedClient.Password;

                if (_selectedClient.Пол_клиента == "Мужской")
                    GenderBox.SelectedIndex = 0;
                else if (_selectedClient.Пол_клиента == "Женский")
                    GenderBox.SelectedIndex = 1;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedClient == null)
            {
                MessageBox.Show("Сначала выберите клиента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _selectedClient.Фамилия = LastNameBox.Text;
                _selectedClient.Имя = FirstNameBox.Text;
                _selectedClient.Отчество = MiddleNameBox.Text;
                _selectedClient.Телефон = PhoneBox.Text;
                _selectedClient.Email = EmailBox.Text;
                _selectedClient.Дата_рождения = BirthDatePicker.SelectedDate ?? DateTime.Now;
                _selectedClient.Пол_клиента = ((ComboBoxItem)GenderBox.SelectedItem)?.Content.ToString();
                _selectedClient.Login = LoginBox.Text;
                _selectedClient.Password = PasswordBox.Password;

                РКИСEntities.GetContext().SaveChanges();

                MessageBox.Show("Изменения успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
