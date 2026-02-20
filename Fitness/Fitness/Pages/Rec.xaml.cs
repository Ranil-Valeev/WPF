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
    /// Логика взаимодействия для Rec.xaml
    /// </summary>
    public partial class Rec : Page
    {
        private readonly RecommendationService _service = new RecommendationService();
        public Rec()
        {
            InitializeComponent();
            LoadRecommendations(CurrentClient.Id);
        }
        private void LoadRecommendations(int clientId)
        {
            var recommendations = _service.GetClientRecommendations(clientId);
            RecommendationsListBox.ItemsSource = recommendations;
        }
    }
}
