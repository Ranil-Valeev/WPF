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
using Справочник_Книг.model;

namespace Справочник_Книг.User_Pages
{
    /// <summary>
    /// Логика взаимодействия для AllBooks.xaml
    /// </summary>
    public partial class AllBooks : Page
    {
        private Справочник_книгEntities _context = Справочник_книгEntities.GetContext();
        private List<Книги> _allBooks;
        public AllBooks()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            // Подгружаем все данные
            _allBooks = _context.Книги.Include("Авторы").Include("Жанры").ToList();

            // Наполняем комбобоксы
            GenreComboBox.ItemsSource = _context.Жанры.ToList();
            AuthorComboBox.ItemsSource = _context.Авторы.ToList();

            UpdateBooksList();
        }

        private void UpdateBooksList()
        {
            var filteredBooks = _allBooks.AsQueryable();

            // Фильтр по жанру
            if (GenreComboBox.SelectedItem is Жанры selectedGenre)
                filteredBooks = filteredBooks.Where(b => b.ID_Жанра == selectedGenre.ID_Жанра);

            // Фильтр по автору
            if (AuthorComboBox.SelectedItem is Авторы selectedAuthor)
                filteredBooks = filteredBooks.Where(b => b.ID_Автора == selectedAuthor.ID_Автора);

            // Фильтр по названию
            string searchText = SearchTextBox.Text?.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchText))
                filteredBooks = filteredBooks.Where(b => b.Название.ToLower().Contains(searchText));

            // Отображаем данные
            BooksDataGrid.ItemsSource = filteredBooks
                .Select(b => new
                {
                    b.ID_Книги,
                    b.Название,
                    Автор = $"{b.Авторы.Фамилия} {b.Авторы.Имя}",
                    Жанр = b.Жанры.Название,
                    b.Год_Издания
                })
                .ToList();
        }

        private void FilterChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateBooksList();
        }

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = string.Empty;
            GenreComboBox.SelectedItem = null;
            AuthorComboBox.SelectedItem = null;
            UpdateBooksList();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateBooksList();
        }

        private void BooksDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (BooksDataGrid.SelectedValue == null)
                return;

            int bookId = (int)BooksDataGrid.SelectedValue;
            NavigationService?.Navigate(new BookDetails(bookId));
        }
    }
}
