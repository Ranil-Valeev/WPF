using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Учебное_Заведение.Teacher_pages
{
    /// <summary>
    /// Логика взаимодействия для lesson.xaml
    /// </summary>
    public partial class lesson : Page
    {
        private Преподаватели _teacher;
        private int _currentSubjectId = 0;
        private int _currentGroupId = 0;

        public lesson(Преподаватели teacher)
        {
            InitializeComponent();
            _teacher = teacher;
            LoadCurrentLesson();
        }

        private void LoadCurrentLesson()
        {
            var now = DateTime.Now;
            string day = now.ToString("dddd", new CultureInfo("ru-RU"));
            int pairNumber = GetCurrentPair(now);

            var context = УчебноеЗаведениеEntities.Getcontext();

            var current = (from r in context.Расписание
                           join g in context.Группы on r.ID_Группы equals g.ID_Группы
                           join p in context.Предметы on r.ID_Предмета equals p.ID_Предмета
                           where p.ID_Преподавателя == _teacher.ID_Преподавателя
                                 && r.День_Недели == day
                                 && r.Номер_Пары == pairNumber
                           select new { r, g, p }).FirstOrDefault();

            if (current == null)
            {
                CurrentLessonInfo.Text = "Сейчас у вас нет занятий.";
                StudentsPanel.ItemsSource = null;
                return;
            }

            _currentSubjectId = current.p.ID_Предмета;
            _currentGroupId = current.g.ID_Группы;

            CurrentLessonInfo.Text =
                $"{current.p.Название_Предмета} | {current.g.Название_Группы} | Пара №{current.r.Номер_Пары}";

            var students = context.Студенты
                .Where(s => s.ID_Группы == _currentGroupId)
                .Select(s => new TeacherStudentRow
                {
                    ID_Студента = s.ID_Студента,
                    ФИО = s.ФИО,
                    Присутствует = false,
                    Оценка = string.Empty
                }).ToList();

            // Загрузка прошлых записей на сегодня
            foreach (var row in students)
            {
                var pres = context.Посещаемость
                    .FirstOrDefault(p => p.ID_Студента == row.ID_Студента &&
                                         p.Дата == DateTime.Today &&
                                         p.ID_Преподавателя == _teacher.ID_Преподавателя);

                if (pres != null) row.Присутствует = pres.Присутствовал;

                var lastGrade = (from gr in context.Оценки
                                 where gr.ID_Студента == row.ID_Студента &&
                                       gr.ID_Предмета == _currentSubjectId
                                 orderby gr.Дата descending
                                 select gr).FirstOrDefault();

                if (lastGrade != null)
                    row.Оценка = lastGrade.Оценка.ToString();
            }

            StudentsPanel.ItemsSource = students;
        }

        private int GetCurrentPair(DateTime time)
        {
            int minutes = time.Hour * 60 + time.Minute;
            if (minutes >= 510 && minutes < 590) return 1;   // 8:30–9:50
            if (minutes >= 590 && minutes < 670) return 2;   // 9:50–11:10
            if (minutes >= 700 && minutes < 780) return 3;   // 11:40–13:00
            if (minutes >= 790 && minutes < 870) return 4;   // 13:10–14:30
            if (minutes >= 880 && minutes < 960) return 5;   // 14:40–16:00
            return 0;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var context = УчебноеЗаведениеEntities.Getcontext();
            var students = StudentsPanel.ItemsSource as IEnumerable<TeacherStudentRow>;
            if (students == null || _currentSubjectId == 0) return;

            foreach (var row in students)
            {
                // Удаляем старую запись на сегодня, если есть
                var existingPres = context.Посещаемость
                    .FirstOrDefault(p => p.ID_Студента == row.ID_Студента &&
                                         p.Дата == DateTime.Today &&
                                         p.ID_Преподавателя == _teacher.ID_Преподавателя &&
                                         p.ID_Предмета == _currentSubjectId);
                if (existingPres != null)
                    context.Посещаемость.Remove(existingPres);

                // Добавляем новую
                var pres = new Посещаемость
                {
                    ID_Студента = row.ID_Студента,
                    Дата = DateTime.Today,
                    Присутствовал = row.Присутствует,
                    ID_Преподавателя = _teacher.ID_Преподавателя,
                    ID_Предмета = _currentSubjectId
                };
                context.Посещаемость.Add(pres);

                // Оценка
                int gradeValue;
                if (int.TryParse(row.Оценка, out gradeValue) && gradeValue >= 1 && gradeValue <= 5)
                {
                    var grade = new Оценки
                    {
                        ID_Студента = row.ID_Студента,
                        ID_Предмета = _currentSubjectId,
                        Оценка = gradeValue,
                        Дата = DateTime.Now,
                        ID_Преподавателя = _teacher.ID_Преподавателя
                    };
                    context.Оценки.Add(grade);
                }
            }

            context.SaveChanges();
            MessageBox.Show("Изменения успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public class TeacherStudentRow
    {
        public int ID_Студента { get; set; }
        public string ФИО { get; set; }
        public bool Присутствует { get; set; }
        public string Оценка { get; set; }
    }
}

