using Magnit.Model;
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

namespace Magnit.Pages
{
    /// <summary>
    /// Логика взаимодействия для Priduct.xaml
    /// </summary>
    public partial class Priduct : Page
    {
        private MagnitEntities _context = new MagnitEntities();
        public Priduct()
        {
            InitializeComponent();
            LoadProducts();
        }
        private void LoadProducts()
        {
            try
            {
                
                var products = _context.Товар.ToList();

                ProductsGrid.ItemsSource = products;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }
    }

    public class ProductViewModel
    {
        public int ID_товара { get; set; }
        public string Название { get; set; }
        public string Описание { get; set; }
        public string ПроизводительНазвание { get; set; }
        public string КатегорияНазвание { get; set; }
    }
}

