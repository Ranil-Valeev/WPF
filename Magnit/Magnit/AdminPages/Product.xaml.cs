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
    /// Логика взаимодействия для Product.xaml
    /// </summary>
    public partial class Product : Page
    {
        private MagnitEntities _context = new MagnitEntities();
        private Товар _currentProduct;
        public Product()
        {
            InitializeComponent();
            LoadData();
            SetNewProductMode();
        }
        private void LoadData()
        {
            // Загрузка товаров, категорий и производителей
            _context.Товар.Include(t => t.Категория).Include(t => t.Производитель).Load();
            _context.Категория.Load();
            _context.Производитель.Load();

            cbProducts.ItemsSource = _context.Товар.Local;
            cbCategories.ItemsSource = _context.Категория.Local;
            cbManufacturers.ItemsSource = _context.Производитель.Local;
        }

        private void SetNewProductMode()
        {
            _currentProduct = new Товар();
            cbProducts.SelectedIndex = -1;
            txtName.Text = "";
            txtDescription.Text = "";
            cbCategories.SelectedIndex = -1;
            cbManufacturers.SelectedIndex = -1;
        }

        private void cbProducts_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (cbProducts.SelectedItem is Товар selectedProduct)
            {
                _currentProduct = selectedProduct;
                txtName.Text = _currentProduct.название;
                txtDescription.Text = _currentProduct.описание;
                cbCategories.SelectedItem = _currentProduct.Категория;
                cbManufacturers.SelectedItem = _currentProduct.Производитель;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название товара");
                return;
            }

            // Обновляем данные товара
            _currentProduct.название = txtName.Text;
            _currentProduct.описание = txtDescription.Text;
            _currentProduct.Категория = cbCategories.SelectedItem as Категория;
            _currentProduct.Производитель = cbManufacturers.SelectedItem as Производитель;

            // Если это новый товар - добавляем в контекст
            if (_currentProduct.ID_товара == 0)
            {
                _context.Товар.Add(_currentProduct);
            }

            try
            {
                _context.SaveChanges();
                MessageBox.Show("Данные сохранены успешно");

                // Обновляем список товаров
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
            SetNewProductMode();
        }
    }
}
