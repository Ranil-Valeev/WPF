using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace Справочник_Книг.User_Pages
{
    /// <summary>
    /// Логика взаимодействия для BookDetails.xaml
    /// </summary>
    public partial class BookDetails : Page
    {
        private readonly Справочник_книгEntities _context = Справочник_книгEntities.GetContext();
        private readonly int _bookId;
        public BookDetails(int bookId)
        {
            InitializeComponent();
            _bookId = bookId;
            LoadBook();
        }
        private void LoadBook()
        {
            var book = _context.Книги.FirstOrDefault(b => b.ID_Книги == _bookId);
            if (book == null) return;

            // Основные поля
            BookTitle.Text = book.Название ?? "Без названия";
            BookYear.Text = $"Год издания: {book.Год_Издания}";
            Bookhistory.Text = book.Описание; 
            // ---------- Жанр: сначала пробуем навигационное свойство, иначе запрос по FK ----------
            string genreName = null;

            // попытка через навигационное свойство (без жёсткого имени)
            var bookType = book.GetType();
            // ищем свойство, содержащее "Жанр" в имени (например "Жанр" или "Жанры")
            var navProp = bookType.GetProperties()
                                  .FirstOrDefault(p => p.Name.IndexOf("Жанр", System.StringComparison.OrdinalIgnoreCase) >= 0);

            if (navProp != null)
            {
                var genreObj = navProp.GetValue(book);
                if (genreObj != null)
                {
                    var nameProp = genreObj.GetType().GetProperty("Название")
                                   ?? genreObj.GetType().GetProperty("Name"); // на всякий случай
                    if (nameProp != null)
                        genreName = nameProp.GetValue(genreObj)?.ToString();
                }
            }

            // если не удалось через навигацию — сделаем прямой запрос по ID_Жанра
            if (string.IsNullOrEmpty(genreName))
            {
                // предполагается, что у книги есть поле ID_Жанра
                int? gid = null;
                var gidProp = bookType.GetProperty("ID_Жанра") ?? bookType.GetProperty("IDЖанра") ?? bookType.GetProperty("ID_Genre");
                if (gidProp != null)
                {
                    var val = gidProp.GetValue(book);
                    if (val != null)
                        gid = (int?)System.Convert.ToInt32(val);
                }

                if (gid.HasValue)
                {
                    genreName = _context.Жанры.FirstOrDefault(g => g.ID_Жанра == gid.Value)?.Название;
                }
            }

            BookGenre.Text = $"Жанр: {(string.IsNullOrEmpty(genreName) ? "Неизвестен" : genreName)}";

            // ---------- Рейтинги / средняя оценка ----------
            // Подгружаем отзывы (Оценки) и безопасно считаем среднее, игнорируя null
            var ratings = _context.Оценки
                                  .Where(r => r.ID_Книги == _bookId)
                                  .ToList();

            // выбираем только ненулевые оценки и преобразуем в double
            var numeric = ratings
                          .Where(r => r.Оценка != null)
                          .Select(r => (double)r.Оценка.Value)
                          .ToList();

            if (numeric.Any())
            {
                double avg = numeric.Average();
                BookRating.Text = $"Средняя оценка: {avg:F1} из 5 ({numeric.Count} шт.)";
            }
            else
            {
                BookRating.Text = "Нет оценок";
            }

            // ---------- Список отзывов с именами пользователей ----------
            var reviews = (from r in _context.Оценки
                           join u in _context.Пользователи on r.ID_Пользователя equals u.ID_Пользователя
                           where r.ID_Книги == _bookId
                           orderby r.Дата descending
                           select new
                           {
                               Пользователь = u.Логин,
                               Оценка = r.Оценка,
                               Комментарий = r.Комментарий,
                               Дата = r.Дата
                           }).ToList();

            ReviewsList.ItemsSource = reviews;
        }

        private void ADD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int userId = Client.Id; // текущий пользователь
                int bookId = _bookId;   // выбранная книга

                // Проверяем, не добавлена ли уже
                var existing = _context.Прочитанные_Книги
                    .FirstOrDefault(p => p.ID_Пользователя == userId && p.ID_Книги == bookId);

                if (existing != null)
                {
                    MessageBox.Show("Эта книга уже есть в списке прочитанных.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Создаем новую запись
                var newEntry = new Прочитанные_Книги
                {
                    ID_Пользователя = userId,
                    ID_Книги = bookId,
                    Дата_Прочтения = DateTime.Now
                };

                _context.Прочитанные_Книги.Add(newEntry);
                _context.SaveChanges();

                MessageBox.Show("Книга добавлена в список прочитанных!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении книги: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
