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
    /// Логика взаимодействия для Subjects.xaml
    /// </summary>
    public partial class Subjects : Page
    {
        private УчебноеЗаведениеEntities _context = УчебноеЗаведениеEntities.Getcontext();
        private Предметы _currentSubject;

        public Subjects()
        {
            InitializeComponent();
            LoadTeachers();
            LoadSubjects();
            NewButton_Click(null, null);
        }

        // Загрузка списка преподавателей
        private void LoadTeachers()
        {
            TeacherComboBox.ItemsSource = _context.Преподаватели
                .OrderBy(t => t.ФИО)
                .ToList();
        }

        // Загрузка списка предметов
        private void LoadSubjects()
        {
            SubjectComboBox.ItemsSource = _context.Предметы
                .OrderBy(s => s.Название_Предмета)
                .ToList();
        }

        // При выборе предмета из списка
        private void SubjectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentSubject = SubjectComboBox.SelectedItem as Предметы;

            if (_currentSubject != null)
            {
                SubjectNameTextBox.Text = _currentSubject.Название_Предмета;
                TeacherComboBox.SelectedValue = _currentSubject.ID_Преподавателя;
            }
        }

        // Сохранение или обновление предмета
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentSubject == null || _currentSubject.ID_Предмета == 0)
                {
                    _currentSubject = new Предметы();
                    _context.Предметы.Add(_currentSubject);
                }

                _currentSubject.Название_Предмета = SubjectNameTextBox.Text.Trim();
                _currentSubject.ID_Преподавателя = (int)TeacherComboBox.SelectedValue;

                if (string.IsNullOrWhiteSpace(_currentSubject.Название_Предмета))
                {
                    MessageBox.Show("Введите название предмета!");
                    return;
                }

                if (_currentSubject.ID_Преподавателя == null)
                {
                    MessageBox.Show("Выберите преподавателя!");
                    return;
                }

                _context.SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
                LoadSubjects();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }

        // Создание нового предмета
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            _currentSubject = new Предметы();
            SubjectComboBox.SelectedIndex = -1;
            SubjectNameTextBox.Text = "";
            TeacherComboBox.SelectedIndex = -1;
        }

        // Удаление предмета
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentSubject == null || _currentSubject.ID_Предмета == 0)
            {
                MessageBox.Show("Выберите предмет для удаления!");
                return;
            }

            if (MessageBox.Show($"Удалить предмет \"{_currentSubject.Название_Предмета}\"?",
                "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Предметы.Remove(_currentSubject);
                    _context.SaveChanges();
                    MessageBox.Show("Удалено успешно!");
                    LoadSubjects();
                    NewButton_Click(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении: " + ex.Message);
                }
            }
        }
    }
}
