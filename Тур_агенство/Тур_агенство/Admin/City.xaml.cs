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
using Тур_агенство.model;


namespace Тур_агенство.Admin
{
    /// <summary>
    /// Логика взаимодействия для City.xaml
    /// </summary>
    public partial class City : Page
    {
        private ТурАгентствоEntities _context = new ТурАгентствоEntities();
        private Города _currentCity;

        public City()
        {
            InitializeComponent();
            LoadCountries();
            LoadCities();
        }

        private void LoadCountries()
        {
            _context.Страны.Load();
            CountryComboBox.ItemsSource = _context.Страны.Local;
        }

        private void LoadCities()
        {
            _context.Города.Load();
            CityComboBox.ItemsSource = _context.Города.Local;
        }

        private void CityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentCity = CityComboBox.SelectedItem as Города;

            if (_currentCity != null)
            {
                CityNameTextBox.Text = _currentCity.Название ?? "";
                CountryComboBox.SelectedItem = _currentCity.Страны;
            }
            else
            {
                CityNameTextBox.Clear();
                CountryComboBox.SelectedIndex = -1;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CityNameTextBox.Text))
            {
                MessageBox.Show("Введите название города!", "Ошибка");
                return;
            }

            if (CountryComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите страну!", "Ошибка");
                return;
            }

            if (_currentCity == null)
            {
                _currentCity = new Города();
                _context.Города.Add(_currentCity);
            }

            _currentCity.Название = CityNameTextBox.Text.Trim();
            _currentCity.СтранаId = (CountryComboBox.SelectedItem as Страны).Id;

            _context.SaveChanges();
            LoadCities();
            MessageBox.Show("Город сохранён!");
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            CityComboBox.SelectedIndex = -1;
            _currentCity = null;
            CityNameTextBox.Clear();
            CountryComboBox.SelectedIndex = -1;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentCity == null)
            {
                MessageBox.Show("Выберите город для удаления!");
                return;
            }

            if (MessageBox.Show($"Удалить город '{_currentCity.Название}'?", "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.Города.Remove(_currentCity);
                _context.SaveChanges();
                LoadCities();
                NewButton_Click(null, null);
                MessageBox.Show("Город удалён!");
            }
        }
    }
}
