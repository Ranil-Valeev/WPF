using Steam.Model;
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
using static Steam.Client;
using System.Data.Entity;
using System.Xml.Linq;

namespace Steam.Pages
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        private SteamEntities _context = new SteamEntities();
        private int _currentUserId;
        public string Photo;
        public Profile()
        {
            InitializeComponent();
            _currentUserId = CurrentClient.Id;
            Loaded += Profile_Loaded; 
        }
        private async void Profile_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var userProfile = _context.Users
                    .Where(u => u.ID_User == _currentUserId)
                    .Select(u => new
                    {
                        Ник = u.Ник,
                        Год_рождения = u.Год_рождения,
                        Адрес = u.Адрес,
                        Фото = u.Фото // предполагаем, что GetPhoto — это строка с путём к изображению
                    })
                    .FirstOrDefault();

                if (userProfile != null)
                {
                    // Устанавливаем DataContext для привязки в XAML
                    DataContext = new
                    {
                        Ник = userProfile.Ник,
                        Год_рождения = userProfile.Год_рождения?.ToShortDateString() ?? "Не указано",
                        Адрес = userProfile.Адрес ?? "Не указано",      
                        GetPhoto = userProfile.Фото ?? "Не указано"
                    };
                }
                else
                {
                    MessageBox.Show("Профиль пользователя не найден.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки профиля: {ex.Message}");
            }

        }
    }
}
