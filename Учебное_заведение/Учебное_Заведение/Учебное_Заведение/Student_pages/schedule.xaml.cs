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
    /// Логика взаимодействия для schedule.xaml
    /// </summary>
    public partial class schedule : Page
    {
        private Студенты _student;

        public schedule(Студенты student)
        {
            InitializeComponent();
            _student = student;

            var context = УчебноеЗаведениеEntities.Getcontext();

            // Получаем все записи расписания для группы студента
            var расписание = context.Расписание
                .Where(r => r.ID_Группы == _student.ID_Группы)
                .ToList();

            // Список дней недели
            string[] дни = { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница" };

            var таблица = new List<dynamic>();

            foreach (var день in дни)
            {
                var пары = расписание
                    .Where(r => r.День_Недели == день)
                    .OrderBy(r => r.Номер_Пары)
                    .ToList();

                таблица.Add(new
                {
                    День = день,
                    Пара1 = FormatCell(пары, 0),
                    Пара2 = FormatCell(пары, 1),
                    Пара3 = FormatCell(пары, 2),
                    Пара4 = FormatCell(пары, 3),
                    Пара5 = FormatCell(пары, 4)
                });
            }

            ScheduleGrid.ItemsSource = таблица;
        }

        private string FormatCell(List<Расписание> пары, int index)
        {
            if (пары.ElementAtOrDefault(index) == null)
                return "";

            var пара = пары.ElementAt(index);
            var предмет = пара?.Предметы?.Название_Предмета ?? "";
            var преподаватель = пара?.Предметы?.Преподаватели?.ФИО ?? "";
            var аудитория = пара?.Аудитория ?? "";

            return $"{предмет}\n{преподаватель}\n{аудитория}";
        }
    }
}

