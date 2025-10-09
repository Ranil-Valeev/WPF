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
    /// Логика взаимодействия для Authors.xaml
    /// </summary>
    public partial class Authors : Page
    {
        private readonly Справочник_книгEntities _context = Справочник_книгEntities.GetContext();
        public Authors()
        {
            InitializeComponent();
            LoadAuthors();
        }
        private void LoadAuthors()
        {
            AuthorsGrid.ItemsSource = _context.Авторы.ToList();
        }

        private void AuthorsGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (AuthorsGrid.SelectedItem is Авторы author)
            {
                int authorId = author.ID_Автора;
                NavigationService?.Navigate(new AuthorDetails(authorId));
            }
        }
    }
}
