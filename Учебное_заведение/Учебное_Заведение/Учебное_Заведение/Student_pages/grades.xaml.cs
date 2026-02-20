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
    /// Логика взаимодействия для grades.xaml
    /// </summary>
    public partial class grades : Page
    {
        private Студенты _student;

        public grades(Студенты student)
        {
            InitializeComponent();
            _student = student;
            LoadGrades();
        }

        private void LoadGrades()
        {
            var context = УчебноеЗаведениеEntities.Getcontext();
            var оценки = context.Оценки
                .Where(o => o.ID_Студента == _student.ID_Студента)
                .ToList();
            if (оценки.Count == 0)
            {
                GradesGrid.ItemsSource = new List<dynamic>
                {
                    new { Предмет = "Нет данных", Оценки = "", Средний = 0.0 }
                };
                return;
            }
            var grouped = оценки
                .GroupBy(o => o.Предметы.Название_Предмета)
                .Select(g => new
                {
                    Предмет = g.Key,
                    Оценки = string.Join(", ", g.Select(x => x.Оценка.ToString())),
                    Средний = Math.Round(g.Average(x => x.Оценка), 2)
                })
                .OrderBy(g => g.Предмет)
                .ToList();
            GradesGrid.ItemsSource = grouped;

        }
    }
}
