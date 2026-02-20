using Steam.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace Steam.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для Add_develop.xaml
    /// </summary>
    public partial class Add_develop : Page
    {
        private SteamEntities _context = new SteamEntities();
        private Разработчик _currentDeveloper;
        public Add_develop()
        {
            InitializeComponent();
            LoadDevelopers();
        }
        private void LoadDevelopers()
        {
            _context.Разработчик.Load();
            DevelopersComboBox.ItemsSource = _context.Разработчик.Local;
        }

        private void DevelopersComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _currentDeveloper = DevelopersComboBox.SelectedItem as Разработчик;

            if (_currentDeveloper != null)
            {
                NameTextBox.Text = _currentDeveloper.Название;
                CountryTextBox.Text = _currentDeveloper.Страна;
                YearTextBox.Text = _currentDeveloper.Год.ToString();
                SpecializationTextBox.Text = _currentDeveloper.специализация;
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
                    _currentDeveloper = new Разработчик();
                    _context.Разработчик.Add(_currentDeveloper);
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

                _currentDeveloper.специализация = SpecializationTextBox.Text;

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
            SpecializationTextBox.Text = "";
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentDeveloper == null)
            {
                MessageBox.Show("Выберите разработчика для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show($"Вы уверены, что хотите удалить разработчика {_currentDeveloper.Название}?",
                                       "Подтверждение удаления",
                                       MessageBoxButton.YesNo,
                                       MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Разработчик.Remove(_currentDeveloper);
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

        //protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        //{
        //    base.OnClosing(e);
        //    _context.Dispose();
        //}
    }
}
