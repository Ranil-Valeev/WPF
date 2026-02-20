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
using static WpfApp2.Windows.Login;

namespace WpfApp2.Pages
{
    /// <summary>
    /// Логика взаимодействия для Records.xaml
    /// </summary>
    public partial class Records : Page
    {
        private Проведенные_услуги _currentService = new Проведенные_услуги();
        public Records()
        {
            InitializeComponent();

            DataContext = _currentService;
            Tema.ItemsSource = РКИСEntities.GetContext().Услуги.ToList();
            sotrudnik.ItemsSource = РКИСEntities.GetContext().Сотрудник.ToList();
        }
        private StringBuilder CheckFields()
        {

            StringBuilder s = new StringBuilder();
            if (CurrentUser.Id >= 0)
                _currentService.ID_Клиента = CurrentUser.Id;
            //if (sotrudnik.SelectedIndex <= 0)  
            //    s.AppendLine("Ошибка2");
            //if (Tema.SelectedIndex <= 0)
            //    s.AppendLine("Ошибка1");

            if (_currentService.Начало_оказания_услуги == null)
                s.AppendLine("Поле название пустое");
            return s;
        }
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder _error = CheckFields();
            if (TimeBox.SelectedItem != null && TimeSpan.TryParse(TimeBox.SelectedItem.ToString(), out TimeSpan selectedTime))
            {
                _currentService.Начало_оказания_услуги = _currentService.Начало_оказания_услуги.Date.Add(selectedTime);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите доступное время для записи.");
                return;
            }
            if (_error.Length > 0)
            {
                MessageBox.Show(_error.ToString());
                return;
            }
            // проверка полей прошла успешно
            if (_currentService.ID_Проведенных_услуг == 0)
            {
                РКИСEntities.GetContext().Проведенные_услуги.Add(_currentService);
            }
            try
            {         
                РКИСEntities.GetContext().SaveChanges();
                MessageBox.Show("Запись Изменена");    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void Tema_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedService = Tema.SelectedItem as Услуги;
            if (selectedService != null)
            {
                // Получаем ID категории выбранной услуги
                int categoryId = selectedService.ID_Категории;
                // Фильтруем сотрудников по этой категории
                var filteredEmployees = РКИСEntities.GetContext().Сотрудник
                    .Where(s => s.ID_Категории == categoryId)
                    .ToList();

                sotrudnik.ItemsSource = filteredEmployees;
                UpdateAvailableTimes();
            }
        }
        private void UpdateAvailableTimes()
        {
            TimeBox.Items.Clear();
            NoTimeText.Visibility = Visibility.Collapsed;
            TimeBox.IsEnabled = true;
            var selectedService = Tema.SelectedItem as Услуги;
            var selectedDate = _currentService.Начало_оказания_услуги.Date;
            var selectedEmployee = sotrudnik.SelectedItem as Сотрудник;

            if (selectedService == null || selectedDate == DateTime.MinValue || selectedEmployee == null)
                return;

            if (!int.TryParse(new string(selectedService.Продолжительность.Where(char.IsDigit).ToArray()), out int durationMinutes))
            {
                MessageBox.Show("Не удалось распознать продолжительность услуги.");
                return;
            }
            TimeSpan serviceDuration = TimeSpan.FromMinutes(durationMinutes);
            TimeSpan workStart = new TimeSpan(9, 0, 0);
            TimeSpan workEnd = new TimeSpan(16, 0, 0);

            DateTime dayStart = selectedDate.Date;
            DateTime dayEnd = dayStart.AddDays(1);

            // Сначала получаем все записи за день
            var allRecords = РКИСEntities.GetContext().Проведенные_услуги
                .Where(r => r.ID_Сотрудника == selectedEmployee.ID_Сотрудника &&
                            r.Начало_оказания_услуги >= dayStart &&
                            r.Начало_оказания_услуги < dayEnd)
                .ToList();

            // Затем получаем все услуги (в памяти)
            var allServices = РКИСEntities.GetContext().Услуги.ToList();

            List<TimeSpan> availableTimes = new List<TimeSpan>();

            for (TimeSpan time = workStart; time + serviceDuration <= workEnd; time = time.Add(TimeSpan.FromMinutes(30)))
            {
                DateTime proposedStart = selectedDate.Add(time);
                DateTime proposedEnd = proposedStart.Add(serviceDuration);

                bool overlap = allRecords.Any(r =>
                {
                    var service = allServices.FirstOrDefault(s => s.ID_Услуги == r.ID_Услуги);
                    if (service == null || !int.TryParse(new string(service.Продолжительность.Where(char.IsDigit).ToArray()), out int existingMinutes))
                        return false;

                    DateTime existingStart = r.Начало_оказания_услуги;
                    DateTime existingEnd = existingStart.AddMinutes(existingMinutes);

                    return proposedStart < existingEnd && proposedEnd > existingStart;
                });

                if (!overlap)
                {
                    availableTimes.Add(time);
                }
            }
            if (availableTimes.Count == 0)
            {
                TimeBox.IsEnabled = false;
                NoTimeText.Visibility = Visibility.Visible;
                return;
            }
            foreach (var time in availableTimes)
            {
                TimeBox.Items.Add(time.ToString(@"hh\:mm"));
            }

            TimeBox.SelectedIndex = 0;
        }
        private void sotrudnik_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAvailableTimes();
        }
        private void TimeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
        private void DatePickerControl_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DatePickerControl.SelectedDate.HasValue)
            {
                _currentService.Начало_оказания_услуги = DatePickerControl.SelectedDate.Value;
                UpdateAvailableTimes();
            }
        }
    }
}

