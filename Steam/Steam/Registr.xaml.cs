using Steam.Model;
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
using System.Windows.Shapes;
using System.Data.Entity;
using Microsoft.Win32;

namespace Steam
{
    /// <summary>
    /// Логика взаимодействия для Registr.xaml
    /// </summary>
    public partial class Registr : Window
    {

        private SteamEntities _context = new SteamEntities();
        private string _photoPath;
        public Registr()
        {
            InitializeComponent();

        }

        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void Minimize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void SelectPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                Title = "Выберите фото профиля"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _photoPath = openFileDialog.FileName;
                LoadSelectedImage();
            }
        }

        private void ProfileImage_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SelectPhotoButton_Click(sender, e);
        }

        private void LoadSelectedImage()
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_photoPath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                ProfileImage.Source = bitmap;
            }
            catch
            {
                MessageBox.Show("Не удалось загрузить изображение", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                LoadDefaultImage();
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Базовая валидация
                if (string.IsNullOrWhiteSpace(LoginTextBox.Text))
                {
                    MessageBox.Show("Введите логин", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(PasswordBox.Password))
                {
                    MessageBox.Show("Введите пароль", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Создание нового пользователя
                var newUser = new User
                {
                    Login = LoginTextBox.Text,
                    Password = PasswordBox.Password,
                    Адрес = EmailTextBox.Text,
                    Ник = NicknameTextBox.Text,
                    Фото = _photoPath // Сохраняем путь к фото
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                MessageBox.Show("Регистрация прошла успешно!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadDefaultImage()
        {
            ProfileImage.Source = new BitmapImage(new Uri("C:\\Users\\User\\Documents\\STUDY\\УП\\Steam\\Фото\\The Legend of Zelda 3.webp"));
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
