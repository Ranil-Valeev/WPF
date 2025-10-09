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
    /// Логика взаимодействия для BooksPage.xaml
    /// </summary>
    public partial class BooksPage : Page
    {
        private Справочник_книгEntities _context = Справочник_книгEntities.GetContext();
        private Книги _currentBook;

        public BooksPage()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            _context.Книги.Load();
            _context.Авторы.Load();
            _context.Жанры.Load();

            BookComboBox.ItemsSource = _context.Книги.Local;
            AuthorComboBox.ItemsSource = _context.Авторы.Local;
            GenreComboBox.ItemsSource = _context.Жанры.Local;
        }

        private void BookComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentBook = BookComboBox.SelectedItem as Книги;

            if (_currentBook != null)
            {
                NameTextBox.Text = _currentBook.Название;
                YearTextBox.Text = _currentBook.Год_Издания?.ToString();
                DescTextBox.Text = _currentBook.Описание;
                AuthorComboBox.SelectedValue = _currentBook.ID_Автора;
                GenreComboBox.SelectedValue = _currentBook.ID_Жанра;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NameTextBox.Text))
                {
                    MessageBox.Show("Введите название книги");
                    return;
                }

                if (_currentBook == null)
                {
                    _currentBook = new Книги();
                    _context.Книги.Add(_currentBook);
                }

                _currentBook.Название = NameTextBox.Text.Trim();
                _currentBook.ID_Автора = (int)AuthorComboBox.SelectedValue;
                _currentBook.ID_Жанра = (int)GenreComboBox.SelectedValue;
                _currentBook.Описание = DescTextBox.Text.Trim();

                if (int.TryParse(YearTextBox.Text, out int year))
                    _currentBook.Год_Издания = year;
                else
                    _currentBook.Год_Издания = null;

                _context.SaveChanges();
                LoadData();
                MessageBox.Show("Книга сохранена успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            BookComboBox.SelectedIndex = -1;
            _currentBook = null;
            NameTextBox.Clear();
            YearTextBox.Clear();
            DescTextBox.Clear();
            AuthorComboBox.SelectedIndex = -1;
            GenreComboBox.SelectedIndex = -1;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentBook == null)
            {
                MessageBox.Show("Выберите книгу для удаления!");
                return;
            }

            if (MessageBox.Show($"Удалить книгу '{_currentBook.Название}'?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.Книги.Remove(_currentBook);
                _context.SaveChanges();
                LoadData();
                NewButton_Click(null, null);
                MessageBox.Show("Книга удалена!");
            }
        }
    }
}
