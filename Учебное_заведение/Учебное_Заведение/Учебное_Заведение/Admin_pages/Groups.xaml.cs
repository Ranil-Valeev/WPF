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
using Учебное_Заведение.model;

namespace Учебное_Заведение.Admin_pages
{
    /// <summary>
    /// Логика взаимодействия для Groups.xaml
    /// </summary>
    public partial class Groups : Page
    {
        private УчебноеЗаведениеEntities _context = УчебноеЗаведениеEntities.Getcontext();
        private Группы _currentGroup;

        public Groups()
        {
            InitializeComponent();
            LoadGroups();
            NewButton_Click(null, null);
        }

        private void LoadGroups()
        {
            GroupComboBox.ItemsSource = _context.Группы.OrderBy(g => g.Название_Группы).ToList();
        }

        private void GroupComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentGroup = GroupComboBox.SelectedItem as Группы;
            if (_currentGroup != null)
                GroupNameTextBox.Text = _currentGroup.Название_Группы;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentGroup == null || _currentGroup.ID_Группы == 0)
                {
                    _currentGroup = new Группы();
                    _context.Группы.Add(_currentGroup);
                }

                _currentGroup.Название_Группы = GroupNameTextBox.Text.Trim();

                if (string.IsNullOrEmpty(_currentGroup.Название_Группы))
                {
                    MessageBox.Show("Введите название группы!");
                    return;
                }

                _context.SaveChanges();
                MessageBox.Show("Сохранено!");
                LoadGroups();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            _currentGroup = new Группы();
            GroupComboBox.SelectedIndex = -1;
            GroupNameTextBox.Text = "";
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentGroup == null || _currentGroup.ID_Группы == 0)
            {
                MessageBox.Show("Выберите группу!");
                return;
            }

            if (MessageBox.Show($"Удалить группу {_currentGroup.Название_Группы}?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.Группы.Remove(_currentGroup);
                _context.SaveChanges();
                MessageBox.Show("Удалено.");
                LoadGroups();
                NewButton_Click(null, null);
            }
        }
    }
}
