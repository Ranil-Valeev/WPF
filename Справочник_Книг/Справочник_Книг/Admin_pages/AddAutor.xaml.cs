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
using Справочник_Книг.model;

namespace Справочник_Книг.Admin_pages
{
    /// <summary>
    /// Логика взаимодействия для AddAutor.xaml
    /// </summary>
    public partial class AddAutor : Page
    {
        private Справочник_книгEntities _context = Справочник_книгEntities.GetContext();
        private Авторы _currentAuthor;
        public AddAutor()
        {
            InitializeComponent();
            LoadAuthors();
        }
        private void LoadAuthors()
        {
            _context.Авторы.Load();
            AuthorComboBox.ItemsSource = _context.Авторы.Local;
        }

        private void AuthorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentAuthor = AuthorComboBox.SelectedItem as Авторы;

            if (_currentAuthor != null)
            {
                SurnameTextBox.Text = _currentAuthor.Фамилия;
                NameTextBox.Text = _currentAuthor.Имя;
                PatronymicTextBox.Text = _currentAuthor.Отчество;
                BirthDatePicker.SelectedDate = _currentAuthor.Дата_Рождения;
                CountryTextBox.Text = _currentAuthor.Страна;
                HistoryTextBox.Text = _currentAuthor.История;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SurnameTextBox.Text) || string.IsNullOrWhiteSpace(NameTextBox.Text))
                {
                    MessageBox.Show("Имя и фамилия обязательны для заполнения!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (_currentAuthor == null)
                {
                    _currentAuthor = new Авторы();
                    _context.Авторы.Add(_currentAuthor);
                }

                _currentAuthor.Фамилия = SurnameTextBox.Text.Trim();
                _currentAuthor.Имя = NameTextBox.Text.Trim();
                _currentAuthor.Отчество = PatronymicTextBox.Text.Trim();
                _currentAuthor.Страна = CountryTextBox.Text.Trim();
                _currentAuthor.История = HistoryTextBox.Text.Trim();
                _currentAuthor.Дата_Рождения = BirthDatePicker.SelectedDate;

                _context.SaveChanges();
                LoadAuthors();
                MessageBox.Show("Автор успешно сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorComboBox.SelectedIndex = -1;
            _currentAuthor = null;
            SurnameTextBox.Clear();
            NameTextBox.Clear();
            PatronymicTextBox.Clear();
            BirthDatePicker.SelectedDate = null;
            CountryTextBox.Clear();
            HistoryTextBox.Clear();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentAuthor == null)
            {
                MessageBox.Show("Выберите автора для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Удалить автора {_currentAuthor.Фамилия} {_currentAuthor.Имя}?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Авторы.Remove(_currentAuthor);
                    _context.SaveChanges();
                    LoadAuthors();
                    NewButton_Click(null, null);
                    MessageBox.Show("Автор успешно удалён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
