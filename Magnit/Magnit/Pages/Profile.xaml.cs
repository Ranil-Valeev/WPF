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
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        private readonly int _userId;
        private readonly MagnitEntities _db = new MagnitEntities();
        public Profile(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadUserData();
        }
        private void LoadUserData()
        {
            try
            {
                var user = _db.Пользователь.FirstOrDefault(u => u.ID_пользователя == _userId);
                if (user != null)
                {
                    DataContext = new UserProfileViewModel
                    {
                        FullName = $"{user.Фамилия} {user.Имя}",
                        LastName = user.Фамилия,
                        FirstName = user.Имя,
                        MiddleName = user.Отчество,
                        Phone = user.Номер_телефона,
                        Email = user.электронная_почта,
                        Gender = user.пол == "М" ? "Мужской" : "Женский",
                        BirthDate = user.Возраст ?? DateTime.Now.AddYears(-18)
                    };
                }
                else
                {
                    MessageBox.Show("Пользователь не найден!", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
;
            }
        }
    }

    public class UserProfileViewModel
    {
        public string FullName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
    }
}

