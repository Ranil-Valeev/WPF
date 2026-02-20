using Steam.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
//using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
namespace Steam.Pages
{
    /// <summary>
    /// Логика взаимодействия для Company.xaml
    /// </summary>
    public partial class Company : Page
    {
        private SteamEntities _context = new SteamEntities();
        public List<Издатель> ServiceCategories { get; set; }
        public Company()
        {
            InitializeComponent();
            using (var context = new SteamEntities())
            {
                ServiceCategories = context.Издатель.ToList();
            }

            // Установим DataContext для биндинга
            this.DataContext = this;
        }
        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем объект Категория_услуг из CommandParameter
            var button = sender as Button;
            var category = button?.CommandParameter as Издатель;
            if (category != null)
            {
                // Переход на страницу DetailsPage с передачей ID категории
                NavigationService?.Navigate(new Pages.Gamecom(category.ID_Издателя));
            }
        }

    }
}
