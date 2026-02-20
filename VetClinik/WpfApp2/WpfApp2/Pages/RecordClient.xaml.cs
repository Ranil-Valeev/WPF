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
using WpfApp2.Models;
using WpfApp2.Windows;
using static WpfApp2.MainWindow;
using System.Data.Entity;


namespace WpfApp2.Pages
{
    /// <summary>
    /// Логика взаимодействия для RecordClient.xaml
    /// </summary>
    public partial class RecordClient : Page
    {

        public RecordClient()
        {
            InitializeComponent();
            LoadUserRecords();
        }
        private void LoadUserRecords()
        {
            using (var db = new VetClinikumEntities())
            {
                var clientProfile = db.Клиенты.FirstOrDefault(c => c.ID_user == CurrentUser.Id);
                int Currentuser1 = clientProfile.ID_клиента;
                var pet = db.Питомцы.FirstOrDefault(c => c.ID_клиента == Currentuser1);
                int pet1 = pet.ID_питомца;
                //var records = db.История_посещений.Where(r => r.ID_питомца == pet1).ToList();
                var pets = db.Питомцы.Where(p => p.ID_клиента == clientProfile.ID_клиента).ToList();

                if (!pets.Any())
                {
                    MessageBox.Show("У клиента нет питомцев");
                    return;
                }

                // Получаем ID всех питомцев
                var petIds = pets.Select(p => p.ID_питомца).ToList();
                var records = db.История_посещений
                                .Include(h => h.Рабочее_время)  // Включаем рабочее время
                                .Include(h => h.Рабочее_время.Услуги)  // Включаем услуги через рабочее время
                                .Include(h => h.Рабочее_время.Сотрудник)  // Включаем сотрудника через рабочее время
                                .Include(h => h.Статус_записи1)  // Включаем статус записи (если есть прямая связь)
                                .Where(h => petIds.Contains(h.ID_питомца))
                                .OrderByDescending(h => h.Рабочее_время.Дата)
                                .ToList();
                RecordsGrid.ItemsSource = records;

            }
            //int currentUserId = CurrentUser.Id;
            //// Получаем записи текущего пользователя
            //var records = VetClinikumEntities.GetContext()
            //    .История_посещений
            //    .Where(r => r.ID_питомца == pet1)
            //    .ToList();
            //RecordsGrid.ItemsSource = records;
        }
    }
}

