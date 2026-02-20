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
    /// Логика взаимодействия для Game.xaml
    /// </summary>
    public partial class Game : Page
    {
        private SteamEntities _context = new SteamEntities();
        public Game()
        {
            InitializeComponent();
            LoadGames();
        }
        private void LoadGames()
        {
            try
            {
                // Получаем все игры из базы (с полем GetPhoto)
                var games = _context.Игра.ToList();
                GamesDataGrid.ItemsSource = games;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки игр: {ex.Message}");
            }
        }

        private void GamesDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GamesDataGrid.SelectedItem is Игра selectedGame)
            {
                // Переход на страницу с подробной информацией
                NavigationService?.Navigate(new Pages.Gamedetal(selectedGame));
            }
        }
    }
}
