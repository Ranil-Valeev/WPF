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
using Тур_агенство.model;

namespace Тур_агенство.User_Pages
{
    /// <summary>
    /// Логика взаимодействия для РекомендацииPage.xaml
    /// </summary>
    public partial class РекомендацииPage : Page
    {
        private int _userId;
        private ТурАгентствоEntities _context = new ТурАгентствоEntities();

        public РекомендацииPage(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadRecommendations();
        }

        private void LoadRecommendations()
        {
            var recommendations = _context.Рекомендации
                .Where(r => r.КлиентId == _userId) // или ПользовательId, если поле называется иначе
                .ToList();

            RecommendationsWrapPanel.Children.Clear();

            if (recommendations.Count == 0)
            {
                TextBlock noRec = new TextBlock
                {
                    Text = "Рекомендации пока отсутствуют.",
                    FontSize = 18,
                    Foreground = Brushes.Gray,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(10)
                };
                RecommendationsWrapPanel.Children.Add(noRec);
                return;
            }

            foreach (var r in recommendations)
            {
                Border card = new Border
                {
                    Width = 260,
                    Height = 360,
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

                var tour = _context.Туры.FirstOrDefault(t => t.Id == r.ТурId);
                if (tour != null)
                {
                    string path = Directory.GetCurrentDirectory() + @"\Images\" + tour.Фото?.Trim();
                    if (!string.IsNullOrEmpty(tour.Фото) && File.Exists(path))
                    {
                        Image img = new Image
                        {
                            Source = new BitmapImage(new Uri(path)),
                            Height = 180,
                            Stretch = Stretch.UniformToFill,
                            Margin = new Thickness(0, 0, 0, 10)
                        };
                        stack.Children.Add(img);
                    }

                    TextBlock name = new TextBlock
                    {
                        Text = tour.Название,
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        Foreground = Brushes.Black,
                        TextAlignment = TextAlignment.Center,
                        Margin = new Thickness(5)
                    };
                    stack.Children.Add(name);
                }

                TextBlock comment = new TextBlock
                {
                    Text = r.Причина ?? "Без комментария",
                    FontSize = 14,
                    Foreground = Brushes.Gray,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(10)
                };
                stack.Children.Add(comment);

                card.Child = stack;
                RecommendationsWrapPanel.Children.Add(card);
            }
        }
    }
}
