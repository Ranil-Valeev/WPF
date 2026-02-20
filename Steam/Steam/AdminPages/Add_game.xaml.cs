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

namespace Steam.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для Add_game.xaml
    /// </summary>
    public partial class Add_game : Page
    {
        private SteamEntities _context = new SteamEntities();
        private Игра _currentGame;

        public Add_game()
        {
            InitializeComponent();
            LoadComboBoxData();
            LoadGames();
        }

        private void LoadComboBoxData()
        {
            try
            {
                // Загрузка разработчиков
                _context.Разработчик.Load();
                DeveloperComboBox.ItemsSource = _context.Разработчик.Local;

                // Загрузка издателей
                _context.Издатель.Load();
                PublisherComboBox.ItemsSource = _context.Издатель.Local;

                // Загрузка типов изданий
                _context.Тип_Издания.Load();
                EditionTypeComboBox.ItemsSource = _context.Тип_Издания.Local;

                // Загрузка статусов
                _context.Статус.Load();
                StatusComboBox.ItemsSource = _context.Статус.Local;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadGames()
        {
            try
            {
                _context.Игра.Load();
                GamesComboBox.ItemsSource = _context.Игра.Local;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке списка игр: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GamesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentGame = GamesComboBox.SelectedItem as Игра;

            if (_currentGame != null)
            {
                TitleTextBox.Text = _currentGame.Название;
                LocalizationTextBox.Text = _currentGame.Локализация;
                ReleaseDatePicker.SelectedDate = _currentGame.Дата_релиза;
                RegionDatePicker.SelectedDate = _currentGame.Дата_региона;
                PlatformsTextBox.Text = _currentGame.Платформы;
                GenresTextBox.Text = _currentGame.Жанры;
                AgeRatingTextBox.Text = _currentGame.Возростной_рейтинг;
                SystemRequirementsTextBox.Text = _currentGame.Системные_требования;
                DescriptionTextBox.Text = _currentGame.Описание;
                AverageTimeTextBox.Text = _currentGame.Среднее_время.ToString();

                // Установка выбранных элементов в ComboBox
                DeveloperComboBox.SelectedItem = _context.Разработчик.Local.FirstOrDefault(d => d.ID_Разработчика == _currentGame.ID_Разработчика);
                PublisherComboBox.SelectedItem = _context.Издатель.Local.FirstOrDefault(p => p.ID_Издателя == _currentGame.ID_Издателя);
                EditionTypeComboBox.SelectedItem = _context.Тип_Издания.Local.FirstOrDefault(t => t.ID_Типа_издания == _currentGame.ID_Типа_издания);
                StatusComboBox.SelectedItem = _context.Статус.Local.FirstOrDefault(s => s.ID_Статуса == _currentGame.ID_Статуса);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация данных
                if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
                {
                    MessageBox.Show("Название игры обязательно для заполнения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (DeveloperComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Выберите разработчика", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (PublisherComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Выберите издателя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (EditionTypeComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Выберите тип издания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (StatusComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Выберите статус", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!int.TryParse(AverageTimeTextBox.Text, out int averageTime))
                {
                    MessageBox.Show("Среднее время должно быть числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (_currentGame == null)
                {
                    // Добавление новой игры
                    _currentGame = new Игра();
                    _context.Игра.Add(_currentGame);
                }

                // Обновление данных игры
                _currentGame.Название = TitleTextBox.Text;
                _currentGame.Локализация = LocalizationTextBox.Text;
                _currentGame.Дата_релиза = ReleaseDatePicker.SelectedDate.HasValue ? ReleaseDatePicker.SelectedDate.Value : DateTime.Now;
                _currentGame.Дата_региона = RegionDatePicker.SelectedDate.HasValue ? RegionDatePicker.SelectedDate.Value : DateTime.Now;
                _currentGame.Платформы = PlatformsTextBox.Text;
                _currentGame.Жанры = GenresTextBox.Text;
                _currentGame.Возростной_рейтинг = AgeRatingTextBox.Text;
                _currentGame.Системные_требования = SystemRequirementsTextBox.Text;
                _currentGame.Описание = DescriptionTextBox.Text;
                _currentGame.Среднее_время = averageTime;
                _currentGame.ID_Разработчика = (DeveloperComboBox.SelectedItem as Разработчик).ID_Разработчика;
                _currentGame.ID_Издателя = (PublisherComboBox.SelectedItem as Издатель).ID_Издателя;
                _currentGame.ID_Типа_издания = (EditionTypeComboBox.SelectedItem as Тип_Издания).ID_Типа_издания;
                _currentGame.ID_Статуса = (StatusComboBox.SelectedItem as Статус).ID_Статуса;

                _context.SaveChanges();
                LoadGames();
                MessageBox.Show("Игра успешно сохранена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении игры: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            _currentGame = null;
            GamesComboBox.SelectedIndex = -1;
            TitleTextBox.Text = "";
            LocalizationTextBox.Text = "";
            ReleaseDatePicker.SelectedDate = null;
            RegionDatePicker.SelectedDate = null;
            PlatformsTextBox.Text = "";
            GenresTextBox.Text = "";
            AgeRatingTextBox.Text = "";
            SystemRequirementsTextBox.Text = "";
            DescriptionTextBox.Text = "";
            AverageTimeTextBox.Text = "";
            DeveloperComboBox.SelectedIndex = -1;
            PublisherComboBox.SelectedIndex = -1;
            EditionTypeComboBox.SelectedIndex = -1;
            StatusComboBox.SelectedIndex = -1;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentGame == null)
            {
                MessageBox.Show("Выберите игру для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show($"Вы уверены, что хотите удалить игру '{_currentGame.Название}'?\n\nВнимание: Будут удалены все связанные записи (коллекции, контент, отзывы)!",
                                       "Подтверждение удаления",
                                       MessageBoxButton.YesNo,
                                       MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Удаление связанных записей
                    var gameToDelete = _context.Игра
                        .Include(g => g.Колекция)
                        .Include(g => g.Контент)
                        .Include(g => g.Отзывы)
                        .FirstOrDefault(g => g.ID_Игры == _currentGame.ID_Игры);

                    if (gameToDelete != null)
                    {
                        // Удаление всех связанных коллекций
                        var collections = gameToDelete.Колекция.ToList();
                        foreach (var collection in collections)
                        {
                            _context.Колекция.Remove(collection);
                        }

                        // Удаление всего связанного контента
                        var content = gameToDelete.Контент.ToList();
                        foreach (var item in content)
                        {
                            _context.Контент.Remove(item);
                        }

                        // Удаление всех связанных отзывов
                        var reviews = gameToDelete.Отзывы.ToList();
                        foreach (var review in reviews)
                        {
                            _context.Отзывы.Remove(review);
                        }

                        // Удаление самой игры
                        _context.Игра.Remove(gameToDelete);
                        _context.SaveChanges();

                        LoadGames();
                        NewButton_Click(null, null);
                        MessageBox.Show("Игра и все связанные данные успешно удалены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}\n\n{ex.InnerException?.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Content = null;
        }
    }
}