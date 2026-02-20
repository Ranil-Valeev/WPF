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
using static WpfApp2.Windows.Login;
using WpfApp2.Models;

namespace WpfApp2.Pages
{
    /// <summary>
    /// Логика взаимодействия для Record_sotrudnik.xaml
    /// </summary>
    public partial class Record_sotrudnik : Page
    {
        public Record_sotrudnik()
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
                .Where(r => r.ID_Сотрудника == currentUserId)
                .ToList();
            RecordsGrid.ItemsSource = records;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                int recordId = (int)button.Tag;
                // Переход на страницу редактирования с передачей Id записи
                NavigationService?.Navigate(new Pages.EditRecordPage(recordId));
            }
        }
    }
}
