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
using static Справочник_Книг.User_Pages.User_Window;
using Справочник_Книг.model;
using System.Data.Entity;

namespace Справочник_Книг.User_Pages
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        private Справочник_книгEntities _context;

        public Profile()
        {
            InitializeComponent();
            _context = Справочник_книгEntities.GetContext(); // используем твой GetContext()
            LoadUserData();
        }

        private void LoadUserData()
        {
            var user = _context.Пользователи.FirstOrDefault(u => u.ID_Пользователя == Client.Id);

            if (user != null)
            {
                LoginText.Text = $"Логин: {user.Логин}";
                PhoneText.Text = $"Телефон: {user.Телефон}";
                EmailText.Text = $"Email: {user.Электронная_Почта}";

                // Количество прочитанных книг
                int booksCount = _context.Прочитанные_Книги.Count(p => p.ID_Пользователя == user.ID_Пользователя);
                BooksButton.Content = $"Прочитанные книги: {booksCount}";

                // Количество отзывов
                int reviewsCount = _context.Оценки.Count(o => o.ID_Пользователя == user.ID_Пользователя);
                ReviewsButton.Content = $"Оставленные отзывы: {reviewsCount}";
            }
        }

        private void BooksButton_Click(object sender, RoutedEventArgs e)
        {
            // переход на страницу с книгами пользователя
            this.NavigationService.Navigate(new UserBooks());
        }

        private void ReviewsButton_Click(object sender, RoutedEventArgs e)
        {
            // переход на страницу с отзывами пользователя
            this.NavigationService.Navigate(new UserReviews());
        }
    }
}
