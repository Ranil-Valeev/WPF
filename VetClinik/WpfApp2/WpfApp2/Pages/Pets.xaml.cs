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
using static WpfApp2.Windows.Client;

namespace WpfApp2.Pages
{
    /// <summary>
    /// Логика взаимодействия для Pets.xaml
    /// </summary>
    public partial class Pets : Page
    {
        VetClinikumEntities db = new VetClinikumEntities();
        private string selectedPhotoPath = null;
        public Pets()
        {
            InitializeComponent();
            LoadSpecies();
        }
        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Изображения (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                selectedPhotoPath = openFileDialog.FileName;
                PetPhotoImage.Source = new BitmapImage(new Uri(selectedPhotoPath));
            }
        }
        private void LoadSpecies()
        {
            var speciesList = db.Категория_питомца.ToList();
            SpeciesComboBox.ItemsSource = speciesList;
        }
        private void AddPetButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                    SpeciesComboBox.SelectedItem == null ||
                    string.IsNullOrWhiteSpace(WeightTextBox.Text))
                {
                    MessageBox.Show("Заполните обязательные поля: Кличка, Вид и Вес", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Получаем выбранный ID вида питомца
                int selectedSpeciesId = (int)SpeciesComboBox.SelectedValue;

                var newPet = new Питомцы
                {
                    Кличка = NameTextBox.Text,
                    ID_вида = selectedSpeciesId,
                    Порода = BreedTextBox.Text,
                    Пол = GenderComboBox.Text,
                    Дата_рождения = BirthDatePicker.SelectedDate,
                    Вес = WeightTextBox.Text,
                    Рост = HeightTextBox.Text,
                    Группа_крови = BloodGroupTextBox.Text,
                    Возраст = AgeTextBox.Text,
                    Аллергии = AllergiesTextBox.Text,
                    Заболевания_хронический = ChronicDiseasesTextBox.Text,
                    Фото = selectedPhotoPath,
                    ID_клиента = CurrentClient.Id
                };

                db.Питомцы.Add(newPet);
                db.SaveChanges();

                MessageBox.Show("Питомец успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении питомца:\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
