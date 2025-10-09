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
    /// Логика взаимодействия для AuthorDetails.xaml
    /// </summary>
    public partial class AuthorDetails : Page
    {
        private readonly Справочник_книгEntities _context = Справочник_книгEntities.GetContext();
        private readonly int _authorId;
        public AuthorDetails(int authorId)
        {
            InitializeComponent();
            _authorId = authorId;
            LoadAuthor();
        }
        private void LoadAuthor()
        {
            var author = _context.Авторы.FirstOrDefault(a => a.ID_Автора == _authorId);
            AuthorName.Text = $"{author.Фамилия} {author.Имя} {author.Отчество}";
            AuthorBio.Text = author.История ?? "Нет биографии";
            var books = _context.Книги.Where(k => k.ID_Автора == _authorId).ToList();

            BooksList.ItemsSource = books;
        }

        private void BooksList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (BooksList.SelectedItem is Книги book)
            {
                int bookId = book.ID_Книги;
                NavigationService?.Navigate(new BookDetails(bookId));
            }
        }
    }
}
