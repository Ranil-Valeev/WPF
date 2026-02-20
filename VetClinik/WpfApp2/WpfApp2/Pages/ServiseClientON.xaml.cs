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
using WpfApp2.Models;

namespace WpfApp2.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServiseClientON.xaml
    /// </summary>
    public partial class ServiseClientON : Page
    {
        public int Categoria;
        public ServiseClientON(int ID_Cat)
        {
            Categoria = ID_Cat;
            InitializeComponent();
            LoadCategoryData();
        }
        private void LoadCategoryData()
        {
            using (var db = new VetClinikumEntities())
            {
                // Загрузка данных категории
                var category = db.Категория_услуг.Find(Categoria);
                if (category != null)
                {
                    CategoryTitle.Text = category.Название;
                    CategoryDescription.Text = category.Описание_услуг;
                }

                // Загрузка услуг категории
                var services = db.Услуги
                    .Where(s => s.ID_категории == Categoria)
                    .OrderBy(s => s.Название)
                    .ToList();

                ServicesGrid.ItemsSource = services;
            }
        }
        
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.AddRecord());
        }
    }
}
