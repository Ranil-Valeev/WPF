using Steam.Model;
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
using System.Data.Entity;

namespace Steam.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для Add_company.xaml
    /// </summary>
    public partial class Add_company : Page
    {
        private SteamEntities _context = new SteamEntities();
        private Издатель _currentDeveloper;
        public Add_company()
        {
            InitializeComponent();
            LoadDevelopers();
        }
        private void LoadDevelopers()
        {
            _context.Издатель.Load();
            DevelopersComboBox.ItemsSource = _context.Издатель.Local;
        }

        private void DevelopersComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _currentDeveloper = DevelopersComboBox.SelectedItem as Издатель;

            if (_currentDeveloper != null)
            {
                NameTextBox.Text = _currentDeveloper.Название;
                CountryTextBox.Text = _currentDeveloper.Страна;
                YearTextBox.Text = _currentDeveloper.Год.ToString();
                
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NameTextBox.Text))
                {
                    MessageBox.Show("Название разработчика обязательно для заполнения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (_currentDeveloper == null)
                {
                    // Добавление нового разработчика
                    _currentDeveloper = new Издатель();
                    _context.Издатель.Add(_currentDeveloper);
                }

                _currentDeveloper.Название = NameTextBox.Text;
                _currentDeveloper.Страна = CountryTextBox.Text;

                if (int.TryParse(YearTextBox.Text, out int year))
                {
                    _currentDeveloper.Год = year.ToString();
                }
                else
                {
                    MessageBox.Show("Год должен быть числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }



                _context.SaveChanges();
                LoadDevelopers();
                MessageBox.Show("Данные сохранены успешно", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            _currentDeveloper = null;
            DevelopersComboBox.SelectedIndex = -1;
            NameTextBox.Text = "";
            CountryTextBox.Text = "";
            YearTextBox.Text = "";
            
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentDeveloper == null)
            {
                MessageBox.Show("Выберите Издателя для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show($"Вы уверены, что хотите удалить Издателя {_currentDeveloper.Название}?",
                                       "Подтверждение удаления",
                                       MessageBoxButton.YesNo,
                                       MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Издатель.Remove(_currentDeveloper);
                    _context.SaveChanges();
                    LoadDevelopers();
                    NewButton_Click(null, null);
                    MessageBox.Show("Разработчик удален успешно", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
