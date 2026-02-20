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
    /// Логика взаимодействия для Rec.xaml
    /// </summary>
    public partial class Rec : Page
    {
        private Фитнес_ЗалEntities _context = new Фитнес_ЗалEntities();
        private Рекомендации _currentRecommendation;

        public Rec()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // Загрузка списка рекомендаций
            RecommendationsComboBox.ItemsSource = _context.Рекомендации.ToList();

            // Загрузка списка тренировок
            var trainings = _context.Тренировки.ToList();
            MainTrainingComboBox.ItemsSource = trainings;
            RecommendedTrainingComboBox.ItemsSource = trainings;

            ClearFields();
        }

        private void ClearFields()
        {
            MainTrainingComboBox.SelectedIndex = -1;
            RecommendedTrainingComboBox.SelectedIndex = -1;
            ReasonTextBox.Text = string.Empty;
            _currentRecommendation = null;
        }

        private void RecommendationsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RecommendationsComboBox.SelectedItem is Рекомендации selectedRecommendation)
            {
                _currentRecommendation = selectedRecommendation;
                FillFields(selectedRecommendation);
            }
        }

        private void FillFields(Рекомендации recommendation)
        {
            MainTrainingComboBox.SelectedValue = recommendation.Id_тренировки;
            RecommendedTrainingComboBox.SelectedValue = recommendation.Id_рекомендованной_тренировки;
            ReasonTextBox.Text = recommendation.Причина;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация полей
                if (MainTrainingComboBox.SelectedItem == null ||
                    RecommendedTrainingComboBox.SelectedItem == null ||
                    string.IsNullOrWhiteSpace(ReasonTextBox.Text))
                {
                    MessageBox.Show("Заполните все обязательные поля!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверка, чтобы основная и рекомендуемая тренировки не совпадали
                if ((int)MainTrainingComboBox.SelectedValue == (int)RecommendedTrainingComboBox.SelectedValue)
                {
                    MessageBox.Show("Основная и рекомендуемая тренировки не могут совпадать!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (_currentRecommendation == null)
                {
                    // Создание новой рекомендации
                    var newRecommendation = new Рекомендации
                    {
                        Id_тренировки = (int)MainTrainingComboBox.SelectedValue,
                        Id_рекомендованной_тренировки = (int)RecommendedTrainingComboBox.SelectedValue,
                        Причина = ReasonTextBox.Text
                    };

                    _context.Рекомендации.Add(newRecommendation);
                }
                else
                {
                    // Обновление существующей рекомендации
                    _currentRecommendation.Id_тренировки = (int)MainTrainingComboBox.SelectedValue;
                    _currentRecommendation.Id_рекомендованной_тренировки = (int)RecommendedTrainingComboBox.SelectedValue;
                    _currentRecommendation.Причина = ReasonTextBox.Text;
                }

                _context.SaveChanges();
                LoadData();
                MessageBox.Show("Данные сохранены успешно!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            RecommendationsComboBox.SelectedIndex = -1;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentRecommendation == null)
            {
                MessageBox.Show("Выберите рекомендацию для удаления!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить эту рекомендацию?", "Подтверждение удаления",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _context.Рекомендации.Remove(_currentRecommendation);
                    _context.SaveChanges();
                    LoadData();
                    MessageBox.Show("Рекомендация удалена успешно!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
