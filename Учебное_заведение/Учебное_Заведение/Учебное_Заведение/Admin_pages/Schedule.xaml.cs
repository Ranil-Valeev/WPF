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
    /// Логика взаимодействия для Schedule.xaml
    /// </summary>
    public partial class Schedule : Page
    {
        private УчебноеЗаведениеEntities context = УчебноеЗаведениеEntities.Getcontext();
        private Расписание currentSchedule;

        public Schedule()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            GroupComboBox.ItemsSource = context.Группы.OrderBy(g => g.Название_Группы).ToList();
            SubjectComboBox.ItemsSource = context.Предметы.OrderBy(s => s.Название_Предмета).ToList();
            TeacherComboBox.ItemsSource = context.Преподаватели.OrderBy(t => t.ФИО).ToList();

            ScheduleComboBox.ItemsSource = context.Расписание
                .ToList()
                .Select(r => new
                {
                    ID_Записи = r.ID_Записи,
                    Отображение = $"{r.День_Недели}, пара {r.Номер_Пары}, {r.Предметы.Название_Предмета}, {r.Группы.Название_Группы}"
                })
                .ToList();

            currentSchedule = new Расписание();
        }

        private void ScheduleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = ScheduleComboBox.SelectedValue;
            if (selected == null) return;

            currentSchedule = context.Расписание.FirstOrDefault(r => r.ID_Записи == (int)selected);
            if (currentSchedule == null) return;

            GroupComboBox.SelectedValue = currentSchedule.ID_Группы;
            RoomTextBox.Text = currentSchedule.Аудитория;
            DayComboBox.Text = currentSchedule.День_Недели;
            PairNumberComboBox.Text = currentSchedule.Номер_Пары.ToString();
            SubjectComboBox.SelectedValue = currentSchedule.ID_Предмета;
            TeacherComboBox.SelectedValue = currentSchedule.Предметы?.ID_Преподавателя;
        }

        private void SubjectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var subject = SubjectComboBox.SelectedItem as Предметы;
            if (subject != null)
            {
                TeacherComboBox.SelectedValue = subject.ID_Преподавателя;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GroupComboBox.SelectedValue == null ||
                    DayComboBox.SelectedItem == null ||
                    PairNumberComboBox.SelectedItem == null ||
                    SubjectComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Заполните все поля!");
                    return;
                }

                currentSchedule.ID_Группы = (int)GroupComboBox.SelectedValue;
                currentSchedule.День_Недели = ((ComboBoxItem)DayComboBox.SelectedItem).Content.ToString();
                currentSchedule.Номер_Пары = int.Parse(((ComboBoxItem)PairNumberComboBox.SelectedItem).Content.ToString());
                currentSchedule.ID_Предмета = (int)SubjectComboBox.SelectedValue;
                currentSchedule.Аудитория = RoomTextBox.Text.Trim();

                // Проверка на пересечение пар у преподавателя
                var subject = context.Предметы.First(p => p.ID_Предмета == currentSchedule.ID_Предмета);
                int teacherId = subject.ID_Преподавателя;

                bool hasConflict = context.Расписание.Any(r =>
                    r.ID_Записи != currentSchedule.ID_Записи &&
                    r.День_Недели == currentSchedule.День_Недели &&
                    r.Номер_Пары == currentSchedule.Номер_Пары &&
                    r.Предметы.ID_Преподавателя == teacherId);

                if (hasConflict)
                {
                    MessageBox.Show("Этот преподаватель уже ведёт пару в это время!");
                    return;
                }

                if (currentSchedule.ID_Записи == 0)
                    context.Расписание.Add(currentSchedule);

                context.SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            ScheduleComboBox.SelectedIndex = -1;
            GroupComboBox.SelectedIndex = -1;
            DayComboBox.SelectedIndex = -1;
            PairNumberComboBox.SelectedIndex = -1;
            SubjectComboBox.SelectedIndex = -1;
            TeacherComboBox.SelectedIndex = -1;
            RoomTextBox.Clear();
            currentSchedule = new Расписание();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentSchedule == null || currentSchedule.ID_Записи == 0)
            {
                MessageBox.Show("Выберите запись для удаления!");
                return;
            }

            if (MessageBox.Show("Удалить эту запись расписания?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                context.Расписание.Remove(currentSchedule);
                context.SaveChanges();
                MessageBox.Show("Запись удалена.");
                LoadData();
                NewButton_Click(null, null);
            }
        }
    }
}
