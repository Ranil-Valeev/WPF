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
    /// Логика взаимодействия для Records.xaml
    /// </summary>
    public partial class Records : Page
    {
        private Журнал _currentClient = new Журнал();
        private readonly Фитнес_ЗалEntities _context = new Фитнес_ЗалEntities();
        private readonly int _clientId;
        public Records()
        {
            InitializeComponent();
            _clientId = CurrentClient.Id;
            UpdateStatuses();
            LoadVisits();
        }
        private void UpdateStatuses()
        {
            var today = DateTime.Today;
            var visitsToUpdate = _context.Посещения
                .Where(p => p.Id_клиента == _clientId
                            && p.Дата_записи < today
                            && p.Статус == "Запланировано")
                .ToList();

            foreach (var visit in visitsToUpdate)
            {
                visit.Статус = "Посетил";
            }

            if (visitsToUpdate.Count > 0)
            {
                _context.SaveChanges();
            }
        }

        // Загрузка всех записей клиента
        private void LoadVisits()
        {
            var записи = _context.Посещения
                .Where(p => p.Id_клиента == _clientId)
                .OrderBy(p => p.Дата_записи)
                .ToList();

            VisitsListBox.ItemsSource = записи;
        }

        // Обработчик кнопки "Отменить запись"
        private void CancelVisit_Click(object sender, RoutedEventArgs e)
        {
            var selectedVisit = VisitsListBox.SelectedItem as Посещения;
            if (selectedVisit == null)
            {
                MessageBox.Show("Выберите запись для отмены.");
                return;
            }

            if (selectedVisit.Статус != "Запланировано")
            {
                MessageBox.Show("Отменить можно только запланированные записи.");
                return;
            }

            selectedVisit.Статус = "Отменено";
            _context.SaveChanges();
            LoadVisits();

            _currentClient.Id_клиента = CurrentClient.Id;
            _currentClient.Действие = "Отмена записи";
            _currentClient.Дата_и_время = DateTime.Now;
            Фитнес_ЗалEntities.GetContext().Журнал.Add(_currentClient);
            Фитнес_ЗалEntities.GetContext().SaveChanges();
        }

        // Переход на страницу записи
        private void GoToBookingPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.ClientRecord());
            //var bookingPage = new Pages.ClientRecord();
            // Модальное окно
            /*LoadVisits(); */// Обновим список после записи
        }
    }
}
