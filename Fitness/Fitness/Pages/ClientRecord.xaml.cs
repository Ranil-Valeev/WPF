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
    /// Логика взаимодействия для ClientRecord.xaml
    /// </summary>
    public partial class ClientRecord : Page
    {
        private Журнал _currentClient = new Журнал();
        private readonly Фитнес_ЗалEntities _context = new Фитнес_ЗалEntities();
        private readonly int _clientId;
        public ClientRecord()
        {
            InitializeComponent();
            _clientId = CurrentClient.Id;
            LoadTrainings();
        }
        private void LoadTrainings()
        {
            TrainingComboBox.ItemsSource = _context.Тренировки.ToList();
        }

        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTraining = TrainingComboBox.SelectedItem as Тренировки;
            var selectedDate = DatePicker.SelectedDate;

            if (selectedTraining == null)
            {
                MessageBox.Show("Пожалуйста, выберите тренировку.");
                return;
            }

            if (selectedDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите дату.");
                return;
            }

            var запись = new Посещения
            {
                Id_клиента = _clientId,
                Id_тренировки = selectedTraining.Id_тренеровки,
                Дата_записи = selectedDate.Value,
                Статус = "Запланировано"
            };

            _context.Посещения.Add(запись);
            _context.SaveChanges();

            _currentClient.Id_клиента = CurrentClient.Id;
            _currentClient.Действие = "Запись на тренировку";
            _currentClient.Дата_и_время = DateTime.Now;
            Фитнес_ЗалEntities.GetContext().Журнал.Add(_currentClient);
            Фитнес_ЗалEntities.GetContext().SaveChanges();

            StatusTextBlock.Text = "Вы успешно записались на тренировку!";
        }
    }
}
