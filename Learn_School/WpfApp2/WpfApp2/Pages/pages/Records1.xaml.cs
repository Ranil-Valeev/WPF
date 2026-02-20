using System;
using System.Collections.Generic;
using System.Data;
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
using static WpfApp2.Windows.Login;

namespace WpfApp2.Pages
{
    /// <summary>
    /// Логика взаимодействия для Records1.xaml
    /// </summary>
    public partial class Records1 : Page
    {
        public Records1()
        {
            InitializeComponent();
            LoadUserRecords();
        }

        private void LoadUserRecords()
        {
            int currentUserId = CurrentUser.Id;
            // Получаем записи текущего пользователя
            var records = РКИСEntities.GetContext()
                .Проведенные_услуги
                .Where(r => r.ID_Клиента == currentUserId)
                .ToList();

            RecordsGrid.ItemsSource = records;
        }
    }
}
