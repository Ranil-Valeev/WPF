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

namespace Magnit.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для Clients.xaml
    /// </summary>
    public partial class Clients : Page
    {
        private MagnitEntities _context = new MagnitEntities();
        public Clients()
        {
            InitializeComponent();
            LoadUsers();
        }
        private void LoadUsers()
        {
            try
            {
                var users = _context.Пользователь
                    .OrderBy(u => u.Фамилия)
                    .ThenBy(u => u.Имя)
                    .ToList();

                UsersGrid.ItemsSource = users;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }
    }
}
