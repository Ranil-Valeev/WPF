using Steam.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для Recom.xaml
    /// </summary>
    public partial class Recom : Page
    {
        private SteamEntities _context = new SteamEntities();

        private int _currentUserId = Client.CurrentClient.Id;

        public Recom()
        {
            InitializeComponent();
            LoadRecommendations();
        }

        private void LoadRecommendations()
        {
            try
            {
                // Игры пользователя из коллекции
                var userGameIds = _context.Колекция
                    .Where(k => k.ID_User == _currentUserId)
                    .Select(k => k.ID_Игры)
                    .ToList();

                // Жанры и разработчики его игр
                var favGenres = _context.Игра
                    .Where(g => userGameIds.Contains(g.ID_Игры))
                    .Select(g => g.Жанры)
                    .Distinct()
                    .ToList();

                var favDevelopers = _context.Игра
                    .Where(g => userGameIds.Contains(g.ID_Игры))
                    .Select(g => g.Разработчик.Название)
                    .Distinct()
                    .ToList();

                // Подбираем игры по жанрам/разработчикам, которых у него нет
                var recommendations = _context.Игра
                    .Where(g =>
                        (favGenres.Contains(g.Жанры) || favDevelopers.Contains(g.Разработчик.Название)) &&
                        !userGameIds.Contains(g.ID_Игры))
                    .OrderByDescending(g => g.Дата_релиза)
                    .Take(15)
                    .ToList();

                LvRecom.ItemsSource = recommendations;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки рекомендаций: " + ex.Message);
            }
        }

        private void LvRecom_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LvRecom.SelectedItem is Игра selectedGame)
            {
                NavigationService.Navigate(new Gamedetal(selectedGame));
            }
        }
    }
}
