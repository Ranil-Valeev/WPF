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
    /// Логика взаимодействия для Teachers.xaml
    /// </summary>
    public partial class Teachers : Page
    {
        private УчебноеЗаведениеEntities _context = УчебноеЗаведениеEntities.Getcontext();
        private Преподаватели _currentTeacher;

        public Teachers()
        {
            InitializeComponent();
            LoadTeachers();
            NewButton_Click(null, null);
        }

        private void LoadTeachers()
        {
            TeacherComboBox.ItemsSource = _context.Преподаватели.OrderBy(t => t.ФИО).ToList();
        }

        private void TeacherComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentTeacher = TeacherComboBox.SelectedItem as Преподаватели;
            if (_currentTeacher != null)
                FIOTextBox.Text = _currentTeacher.ФИО;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentTeacher == null || _currentTeacher.ID_Преподавателя == 0)
                {
                    _currentTeacher = new Преподаватели();
                    _context.Преподаватели.Add(_currentTeacher);
                }

                _currentTeacher.ФИО = FIOTextBox.Text.Trim();

                if (string.IsNullOrEmpty(_currentTeacher.ФИО))
                {
                    MessageBox.Show("Введите ФИО!");
                    return;
                }

                _context.SaveChanges();
                MessageBox.Show("Сохранено!");
                LoadTeachers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            _currentTeacher = new Преподаватели();
            TeacherComboBox.SelectedIndex = -1;
            FIOTextBox.Text = "";
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTeacher == null || _currentTeacher.ID_Преподавателя == 0)
            {
                MessageBox.Show("Выберите преподавателя!");
                return;
            }

            if (MessageBox.Show($"Удалить преподавателя {_currentTeacher.ФИО}?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.Преподаватели.Remove(_currentTeacher);
                _context.SaveChanges();
                MessageBox.Show("Удалено.");
                LoadTeachers();
                NewButton_Click(null, null);
            }
        }
    }
}
