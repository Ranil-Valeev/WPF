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
using static WpfApp2.MainWindow;

namespace WpfApp2.Pages
{
    /// <summary>
    /// Логика взаимодействия для Add_Profile.xaml
    /// </summary>
    public partial class Add_Profile : Page
    {
        private Клиенты _currentClient = new Клиенты();
        public Add_Profile()
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
            if (Phone == null)
                s.AppendLine("Ошибка4");
            if (BirthDate == null)
                s.AppendLine("Ошибка6");

            return s;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
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
                _currentClient.Фамилия = LastName.Text;
                _currentClient.Имя = FirstName.Text;
                _currentClient.Отчество = MiddleName.Text;
                _currentClient.Номер_телефона = Phone.Text;
                _currentClient.Электронная_почта = Email.Text;
                _currentClient.Дата_рождения = BirthDate.SelectedDate.Value;
                _currentClient.Город = City.Text;
                _currentClient.Улица = Street.Text;
                _currentClient.Дом = House.Text;
                _currentClient.Квартира = Apartment.Text;
                _currentClient.ID_user = CurrentUser.Id;

                // Добавляем сотрудника в БД
                VetClinikumEntities.GetContext().Клиенты.Add(_currentClient);
                VetClinikumEntities.GetContext().SaveChanges();

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
