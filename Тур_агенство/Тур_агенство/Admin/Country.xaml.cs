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
using Тур_агенство.model;
using System.Data.Entity;

namespace Тур_агенство.Admin
{
    /// <summary>
    /// Логика взаимодействия для Country.xaml
    /// </summary>
    public partial class Country : Page
    {
        private ТурАгентствоEntities _context = new ТурАгентствоEntities();
        private Страны _currentCountry;

        public Country()
        {
            InitializeComponent();
            LoadCountries();
        }

        private void LoadCountries()
        {
            _context.Страны.Load();
            CountryComboBox.ItemsSource = _context.Страны.Local;
        }

        private void CountryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentCountry = CountryComboBox.SelectedItem as Страны;
            CountryNameTextBox.Text = _currentCountry?.Название ?? "";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CountryNameTextBox.Text))
            {
                MessageBox.Show("Введите название страны!", "Ошибка");
                return;
            }

            if (_currentCountry == null)
            {
                _currentCountry = new Страны();
                _context.Страны.Add(_currentCountry);
            }

            _currentCountry.Название = CountryNameTextBox.Text.Trim();
            _context.SaveChanges();
            LoadCountries();
            MessageBox.Show("Страна сохранена!");
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            CountryComboBox.SelectedIndex = -1;
            _currentCountry = null;
            CountryNameTextBox.Clear();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentCountry == null)
            {
                MessageBox.Show("Выберите страну для удаления!");
                return;
            }

            // Проверка на наличие связанных городов
            if (_currentCountry.Города.Any())
            {
                MessageBox.Show("Невозможно удалить страну, так как есть связанные города!", "Ошибка");
                return;
            }

            if (MessageBox.Show($"Удалить страну '{_currentCountry.Название}'?", "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.Страны.Remove(_currentCountry);
                _context.SaveChanges();
                LoadCountries();
                NewButton_Click(null, null);
                MessageBox.Show("Страна удалена!");
            }
        }
    }
}
