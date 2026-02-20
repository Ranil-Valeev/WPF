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
    /// Логика взаимодействия для EditSotrudnik.xaml
    /// </summary>
    public partial class EditSotrudnik : Page
    {
        private Сотрудник selectedEmployee;

        public EditSotrudnik()
        {
            InitializeComponent();
            SotrudnikSelector.ItemsSource = РКИСEntities.GetContext().Сотрудник.ToList();
        }

        private void SotrudnikSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedEmployee = SotrudnikSelector.SelectedItem as Сотрудник;
            if (selectedEmployee != null)
            {
                LastNameBox.Text = selectedEmployee.Фамилия;
                FirstNameBox.Text = selectedEmployee.Имя;
                MiddleNameBox.Text = selectedEmployee.Отчество;
                PassportBox.Text = selectedEmployee.Серия_номер;
                GenderBox.Text = selectedEmployee.Пол;
                BirthDatePicker.SelectedDate = selectedEmployee.Дата_рождения;
                LoginBox.Text = selectedEmployee.Login_s;
                PasswordBox.Password = selectedEmployee.Password_s;
                CategoryBox.Text = selectedEmployee.ID_Категории.ToString();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (selectedEmployee == null)
            {
                MessageBox.Show("Выберите сотрудника для редактирования.");
                return;
            }

            try
            {
                selectedEmployee.Фамилия = LastNameBox.Text;
                selectedEmployee.Имя = FirstNameBox.Text;
                selectedEmployee.Отчество = MiddleNameBox.Text;
                selectedEmployee.Серия_номер = PassportBox.Text;
                selectedEmployee.Пол = GenderBox.Text;
                selectedEmployee.Дата_рождения = BirthDatePicker.SelectedDate ?? DateTime.Now;
                selectedEmployee.Login_s = LoginBox.Text;
                selectedEmployee.Password_s = PasswordBox.Password;
                selectedEmployee.ID_Категории = int.TryParse(CategoryBox.Text, out int idCat) ? idCat : 0;

                РКИСEntities.GetContext().SaveChanges();
                MessageBox.Show("Сотрудник успешно обновлён!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
