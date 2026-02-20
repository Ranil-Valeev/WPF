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
    /// Логика взаимодействия для EditRecordPage.xaml
    /// </summary>
    public partial class EditRecordPage : Page
    {
        private int _recordId;
        private Проведенные_услуги _currentRecord;

        public EditRecordPage(int recordId)
        {
            InitializeComponent();
            _recordId = recordId;
            LoadRecordData();
            LoadStatuses();
        }

        private void LoadRecordData()
        {
            try
            {
                _currentRecord = РКИСEntities.GetContext()
                    .Проведенные_услуги
                    .FirstOrDefault(r => r.ID_Проведенных_услуг == _recordId);

                if (_currentRecord != null)
                {
                    DataContext = _currentRecord;
                    StatusComboBox.SelectedItem = _currentRecord.Статус;
                    CommentBox.Text = _currentRecord.Коментарий_Сотрудника;
                }
                else
                {
                    MessageBox.Show("Запись не найдена.");
                    NavigationService?.GoBack();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void LoadStatuses()
        {
            // Простой список статусов, можно заменить на справочник из БД при необходимости
            var statuses = new List<string> { "Ожидание", "Выполнено", "Отменено" };
            StatusComboBox.ItemsSource = statuses;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentRecord != null)
                {
                    _currentRecord.Статус = StatusComboBox.SelectedItem?.ToString();
                    _currentRecord.Коментарий_Сотрудника = CommentBox.Text;

                    РКИСEntities.GetContext().SaveChanges();

                    MessageBox.Show("Изменения сохранены.");
                    NavigationService?.GoBack();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}
