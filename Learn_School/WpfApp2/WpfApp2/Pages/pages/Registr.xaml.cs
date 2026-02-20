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
using WpfApp2.Windows;
using static WpfApp2.Windows.Login;

namespace WpfApp2.Pages
{
    /// <summary>
    /// Логика взаимодействия для Registr.xaml
    /// </summary>
    public partial class Registr : Page
    {
        private Клиент _currentClient = new Клиент();
        public Registr()
        {
            InitializeComponent();
            BirthDate.SelectedDate = DateTime.Now.AddYears(-18); 
        }
        private StringBuilder ValidateFields()
        {
            StringBuilder s = new StringBuilder();
            if (LastName.Text == null)
                s.AppendLine("Ошибка1");
            if (FirstName.Text == null)
                s.AppendLine("Ошибка2");
            if (MiddleName == null)
                s.AppendLine("Ошибка3");
            if (Phone == null)
                s.AppendLine("Ошибка4");
            if (Email == null)
                s.AppendLine("Ошибка5");
            if (BirthDate == null)
                s.AppendLine("Ошибка6");
            if (Login == null)
                s.AppendLine("Ошибка7");
            if (Password == null)
                s.AppendLine("Ошибка8");
            if (ConfirmPassword == null)
                s.AppendLine("Ошибка9");
            return s;
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder s = ValidateFields();
            if (s.Length > 0)
            {
                Message.Text = s.ToString();
                return;
            }
            try
            {
                // Заполняем данные клиента
                _currentClient.Фамилия = LastName.Text;
                _currentClient.Имя = FirstName.Text;
                _currentClient.Отчество = MiddleName.Text;
                _currentClient.Телефон = Phone.Text;
                _currentClient.Email = Email.Text;
                _currentClient.Дата_рождения = BirthDate.SelectedDate.Value;
                _currentClient.Пол_клиента = Male.IsChecked == true ? "М" : "Ж";
                _currentClient.Дата_регистрации = DateTime.Now;
                _currentClient.Login = Login.Text;
                _currentClient.Password = Password.Password;
                // Добавляем клиента в БД
                РКИСEntities.GetContext().Клиент.Add(_currentClient);
                РКИСEntities.GetContext().SaveChanges();
                MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService?.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
