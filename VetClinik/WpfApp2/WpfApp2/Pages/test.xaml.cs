using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using static WpfApp2.MainWindow;
using WpfApp2.Models;
using System.Data.Entity;

namespace WpfApp2.Pages
{
    /// <summary>
    /// Логика взаимодействия для test.xaml
    /// </summary>
    public partial class test : Page
    {
       
        public test()
        {
            InitializeComponent();
            LoadPetsPhotos();
        }
        private void LoadPetsPhotos()
        {

            using (var db = new VetClinikumEntities())
            {
                var pets = db.Питомцы
                    .Where(p => p.ID_клиента == CurrentUser.Id && p.Фото != null)
                    .ToList();

                PhotosGrid.ItemsSource = pets;
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if (sender is Image image && image.DataContext is Питомцы pet)
            //{
            //    // Переход на страницу с подробной информацией о питомце
            //    NavigationService.Navigate(new PetDetail(/*pet.ID_питомца*/));
            //}
        }
    }
}
