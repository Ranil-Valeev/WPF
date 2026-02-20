using Microsoft.Win32;
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
using WpfApp2.Windows;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp2.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditService.xaml
    /// </summary>
    public partial class EditService : Page
    {
        private Услуги selectedService;
        private Услуги _currentService;
        public EditService()
        {
            InitializeComponent();
            DataContext = _currentService;
            Service.ItemsSource = РКИСEntities.GetContext().Услуги.ToList();
            Service.SelectedItem = _currentService;
            servis.ItemsSource = РКИСEntities.GetContext().Категории.ToList();
        }

        private void Service_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedService = Service.SelectedItem as Услуги;
            if (selectedService == null)
                return;

            // Подставляем данные в поля
            ServiceName.Text = selectedService.Наименование_услуги;
            Duration.Text = selectedService.Продолжительность;
            Price.Text = selectedService.Стоимость.ToString();
            Discount.Text = selectedService.Procent1.ToString();
            servis.SelectedIndex = selectedService.ID_Категории;

            if (selectedService.ID_Изабражения != null)
            {
                var image = РКИСEntities.GetContext().Изображения
                    .FirstOrDefault(i => i.ID_Изображение == selectedService.ID_Изабражения);
                if (image != null)
                {
                    imagePath = image.Изображение;
                    ServiceImage.Source = new BitmapImage(new Uri(imagePath));
                }
                else
                {
                    ServiceImage.Source = null;
                }
            }
            else
            {
                ServiceImage.Source = null;
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (selectedService == null)
            {
                MessageBox.Show("Пожалуйста, выберите услугу для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                int Discount1 = int.Parse(Discount.Text);
                float Discount2 = Convert.ToSingle(Discount1) / 100;
                selectedService.Наименование_услуги = ServiceName.Text;
                selectedService.Продолжительность = Duration.Text;
                selectedService.Стоимость = int.Parse(Price.Text);
                selectedService.Скидка = Discount2;
                selectedService.ID_Категории = servis.SelectedIndex;
                if (!string.IsNullOrEmpty(imagePath))
                {
                    var newImage = new Изображения
                    {
                        Изображение = imagePath
                    };
                    РКИСEntities.GetContext().Изображения.Add(newImage);
                    РКИСEntities.GetContext().SaveChanges();
                    selectedService.ID_Изабражения = newImage.ID_Изображение;
                }

                РКИСEntities.GetContext().SaveChanges();

                MessageBox.Show("Услуга успешно обновлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService?.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private string imagePath; 
        private void Image_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                imagePath = openFileDialog.FileName;
                ServiceImage.Source = new BitmapImage(new Uri(imagePath));
            }
        }
    }
}
