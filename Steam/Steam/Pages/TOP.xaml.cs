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
using System.Data.Entity;

namespace Steam.Pages
{
    /// <summary>
    /// Логика взаимодействия для TOP.xaml
    /// </summary>
    public partial class TOP : Page
    {
        public class TopGameViewModel
        {
            public int Position { get; set; }
            public string GameName { get; set; }
            public double AverageRating { get; set; }
            public int ReviewCount { get; set; }
        }
        private SteamEntities _context = new SteamEntities();

        public TOP()
        {
            InitializeComponent();
            LoadTopGames();
        }
        private void LoadTopGames()
        {
            try
            {
                // Получаем топ игр по среднему рейтингу с явным преобразованием типов
                var topGames = _context.Отзывы
                    .Include(o => o.Игра) // Подгружаем данные об игре
                    .GroupBy(r => r.Игра)
                    .Select(g => new
                    {
                        Game = g.Key,
                        AvgRating = g.Average(r => (double?)r.Рейтинг) ?? 0, // Явное преобразование и обработка null
                        Count = g.Count()
                    })
                    .Where(g => g.Count >= 1) // Фильтр по количеству отзывов
                    .OrderByDescending(g => g.AvgRating)
                    .Take(20) // Топ-20 игр
                    .ToList()
                    .Select((g, index) => new TopGameViewModel
                    {
                        Position = index + 1,
                        GameName = g.Game.Название,
                        AverageRating = g.AvgRating,
                        ReviewCount = g.Count
                    })
                    .ToList();

                TopGamesDataGrid.ItemsSource = topGames;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке топ-игр: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
