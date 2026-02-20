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

namespace Fitness.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для Catalog.xaml
    /// </summary>
    public partial class Catalog : Page
    {
        private Фитнес_ЗалEntities _context = new Фитнес_ЗалEntities();
        private Тренировки _CurrentTraining;
        public Catalog()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            CatalogTrainning.ItemsSource = _context.Тренировки.ToList();

            Fel.ItemsSource = _context.Филиалы.ToList();
            Fel.DisplayMemberPath = "Название";
            Fel.SelectedValuePath = "Id_филиала";

            _CurrentTraining = null;
            ClearFields();
        }
        private void ClearFields()
        {
            Name.Text = string.Empty;
            Type.Text = string.Empty;
            Trener.Text = string.Empty;
            Time.Text = string.Empty;
            Fake_ID.Text = string.Empty;
            Fel.SelectedIndex = -1;
        }

        private void Catalog_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CatalogTrainning.SelectedItem is Тренировки selectedTraining)
            {
                _CurrentTraining = selectedTraining;
                FillFields(selectedTraining);
            }
        }
        private void FillFields(Тренировки training)
        {
            Name.Text = training.Название;
            Type.Text = training.Тип;
            Trener.Text = training.Тренер;
            Time.Text = training.Время_проведения.ToString();
            Fake_ID.Text = training.Уникальный_код;
            Fel.SelectedValue = training.Id_филиала;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация полей
                if (string.IsNullOrWhiteSpace(Name.Text) ||
                    string.IsNullOrWhiteSpace(Type.Text) ||
                    string.IsNullOrWhiteSpace(Trener.Text) ||
                    string.IsNullOrWhiteSpace(Time.Text) ||
                    string.IsNullOrWhiteSpace(Fake_ID.Text) ||
                    Fel.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все обязательные поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверка даты
                if (!DateTime.TryParse(Time.Text, out DateTime trainingTime))
                {
                    MessageBox.Show("Некорректный формат даты и времени!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (_CurrentTraining == null)
                {
                    // Создание новой тренировки
                    var newTraining = new Тренировки
                    {
                        Название = Name.Text,
                        Тип = Type.Text,
                        Тренер = Trener.Text,
                        Время_проведения = trainingTime,
                        Уникальный_код = Fake_ID.Text,
                        Id_филиала = (int)Fel.SelectedValue
                    };

                    _context.Тренировки.Add(newTraining);
                }
                else
                {
                    // Обновление существующей тренировки
                    _CurrentTraining.Название = Name.Text;
                    _CurrentTraining.Тип = Type.Text;
                    _CurrentTraining.Тренер = Trener.Text;
                    _CurrentTraining.Время_проведения = trainingTime;
                    _CurrentTraining.Уникальный_код = Fake_ID.Text;
                    _CurrentTraining.Id_филиала = (int)Fel.SelectedValue;
                }

                _context.SaveChanges();
                Load();
                MessageBox.Show("Данные сохранены успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            _CurrentTraining = null;
            ClearFields();
            CatalogTrainning.SelectedIndex = -1;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (_CurrentTraining == null)
            {
                MessageBox.Show("Выберите тренировку для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить эту тренировку?", "Подтверждение удаления",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _context.Тренировки.Remove(_CurrentTraining);
                    _context.SaveChanges();
                    Load();
                    MessageBox.Show("Тренировка удалена успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
