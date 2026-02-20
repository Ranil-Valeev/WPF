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
    /// Логика взаимодействия для Students.xaml
    /// </summary>
    public partial class Students : Page
    {
        private УчебноеЗаведениеEntities context = УчебноеЗаведениеEntities.Getcontext();
        private Студенты currentStudent;

        public Students()
        {
            InitializeComponent();
            LoadGroups();
            LoadStudents();
            NewButton_Click(null, null);
        }

        private void LoadGroups()
        {
            GroupComboBox.ItemsSource = context.Группы.OrderBy(g => g.Название_Группы).ToList();
        }

        private void LoadStudents()
        {
            StudentComboBox.ItemsSource = context.Студенты
                .OrderBy(s => s.ФИО)
                .ToList();
        }

        private void StudentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentStudent = StudentComboBox.SelectedItem as Студенты;

            if (currentStudent != null)
            {
                FullNameTextBox.Text = currentStudent.ФИО;
                GroupComboBox.SelectedValue = currentStudent.ID_Группы;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FullNameTextBox.Text))
                {
                    MessageBox.Show("Введите ФИО студента!");
                    return;
                }

                if (GroupComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Выберите группу!");
                    return;
                }

                if (currentStudent == null || currentStudent.ID_Студента == 0)
                {
                    currentStudent = new Студенты();
                    context.Студенты.Add(currentStudent);
                }

                currentStudent.ФИО = FullNameTextBox.Text.Trim();
                currentStudent.ID_Группы = (int)GroupComboBox.SelectedValue;

                context.SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
                LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            StudentComboBox.SelectedIndex = -1;
            FullNameTextBox.Clear();
            GroupComboBox.SelectedIndex = -1;
            currentStudent = new Студенты();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentStudent == null || currentStudent.ID_Студента == 0)
            {
                MessageBox.Show("Выберите студента для удаления!");
                return;
            }

            if (MessageBox.Show($"Удалить {currentStudent.ФИО}?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    context.Студенты.Remove(currentStudent);
                    context.SaveChanges();
                    MessageBox.Show("Студент удалён.");
                    LoadStudents();
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
