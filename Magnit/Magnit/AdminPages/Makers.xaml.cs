using Magnit.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для Makers.xaml
    /// </summary>
    public partial class Makers : Page
    {
        private MagnitEntities _context = new MagnitEntities();
        private Производитель _currentManufacturer;
        public Makers()
        {
            InitializeComponent();
            LoadData();
            SetNewManufacturerMode();
        }
        private void LoadData()
        {
            // Загрузка производителей
            _context.Производитель.Load();
            cbManufacturers.ItemsSource = _context.Производитель.Local;
        }

        private void SetNewManufacturerMode()
        {
            _currentManufacturer = new Производитель();
            cbManufacturers.SelectedIndex = -1;
            txtName.Text = "";
            txtAddress.Text = "";
        }

        private void cbManufacturers_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (cbManufacturers.SelectedItem is Производитель selectedManufacturer)
            {
                _currentManufacturer = selectedManufacturer;
                txtName.Text = _currentManufacturer.Название;
                txtAddress.Text = _currentManufacturer.Адрес;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название производителя");
                return;
            }

            // Обновляем данные производителя
            _currentManufacturer.Название = txtName.Text;
            _currentManufacturer.Адрес = txtAddress.Text;

            // Если это новый производитель - добавляем в контекст
            if (_currentManufacturer.ID_производителя == 0)
            {
                _context.Производитель.Add(_currentManufacturer);
            }

            try
            {
                _context.SaveChanges();
                MessageBox.Show("Данные сохранены успешно");

                // Обновляем список производителей
                _context = new MagnitEntities();
                LoadData();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            SetNewManufacturerMode();
        }
    }
}
