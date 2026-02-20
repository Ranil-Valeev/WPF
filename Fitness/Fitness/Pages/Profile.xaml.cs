using Fitness.Model;
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
using static Fitness.Client;

namespace Fitness.Pages
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        private Фитнес_ЗалEntities _context = new Фитнес_ЗалEntities();
        public Profile()
        {
            InitializeComponent();
            Profile_Loaded();
        }
        private async void Profile_Loaded()
        {
            try
            {
                var userProfile = _context.Клиенты
                    .Where(u => u.Id_клиента == CurrentClient.Id)
                    .Select(u => new
                    {
                        Логин = u.Логин,
                        Дата_регистрации = u.Дата_регистрации,
                        Email = u.Email,
                    })
                    .FirstOrDefault();

                if (userProfile != null)
                {
                   
                    DataContext = new
                    {
                        Логин = userProfile.Логин,
                        Дата_регистрации = userProfile.Дата_регистрации,
                        Email = userProfile.Email ?? "Не указано",     
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
