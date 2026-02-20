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
    /// Логика взаимодействия для AddCollection.xaml
    /// </summary>
    public partial class AddCollection : Page
    {
        private SteamEntities _context = new SteamEntities();
        private int _userId;
        public AddCollection(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadGames();
        }
        private void LoadGames()
        {
            var allGames = _context.Игра.ToList();
            var userGameIds = _context.Колекция
                .Where(c => c.ID_User == _userId)
                .Select(c => c.ID_Игры)
                .ToList();

            GameComboBox.ItemsSource = allGames
                .Where(g => !userGameIds.Contains(g.ID_Игры))
                .OrderBy(g => g.Название)
                .ToList();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (GameComboBox.SelectedValue is int gameId)
            {
                var newEntry = new Колекция
                {
                    ID_User = _userId,
                    ID_Игры = gameId,
                    ID_Сатуса_прохождения = 3, // Устанавливаем статус по умолчанию
                    Пользовательские_тэги = TagsTextBox.Text.Trim()
                };

                if (decimal.TryParse(RatingTextBox.Text, out var rating))
                    newEntry.Оценка = rating;

                if (decimal.TryParse(HoursPlayedTextBox.Text, out var hours))
                    newEntry.Часы_игры = hours;

                try
                {
                    _context.Колекция.Add(newEntry);
                    _context.SaveChanges();
                    MessageBox.Show("Игра успешно добавлена в коллекцию!");
                    NavigationService.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите игру.");
            }
        }
    }
}
