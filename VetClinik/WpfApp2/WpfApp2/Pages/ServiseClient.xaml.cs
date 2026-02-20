using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для ServiseClient.xaml
    /// </summary>
    public partial class ServiseClient : Page
    {
        public List<Категория_услуг> ServiceCategories { get; set; }
        public ServiseClient()
        {
            InitializeComponent();
            using (var context = new VetClinikumEntities())
            {
                ServiceCategories = context.Категория_услуг.ToList();
            }

            // Установим DataContext для биндинга
            this.DataContext = this;
        }
        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем объект Категория_услуг из CommandParameter
            var button = sender as Button;
            var category = button?.CommandParameter as Категория_услуг;
            if (category != null)
            {
                // Переход на страницу DetailsPage с передачей ID категории
                NavigationService?.Navigate(new Pages.ServiseClientON(category.ID_Категории));
            }
        }

        // Обработчик кнопки "Записаться" (заготовка)
        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.AddRecord());
            //var button = sender as Button;
            //var category = button?.CommandParameter as Категория_услуг;
            //if (category != null)
            //{
            //    MessageBox.Show($"Вы выбрали категорию: {category.Название} для записи.");
            //    // Здесь можно добавить переход на страницу записи или другую логику
            //}
        }
    }
}
