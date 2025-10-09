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
    /// Логика взаимодействия для RecommendationsPage.xaml
    /// </summary>
    public partial class RecommendationsPage : Page
    {
        private readonly RecommendationService _recommendationService;
        public RecommendationsPage()
        {
            InitializeComponent();
            _recommendationService = new RecommendationService();
            LoadRecommendations();
        }
        private void LoadRecommendations()
        {
            var books = _recommendationService.GetRecommendations(Client.Id);
            BooksList.ItemsSource = books;
        }

        private void BooksList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (BooksList.SelectedItem is Книги books)
            {
                int authorId = books.ID_Книги;
                NavigationService?.Navigate(new BookDetails(authorId));
            }
        }
    }
}
