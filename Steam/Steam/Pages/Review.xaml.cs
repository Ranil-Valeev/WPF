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

namespace Steam.Pages
{
    /// <summary>
    /// Логика взаимодействия для Review.xaml
    /// </summary>
    public partial class Review : Page
    {
        private SteamEntities _context = new SteamEntities();
        private int _userId;
        private int _gameId;
        public Review(int userID, int GameID)
        {
            InitializeComponent();
            _userId = userID;
            _gameId = GameID;
        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ReviewTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите текст отзыва.");
                return;
            }

            int? rating = null;
            if (int.TryParse(RatingTextBox.Text, out int parsedRating))
            {
                if (parsedRating < 0 || parsedRating > 10)
                {
                    MessageBox.Show("Рейтинг должен быть от 0 до 10.");
                    return;
                }
                rating = parsedRating;
            }

            var newReview = new Отзывы
            {
                ID_User = _userId,
                ID_Игры = _gameId,
                Тип_автора = "Пользователь",
                Текст_рецензии = ReviewTextBox.Text.Trim(),
                Рейтинг = rating,
                Дата = DateTime.Now,
                Лайки = 0,
                Дизлайки = 0
            };

            try
            {
                _context.Отзывы.Add(newReview);
                _context.SaveChanges();
                MessageBox.Show("Отзыв успешно добавлен!");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении отзыва: " + ex.Message);
            }
        }
    }
}
