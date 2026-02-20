using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
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
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp2.Pages
{
    /// <summary>
    /// Логика взаимодействия для Add_Servise.xaml
    /// </summary>
    public partial class Add_Servise : Page
    {
        private Услуги _currentservise = new Услуги();
        public Add_Servise()
        {
            InitializeComponent();
            servis.ItemsSource = РКИСEntities.GetContext().Категории.ToList();
        }
        private StringBuilder ValidateFields()
        {
            StringBuilder s = new StringBuilder();
            if (ServiceName.Text == null)
                s.AppendLine("Ошибка1");
            if (Duration.Text == null)
                s.AppendLine("Ошибка2");
            if (Price == null)
                s.AppendLine("Ошибка3");
            if (Discount == null)
                s.AppendLine("Ошибка4");
            return s;
        }
        private string imagePath;
        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                imagePath = openFileDialog.FileName;
                ServiceImage.Source = new BitmapImage(new Uri(imagePath));
            }
        }
        private void SaveService_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder s = ValidateFields();
            if (s.Length > 0)
            {
                
                MessageBox.Show(s.ToString());
                return;
            }
            try
            {
                int Discount1 = int.Parse(Discount.Text);
                float Discount2 = Convert.ToSingle(Discount1) / 100;
                _currentservise.Наименование_услуги = ServiceName.Text;
                _currentservise.Продолжительность = Duration.Text;
                _currentservise.Стоимость = int.Parse(Price.Text);
                _currentservise.Скидка = Discount2;
                int imageId = 0;
                _currentservise.ID_Категории = servis.SelectedIndex;
                if (!string.IsNullOrEmpty(imagePath))
                {
                    var newImage = new Изображения
                    {
                        Изображение = imagePath
                    };
                    РКИСEntities.GetContext().Изображения.Add(newImage);
                    РКИСEntities.GetContext().SaveChanges(); 
                    imageId = newImage.ID_Изображение;
                }
                if (imageId == 0)
                {
                    MessageBox.Show("ERORR IMAGE");
                    
                } else
                {
                    _currentservise.ID_Изабражения = imageId;
                }
                // Добавляем сотрудника в БД
                РКИСEntities.GetContext().Услуги.Add(_currentservise);
                РКИСEntities.GetContext().SaveChanges();

                MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService?.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
