using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfApp2.Pages
{
    /// <summary>
    /// Логика взаимодействия для PetDetail.xaml
    /// </summary>
    public partial class PetDetail : Page
    {
        private VetClinikumEntities db = new VetClinikumEntities();
        private int _petId;

        public PetDetail(int petId)
        {
            InitializeComponent();
            _petId = petId;
            LoadPetData(_petId);
        }
        private void LoadPetData(int petId)
        {
            var pet = db.Питомцы.FirstOrDefault(p => p.ID_питомца == petId);
            if (pet == null)
            {
                MessageBox.Show("Питомец не найден.");
                return;
            }

            var owner = db.Клиенты.FirstOrDefault(c => c.ID_клиента == pet.ID_клиента);

            // Заполняем данные питомца
            PetName.Text = pet.Кличка;
            PetType.Text = GetPetTypeName(pet.ID_вида);
            PetBreed.Text = pet.Порода;
            PetGender.Text = pet.Пол;
            PetBirthDate.Text = pet.Дата_рождения?.ToString("dd.MM.yyyy");
            PetWeight.Text = pet.Вес + " кг";

            // Загружаем фото (если оно сохранено как путь к файлу или base64 строка)
            if (!string.IsNullOrEmpty(pet.Фото))
            {
                try
                {
                    // Если это путь к файлу
                    if (File.Exists(pet.Фото))
                    {
                        PetPhoto.Source = new BitmapImage(new Uri(pet.Фото, UriKind.Absolute));
                    }
                    else
                    {
                        // Если это base64 строка изображения
                        byte[] imageBytes = Convert.FromBase64String(pet.Фото);
                        using (var ms = new MemoryStream(imageBytes))
                        {
                            BitmapImage image = new BitmapImage();
                            image.BeginInit();
                            image.StreamSource = ms;
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.EndInit();
                            PetPhoto.Source = image;
                        }
                    }
                }
                catch
                {
                    // В случае ошибки показываем заглушку
                }
            }

            // Заполняем данные владельца
            if (owner != null)
            {
                OwnerFullName.Text = $"{owner.Фамилия} {owner.Имя} {owner.Отчество}";
                OwnerPhone.Text = owner.Номер_телефона;
                OwnerEmail.Text = owner.Электронная_почта;
                OwnerAddress.Text = $"г. {owner.Город}, ул. {owner.Улица}, д. {owner.Дом}";
                if (!string.IsNullOrEmpty(owner.Квартира))
                    OwnerAddress.Text += $", кв. {owner.Квартира}";
                OwnerBirthDate.Text = owner.Дата_рождения?.ToString("dd.MM.yyyy");
            }
        }

        private string GetPetTypeName(int idType)
        {
            // Если есть таблица "Виды" — здесь можно сделать запрос. Пока — хардкод:
            return idType == 1 ? "Кот" : "Животное";
        }
    }
}
