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
using WpfApp2.Models;
using static WpfApp2.MainWindow;
using System.Data.Entity;

namespace WpfApp2.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfileOn.xaml
    /// </summary>
    public partial class ProfileOn : Page
    {
        private int _currentClientId;
        public ProfileOn()
        {
            InitializeComponent();
            LoadProfile();
            LoadClientPets();
        }
        private void LoadProfile()
        {
            using (var db = new VetClinikumEntities())
            {
                _currentClientId = GetClientId(CurrentUser.Id, db.Database.Connection.ConnectionString);

                var user = db.Клиенты.Find(_currentClientId);
                if (user != null)
                {
                    DataContext = user;
                }
            }
        }
        private void LoadClientPets()
        {
            using (var db = new VetClinikumEntities())
            {
                PetsListView.ItemsSource = db.Питомцы
                    .Where(p => p.ID_клиента == CurrentUser.Id)
                    .ToList();
            }
        }

        public int GetClientId(int userId, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "SELECT ID_клиента FROM Клиенты WHERE ID_user = @userId",
                    connection);
                command.Parameters.AddWithValue("@userId", userId);

                return (int)command.ExecuteScalar();
            }
        }

        private void AddPet_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Pages.Pets());
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Image clickedImage = sender as Image;
            //if (clickedImage == null)
            //    return;

            //// Получаем DataContext изображения (это объект Питомец)
            //var pet = clickedImage.DataContext as Питомцы;
            //if (pet == null)
            //    return;

            //// Передаем ID питомца на страницу деталей
            //NavigationService?.Navigate(new Pages.PetDetail(pet.ID_питомца));
        }

        private void PetsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedPet = PetsListView.SelectedItem as Питомцы;
            if (selectedPet != null)
            {
                NavigationService?.Navigate(new Pages.PetDetail(selectedPet.ID_питомца));
            }
        }
    }
}
