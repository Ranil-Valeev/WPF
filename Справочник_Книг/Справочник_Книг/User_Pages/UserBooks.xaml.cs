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
using static Справочник_Книг.User_Pages.User_Window;
using System.Data.Entity;


namespace Справочник_Книг.User_Pages
{
    /// <summary>
    /// Логика взаимодействия для UserBooks.xaml
    /// </summary>
    public partial class UserBooks : Page
    {
        private Справочник_книгEntities _context;
        public UserBooks()
        {
            InitializeComponent();
            _context = Справочник_книгEntities.GetContext();
            LoadBooks();
        }
        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && int.TryParse(btn.Tag?.ToString(), out int bookId))
            {
                NavigationService?.Navigate(new AddReview(bookId));
            }
        }

        private void LoadBooks()
        {
            int userId = Client.Id;
            var list = (from pk in _context.Прочитанные_Книги
                        where pk.ID_Пользователя == userId
                        join k in _context.Книги on pk.ID_Книги equals k.ID_Книги
                        let review = _context.Оценки
                                             .FirstOrDefault(r => r.ID_Пользователя == userId && r.ID_Книги == k.ID_Книги)
                        select new
                        {
                            ID = k.ID_Книги,
                            Название = k.Название,
                            Год = k.Год_Издания,
                            review
                        })
           .AsEnumerable() 
           .Select(x => new
           {
               x.ID,
               x.Название,
               x.Год,
               Отзыв = x.review == null ? null : $"Оценка: {x.review.Оценка}, {x.review.Комментарий}"
           })
           .ToList();

            //var list = (from pk in _context.Прочитанные_Книги
            //            where pk.ID_Пользователя == userId
            //            join k in _context.Книги on pk.ID_Книги equals k.ID_Книги
            //            
            //            let review = _context.Оценки
            //                                 .FirstOrDefault(r => r.ID_Пользователя == userId && r.ID_Книги == k.ID_Книги)
            //            select new
            //            {
            //                ID = k.ID_Книги,
            //                Название = k.Название,
            //                Год = k.Год_Издания,
            //                Отзыв = review == null ? null : $"Оценка: {review.Оценка}, {review.Комментарий}"
            //            }).ToList();


            BooksGrid.ItemsSource = list;
        }
    }
}
