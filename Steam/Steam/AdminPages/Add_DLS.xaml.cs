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
    /// Логика взаимодействия для Add_DLS.xaml
    /// </summary>
    public partial class Add_DLS : Page
    {
        private SteamEntities _context = new SteamEntities();
        public Add_DLS()
        {
            InitializeComponent();
            LoadGames();
        }
        private void LoadGames()
        {
            try
            {
                _context.Игра.Load();
                GameComboBox.ItemsSource = _context.Игра.Local;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке списка игр: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация данных
                if (GameComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Выберите игру", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
                {
                    MessageBox.Show("Введите название контента", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Создание нового доп. контента
                var newContent = new Контент
                {
                    ID_Игры = ((Игра)GameComboBox.SelectedItem).ID_Игры,
                    Название = TitleTextBox.Text,
                    Тип_контента = ContentTypeTextBox.Text,
                    Описание = DescriptionTextBox.Text,                  
                    Дата = ReleaseDatePicker.SelectedDate.HasValue ? ReleaseDatePicker.SelectedDate.Value : default(DateTime),
                    Цена = PriceTextBox.Text
                };

                _context.Контент.Add(newContent);
                _context.SaveChanges();

                MessageBox.Show("Дополнительный контент успешно добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                Content = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Content = null;
        }
    }
}
