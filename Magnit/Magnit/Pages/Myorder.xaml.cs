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
    /// Логика взаимодействия для Myorder.xaml
    /// </summary>
    public partial class Myorder : Page
    {
        private MagnitEntities _context = new MagnitEntities();
        public Myorder(int userId)
        {
            InitializeComponent();
            LoadUserOrders(userId);
        }
        private void LoadUserOrders(int userId)
        {
            try
            {
                var user = _context.Пользователь
                    .FirstOrDefault(u => u.ID_пользователя == userId);


                if (user != null)
                {
                    lblUserInfo.Text = $"{user.Фамилия} {user.Имя}";
                    dataGridOrders.ItemsSource = user.Заказ.ToList();
                }
                else
                {
                    MessageBox.Show("Пользователь не найден");
                    
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }
    }
}
