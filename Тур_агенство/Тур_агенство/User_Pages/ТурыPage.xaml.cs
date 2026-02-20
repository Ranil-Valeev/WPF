using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
using Тур_агенство.model;

namespace Тур_агенство.User_Pages
{
    /// <summary>
    /// Логика взаимодействия для ТурыPage.xaml
    /// </summary>
    public partial class ТурыPage : Page
    {
        private int _userId;
        private ТурАгентствоEntities _context = new ТурАгентствоEntities();

        public ТурыPage(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadTours();
        }

        private void LoadTours()
        {
            var tours = _context.Туры.ToList();
            ToursWrapPanel.Children.Clear();

            foreach (var t in tours)
            {
                Border card = new Border
                {
                    Width = 250,
                    Height = 380,
                    Margin = new Thickness(10),
                    Background = Brushes.White,
                    CornerRadius = new CornerRadius(15),
                    Effect = new System.Windows.Media.Effects.DropShadowEffect
                    {
                        BlurRadius = 5,
                        ShadowDepth = 2,
                        Opacity = 0.3
                    }
                };

                StackPanel stack = new StackPanel();

                // Фото
                string path = Directory.GetCurrentDirectory() + @"\Images\" + t.Фото?.Trim();
                if (!string.IsNullOrEmpty(t.Фото) && File.Exists(path))
                {
                    Image img = new Image
                    {
                        Source = new BitmapImage(new Uri(path)),
                        Height = 180,
                        Stretch = Stretch.UniformToFill,
                        Margin = new Thickness(0, 0, 0, 10),
                        ClipToBounds = true
                    };
                    stack.Children.Add(img);
                }

                // Название
                TextBlock name = new TextBlock
                {
                    Text = t.Название,
                    FontSize = 18,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Black,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(5, 5, 5, 5)
                };
                stack.Children.Add(name);

                // Описание
                TextBlock desc = new TextBlock
                {
                    Text = t.Описание,
                    FontSize = 14,
                    Foreground = Brushes.Gray,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(5, 0, 5, 10)
                };
                stack.Children.Add(desc);

                // Цена
                TextBlock price = new TextBlock
                {
                    Text = $"Цена: {t.Цена:C}",
                    FontSize = 16,
                    Foreground = Brushes.DarkGreen,
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 10)
                };
                stack.Children.Add(price);

                // Кнопка бронирования
                Button bookBtn = new Button
                {
                    Content = "Забронировать",
                    Height = 35,
                    Background = new SolidColorBrush(Color.FromRgb(44, 62, 80)),
                    Foreground = Brushes.White,
                    BorderThickness = new Thickness(0),
                    Cursor = System.Windows.Input.Cursors.Hand,
                    Margin = new Thickness(30, 0, 30, 10),
                    Tag = t.Id
                };
                bookBtn.Click += BookBtn_Click;
                stack.Children.Add(bookBtn);

                card.Child = stack;
                ToursWrapPanel.Children.Add(card);
            }
        }

        private void BookBtn_Click(object sender, RoutedEventArgs e)
        {
            int tourId = (int)(sender as Button).Tag;

            try
            {
                var newBooking = new Бронирования
                {
                    КлиентId = _userId, // поменяй, если у тебя другое поле (например, ПользовательId)
                    ТурId = tourId,
                    ДатаБронирования = DateTime.Now
                };

                _context.Бронирования.Add(newBooking);
                _context.SaveChanges();

                MessageBox.Show("Тур успешно забронирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при бронировании: " + ex.Message);
            }
        }
    }
}
