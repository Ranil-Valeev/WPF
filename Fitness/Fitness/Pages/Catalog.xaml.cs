using Fitness.Model;
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

namespace Fitness.Pages
{
    /// <summary>
    /// Логика взаимодействия для Catalog.xaml
    /// </summary>
    public partial class Catalog : Page
    {
        private Фитнес_ЗалEntities _context = new Фитнес_ЗалEntities();
        public Catalog()
        {
            InitializeComponent();
            LoadGames();
        }
        private void LoadGames()
        {
            try
            {
                
                var games = _context.Тренировки.ToList();
                GamesDataGrid.ItemsSource = games;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки игр: {ex.Message}");
            }
        }
        private void GamesDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GamesDataGrid.SelectedItem is Тренировки selectedGame)
            {
                // Переход на страницу с подробной информацией
                NavigationService?.Navigate(new Pages.CatalogInfo(selectedGame));
            }
        }
    }
}
