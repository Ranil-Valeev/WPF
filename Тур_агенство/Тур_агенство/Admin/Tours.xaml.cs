using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для Tours.xaml
    /// </summary>
    public partial class Tours : Page
    {
        private ТурАгентствоEntities _context = new ТурАгентствоEntities();
        private Туры _currentTour;
        private string _selectedPhotoPath;

        public Tours()
        {
            InitializeComponent();
            LoadCities();
            LoadTours();
        }

        private void LoadCities()
        {
            _context.Города.Load();
            CityComboBox.ItemsSource = _context.Города.Local;
        }

        private void LoadTours()
        {
            _context.Туры.Load();
            TourComboBox.ItemsSource = _context.Туры.Local;
        }

        private void TourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentTour = TourComboBox.SelectedItem as Туры;

            if (_currentTour != null)
            {
                TourNameTextBox.Text = _currentTour.Название ?? "";
                DescriptionTextBox.Text = _currentTour.Описание ?? "";
                PriceTextBox.Text = _currentTour.Цена.ToString();
                CityComboBox.SelectedItem = _currentTour.Города;
                StartDatePicker.SelectedDate = _currentTour.ДатаНачала;
                EndDatePicker.SelectedDate = _currentTour.ДатаОкончания;
                PhotoTextBox.Text = _currentTour.Фото ?? "";

                LoadPhotoPreview();
            }
            else
            {
                ClearFields();
            }
        }

        private void SelectPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                Title = "Выберите фото"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedPhotoPath = openFileDialog.FileName;
                string fileName = System.IO.Path.GetFileName(_selectedPhotoPath);
                PhotoTextBox.Text = fileName;

                // Показываем превью выбранного файла
                try
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(_selectedPhotoPath, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    PhotoPreview.Source = bitmap;
                }
                catch
                {
                    PhotoPreview.Source = null;
                }
            }
        }

        private void LoadPhotoPreview()
        {
            if (_currentTour == null || string.IsNullOrEmpty(_currentTour.Фото))
            {
                PhotoPreview.Source = null;
                return;
            }

            try
            {
                // Используем свойство GetFhoto из модели
                string photoPath = _currentTour.GetFhoto;

                if (!string.IsNullOrEmpty(photoPath) && File.Exists(photoPath))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(photoPath, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    PhotoPreview.Source = bitmap;
                }
                else
                {
                    PhotoPreview.Source = null;
                }
            }
            catch (Exception ex)
            {
                PhotoPreview.Source = null;
                // Переменная "ex" теперь используется
                Console.WriteLine($"Ошибка загрузки фото: {ex.Message}");
                // Для отладки:
                // MessageBox.Show($"Ошибка загрузки фото: {ex.Message}");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TourNameTextBox.Text))
            {
                MessageBox.Show("Введите название тура!", "Ошибка");
                return;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price))
            {
                MessageBox.Show("Введите корректную цену!", "Ошибка");
                return;
            }

            if (CityComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите город!", "Ошибка");
                return;
            }

            if (!StartDatePicker.SelectedDate.HasValue || !EndDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Выберите даты начала и окончания!", "Ошибка");
                return;
            }

            if (StartDatePicker.SelectedDate >= EndDatePicker.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть раньше даты окончания!", "Ошибка");
                return;
            }

            if (_currentTour == null)
            {
                _currentTour = new Туры();
                _context.Туры.Add(_currentTour);
            }

            _currentTour.Название = TourNameTextBox.Text.Trim();
            _currentTour.Описание = DescriptionTextBox.Text.Trim();
            _currentTour.Цена = price;
            _currentTour.ГородId = (CityComboBox.SelectedItem as Города).Id;
            _currentTour.ДатаНачала = StartDatePicker.SelectedDate.Value;
            _currentTour.ДатаОкончания = EndDatePicker.SelectedDate.Value;

            // Сохранение фото
            if (!string.IsNullOrEmpty(_selectedPhotoPath))
            {
                try
                {
                    string imagesFolder = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Images");

                    // Создаем папку Images, если её нет
                    if (!Directory.Exists(imagesFolder))
                    {
                        Directory.CreateDirectory(imagesFolder);
                    }

                    string fileName = System.IO.Path.GetFileName(_selectedPhotoPath);
                    string destPath = System.IO.Path.Combine(imagesFolder, fileName);

                    // Копируем файл только если это другой файл
                    if (_selectedPhotoPath != destPath)
                    {
                        File.Copy(_selectedPhotoPath, destPath, true);
                    }

                    // Сохраняем только имя файла в БД
                    _currentTour.Фото = fileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении фото: {ex.Message}", "Ошибка");
                    return;
                }
            }

            _context.SaveChanges();
            LoadTours();
            MessageBox.Show("Тур сохранён!");
            _selectedPhotoPath = null;
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            TourComboBox.SelectedIndex = -1;
            _currentTour = null;
            _selectedPhotoPath = null;
            ClearFields();
        }

        private void ClearFields()
        {
            TourNameTextBox.Clear();
            DescriptionTextBox.Clear();
            PriceTextBox.Clear();
            CityComboBox.SelectedIndex = -1;
            StartDatePicker.SelectedDate = null;
            EndDatePicker.SelectedDate = null;
            PhotoTextBox.Clear();
            PhotoPreview.Source = null;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTour == null)
            {
                MessageBox.Show("Выберите тур для удаления!");
                return;
            }

            // Проверка на наличие связанных бронирований
            if (_currentTour.Бронирования.Any())
            {
                MessageBox.Show("Невозможно удалить тур, так как есть связаные бронирования!", "Ошибка");
                return;
            }

            if (MessageBox.Show($"Удалить тур '{_currentTour.Название}'?", "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.Туры.Remove(_currentTour);
                _context.SaveChanges();
                LoadTours();
                NewButton_Click(null, null);
                MessageBox.Show("Тур удалён!");
            }
        }
    }
}
