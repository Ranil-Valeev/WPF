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
    /// Логика взаимодействия для AddReview.xaml
    /// </summary>
    public partial class AddReview : Page
    {
        private Справочник_книгEntities _context;
        private int _bookId;
        public AddReview(int bookId)
        {
            InitializeComponent();
            _context = Справочник_книгEntities.GetContext();
            _bookId = bookId;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int score = int.Parse(ScoreBox.Text);

                if (score < 1 || score > 5)
                {
                    MessageBox.Show("Оценка должна быть от 1 до 5.");
                    return;
                }

                var review = new Оценки
                {
                    ID_Пользователя = Client.Id,
                    ID_Книги = _bookId,
                    Оценка = score,
                    Комментарий = CommentBox.Text,
                    Дата = DateTime.Now
                };

                _context.Оценки.Add(review);
                _context.SaveChanges();

                MessageBox.Show("Отзыв успешно сохранён!");
                NavigationService?.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}
