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

namespace Учебное_Заведение.Teacher_pages
{
    /// <summary>
    /// Логика взаимодействия для schedule.xaml
    /// </summary>
    public partial class schedule : Page
    {
        private Преподаватели _teacher;

        public schedule(Преподаватели teacher)
        {
            InitializeComponent();
            _teacher = teacher;
            LoadSchedule();
        }

        private void LoadSchedule()
        {
            var context = УчебноеЗаведениеEntities.Getcontext();

            var rawData = (from r in context.Расписание
                           join g in context.Группы on r.ID_Группы equals g.ID_Группы
                           join p in context.Предметы on r.ID_Предмета equals p.ID_Предмета
                           where p.ID_Преподавателя == _teacher.ID_Преподавателя
                           select new
                           {
                               r.День_Недели,
                               r.Номер_Пары,
                               Группа = g.Название_Группы,
                               Предмет = p.Название_Предмета,
                               Аудитория = r.Аудитория
                           })
                           .ToList(); // ← выполняется в SQL, ошибок больше не будет

            var scheduleData = rawData
                .Select(r => new
                {
                    r.День_Недели,
                    r.Номер_Пары,
                    Время = GetTimeByPair(r.Номер_Пары),
                    r.Группа,
                    r.Предмет,
                    r.Аудитория
                })
                .GroupBy(x => new { x.День_Недели, x.Номер_Пары, x.Группа, x.Предмет, x.Аудитория }) // убираем дубли
                .Select(g => g.First()) // берём только уникальные
                .ToList();


            var orderedDays = new List<string> { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота" };

            var groupedSchedule = scheduleData
                .GroupBy(s => s.День_Недели)
                .OrderBy(g => orderedDays.IndexOf(g.Key))
                .Select(g => new
                {
                    День = g.Key,
                    Пары = g.OrderBy(p => p.Номер_Пары)
                            .Select(p => new
                            {
                                p.Время,
                                p.Предмет,
                                Группа = $"Группа: {p.Группа}",
                                Аудитория = $"Аудитория: {p.Аудитория}"
                            })
                            .ToList()
                })
                .ToList();

            DaysPanel.ItemsSource = groupedSchedule;
        }



        private string GetTimeByPair(int номер)
        {
            switch (номер)
            {
                case 1:
                    return "08:30 – 09:50";
                case 2:
                    return "10:00 – 11:20";
                case 3:
                    return "11:40 – 13:00";
                case 4:
                    return "13:10 – 14:30";
                case 5:
                    return "14:40 – 16:00";
                default:
                    return "—";
            }
        }
    }
}
