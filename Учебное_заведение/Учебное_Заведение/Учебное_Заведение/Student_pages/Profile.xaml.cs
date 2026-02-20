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
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        private Студенты _student;

        public Profile(Студенты student)
        {
            InitializeComponent();
            _student = student;
            LoadProfileData();
        }

        private void LoadProfileData()
        {
            var context = УчебноеЗаведениеEntities.Getcontext();

            // Основная информация
            FioText.Text = _student.ФИО;
            GroupText.Text = _student.Группы.Название_Группы;

            // Средняя оценка по всем предметам
            var оценки = context.Оценки
                .Where(o => o.ID_Студента == _student.ID_Студента)
                .Select(o => (double?)o.Оценка)
                .ToList();

            double avgGrade = оценки.Any() ? Math.Round(оценки.Average().Value, 2) : 0;
            AverageGradeText.Text = avgGrade > 0 ? avgGrade.ToString("0.00") : "нет данных";

   
            var посещения = context.Посещаемость
                .Where(p => p.ID_Студента == _student.ID_Студента)
                .Select(p => p.Присутствовал)
                .ToList();

            if (посещения.Any())
            {
                double percent = Math.Round((double)посещения.Count(p => p) / посещения.Count * 100, 1);
                AttendanceText.Text = $"{percent}%";
            }
            else
            {
                AttendanceText.Text = "нет данных";
            }
        }
    }
}
