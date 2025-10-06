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
using System.Windows.Shapes;
using Университет.model;

namespace Университет
{
    /// <summary>
    /// Логика взаимодействия для Teachers.xaml
    /// </summary>
    public partial class Teachers : Window
    {
        private readonly УниверситетEntities _context = new УниверситетEntities();

        public Teachers()
        {
            InitializeComponent();

            // Загружаем категории и добавляем вариант "Все"
            var cats = _context.Категория
                               .Select(c => c.Категория1)
                               .ToList();
            cats.Insert(0, "Все");          // первый пункт - без фильтра
            Category.ItemsSource = cats;
            Category.SelectedIndex = 0;

            // В комбобокс сортировки добавляем "Без сортировки"
            Sort.Items.Insert(0, new ComboBoxItem { Content = "Без сортировки" });
            Sort.SelectedIndex = 0;

            // Поиск по фамилии
            SerName.TextChanged += (s, e) => LoadTeachers();

            LoadTeachers();
        }

        private void LoadTeachers()
        {
            var query = _context.Преподы.AsQueryable();

            // Поиск по фамилии
            if (!string.IsNullOrWhiteSpace(SerName.Text))
            {
                string search = SerName.Text.Trim().ToLower();
                query = query.Where(p => p.Фамилия.ToLower().Contains(search));
            }

            // Фильтрация по категории (пропускаем если "Все")
            if (Category.SelectedItem != null && Category.SelectedItem.ToString() != "Все")
            {
                string cat = Category.SelectedItem.ToString();
                query = query.Where(p => p.Категория.Категория1 == cat);
            }

            // Сортировка
            if (Sort.SelectedIndex > 0) // 0 — это «Без сортировки»
            {
                // +1 сдвиг, т.к. первый элемент теперь "Без сортировки"
                if (Sort.SelectedIndex == 1)
                    query = query.OrderBy(p => p.Категория.оклад);
                else if (Sort.SelectedIndex == 2)
                    query = query.OrderByDescending(p => p.Категория.оклад);
            }

            Teacher.ItemsSource = query
                .Select(p => new
                {
                    Фамилия = p.Фамилия,
                    Имя = p.Имя,
                    Отчество = p.Отчество,
                    Категория = p.Категория.Категория1,
                    Оклад = p.Категория.оклад
                })
                .ToList();
        }

        private void Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadTeachers();
        }

        private void Sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadTeachers();
        }

        private void SledSTR_Click(object sender, RoutedEventArgs e)
        {
            Load load = new Load();
            load.Show();
            this.Close();
        }
    }
}
