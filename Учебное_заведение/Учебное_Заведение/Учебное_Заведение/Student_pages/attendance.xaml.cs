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

namespace Учебное_Заведение.Student_pages
{
    /// <summary>
    /// Логика взаимодействия для attendance.xaml
    /// </summary>
    public partial class attendance : Page
    {
        private Студенты _student;

        // Кеш всех записей (в виде динамических объектов)
        private List<dynamic> _allRecords = new List<dynamic>();

        public attendance(Студенты student)
        {
            InitializeComponent();
            _student = student;
            LoadSubjects();
            LoadAttendance();
        }

        // Загружаем предметы (для фильтра)
        private void LoadSubjects()
        {
            var context = УчебноеЗаведениеEntities.Getcontext();

            var subjects = context.Посещаемость
                .Where(p => p.ID_Студента == _student.ID_Студента)
                .Select(p => p.Предметы.Название_Предмета)
                .Distinct()
                .OrderBy(n => n)
                .ToList();

            subjects.Insert(0, "Все предметы");
            SubjectFilter.ItemsSource = subjects;
            SubjectFilter.SelectedIndex = 0;
        }

        // Основная загрузка посещаемости — загружаем из БД и кэшируем
        private void LoadAttendance()
        {
            var context = УчебноеЗаведениеEntities.Getcontext();

            // вытягиваем записи из БД (ToList выполняет запрос)
            var raw = context.Посещаемость
                .Where(p => p.ID_Студента == _student.ID_Студента)
                .OrderByDescending(p => p.Дата)
                .ToList();

            // проецируем в анонимный тип, затем кастим к dynamic
            var projected = raw
                .Select(p => new
                {
                    Дата = p.Дата, // храним DateTime пока что
                    ДатаСтрока = p.Дата.ToString("dd.MM.yyyy"),
                    Предмет = p.Предметы.Название_Предмета,
                    Преподаватель = p.Предметы.Преподаватели.ФИО,
                    Статус = p.Присутствовал ? "Присутствовал" : "Отсутствовал"
                })
                .ToList();

            // Преобразуем List<анонимного> в List<dynamic>
            _allRecords = projected.Cast<dynamic>().ToList();

            // Применяем начальные фильтры (включая "Все предметы" и период)
            ApplyFilters();
        }

        // Применить фильтры к _allRecords и показать их
        private void ApplyFilters()
        {
            if (_allRecords == null) return;

            // работаем с IEnumerable<dynamic>
            IEnumerable<dynamic> filtered = _allRecords;

            // фильтр по предмету
            if (SubjectFilter.SelectedItem != null && SubjectFilter.SelectedItem.ToString() != "Все предметы")
                filtered = filtered.Where(r => r.Предмет == SubjectFilter.SelectedItem.ToString());

            // фильтр по датам (работаем с исходным DateTime в поле Дата)
            if (StartDate.SelectedDate.HasValue)
                filtered = filtered.Where(r => (DateTime)r.Дата >= StartDate.SelectedDate.Value);
            if (EndDate.SelectedDate.HasValue)
                filtered = filtered.Where(r => (DateTime)r.Дата <= EndDate.SelectedDate.Value);

            // Проекция для отображения: используем строковое представление Даты
            var finalList = filtered
                .OrderByDescending(r => (DateTime)r.Дата)
                .Select(r => new
                {
                    Дата = r.ДатаСтрока,
                    Предмет = r.Предмет,
                    Преподаватель = r.Преподаватель,
                    Статус = r.Статус
                })
                .ToList();

            AttendanceGrid.ItemsSource = finalList;

            // Обновляем сводку
            if (finalList.Count == 0)
            {
                AttendanceSummary.Text = "Нет данных о посещаемости";
                return;
            }

            double total = finalList.Count;
            double present = finalList.Count(r => r.Статус == "Присутствовал");
            double percent = Math.Round((present / total) * 100, 1);

            AttendanceSummary.Text = $"Посещаемость: {percent}% ({present} из {total})";
        }

        // Срабатывает при изменении ComboBox (предмета)
        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        // Срабатывает при изменении дат
        private void Filter_Changed(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }
    }
}
