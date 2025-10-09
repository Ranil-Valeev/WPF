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

namespace Справочник_Книг.User_Pages
{
    /// <summary>
    /// Логика взаимодействия для UserReviews.xaml
    /// </summary>
    public partial class UserReviews : Page
    {
        private Справочник_книгEntities _context;
        public UserReviews()
        {
            InitializeComponent();
            _context = Справочник_книгEntities.GetContext();
            LoadReviews();
        }
        private void LoadReviews()
        {
            int userId = Client.Id;

            var list = (from r in _context.Оценки
                        where r.ID_Пользователя == userId
                        join k in _context.Книги on r.ID_Книги equals k.ID_Книги
                        select new
                        {
                            k.Название,
                            r.Оценка,
                            r.Комментарий,
                            r.Дата
                        }).ToList();

            ReviewsGrid.ItemsSource = list;
        }
    }
}
