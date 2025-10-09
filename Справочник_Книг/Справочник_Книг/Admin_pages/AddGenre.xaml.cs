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
    /// Логика взаимодействия для AddGenre.xaml
    /// </summary>
    public partial class AddGenre : Page
    {
        private Справочник_книгEntities _context = Справочник_книгEntities.GetContext();
        private Жанры _currentGenre;
        public AddGenre()
        {
            InitializeComponent();
            LoadGenres();
        }
        private void LoadGenres()
        {
            _context.Жанры.Load();
            GenreComboBox.ItemsSource = _context.Жанры.Local;
        }

        private void GenreComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentGenre = GenreComboBox.SelectedItem as Жанры;

            if (_currentGenre != null)
            {
                GenreNameTextBox.Text = _currentGenre.Название;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(GenreNameTextBox.Text))
                {
                    MessageBox.Show("Название жанра обязательно для заполнения!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (_currentGenre == null)
                {
                    _currentGenre = new Жанры();
                    _context.Жанры.Add(_currentGenre);
                }

                _currentGenre.Название = GenreNameTextBox.Text.Trim();

                _context.SaveChanges();
                LoadGenres();
                MessageBox.Show("Жанр успешно сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении жанра: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            GenreComboBox.SelectedIndex = -1;
            _currentGenre = null;
            GenreNameTextBox.Clear();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentGenre == null)
            {
                MessageBox.Show("Выберите жанр для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Удалить жанр '{_currentGenre.Название}'?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Жанры.Remove(_currentGenre);
                    _context.SaveChanges();
                    LoadGenres();
                    NewButton_Click(null, null);
                    MessageBox.Show("Жанр успешно удалён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении жанра: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
