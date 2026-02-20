using Magnit.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace Magnit.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для Category.xaml
    /// </summary>
    public partial class Category : Page
    {
        private MagnitEntities _context = new MagnitEntities();
        private Категория _currentCategory;
        public Category()
        {
            InitializeComponent();
            LoadData();
            SetNewCategoryMode();
        }
        private void LoadData()
        {
            // Загрузка категорий из базы данных
            _context.Категория.Load();
            cbCategories.ItemsSource = _context.Категория.Local;
        }

        private void SetNewCategoryMode()
        {
            _currentCategory = new Категория();
            cbCategories.SelectedIndex = -1;
            txtName.Text = "";
        }

        private void cbCategories_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (cbCategories.SelectedItem is Категория selectedCategory)
            {
                _currentCategory = selectedCategory;
                txtName.Text = _currentCategory.название;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название категории");
                return;
            }

            // Обновляем данные категории
            _currentCategory.название = txtName.Text;

            // Если это новая категория - добавляем в контекст
            if (_currentCategory.ID_категории == 0)
            {
                _context.Категория.Add(_currentCategory);
            }

            try
            {
                _context.SaveChanges();
                MessageBox.Show("Данные сохранены успешно");

                // Обновляем список категорий
                _context = new MagnitEntities();
                LoadData();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            SetNewCategoryMode();
        }
    }
}
