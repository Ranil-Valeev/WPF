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
    /// Логика взаимодействия для Branches.xaml
    /// </summary>
    public partial class Branches : Page
    {
        private Фитнес_ЗалEntities _context = new Фитнес_ЗалEntities();
        private Филиалы _currentBranch;

        public Branches()
        {
            InitializeComponent();
            LoadBranches();
        }

        private void LoadBranches()
        {
            BranchesComboBox.ItemsSource = _context.Филиалы.ToList();
            ClearFields();
        }

        private void ClearFields()
        {
            NameTextBox.Text = string.Empty;
            AddressTextBox.Text = string.Empty;
            PhoneTextBox.Text = string.Empty;
            _currentBranch = null;
        }

        private void BranchesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BranchesComboBox.SelectedItem is Филиалы selectedBranch)
            {
                _currentBranch = selectedBranch;
                FillFields(selectedBranch);
            }
        }

        private void FillFields(Филиалы branch)
        {
            NameTextBox.Text = branch.Название;
            AddressTextBox.Text = branch.Адрес;
            PhoneTextBox.Text = branch.Контактный_телефон;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация полей
                if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(AddressTextBox.Text) ||
                    string.IsNullOrWhiteSpace(PhoneTextBox.Text))
                {
                    MessageBox.Show("Заполните все обязательные поля!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (_currentBranch == null)
                {
                    // Создание нового филиала
                    var newBranch = new Филиалы
                    {
                        Название = NameTextBox.Text,
                        Адрес = AddressTextBox.Text,
                        Контактный_телефон = PhoneTextBox.Text
                    };

                    _context.Филиалы.Add(newBranch);
                }
                else
                {
                    // Обновление существующего филиала
                    _currentBranch.Название = NameTextBox.Text;
                    _currentBranch.Адрес = AddressTextBox.Text;
                    _currentBranch.Контактный_телефон = PhoneTextBox.Text;
                }

                _context.SaveChanges();
                LoadBranches();
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
            BranchesComboBox.SelectedIndex = -1;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentBranch == null)
            {
                MessageBox.Show("Выберите филиал для удаления!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Проверка на наличие связанных тренировок
                if (_currentBranch.Тренировки.Any())
                {
                    MessageBox.Show("Нельзя удалить филиал, к которому привязаны тренировки!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var result = MessageBox.Show("Вы уверены, что хотите удалить этот филиал?", "Подтверждение удаления",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _context.Филиалы.Remove(_currentBranch);
                    _context.SaveChanges();
                    LoadBranches();
                    MessageBox.Show("Филиал удален успешно!", "Успех",
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
