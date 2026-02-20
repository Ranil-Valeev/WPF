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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Steam.Client;

namespace Steam.Pages
{
    /// <summary>
    /// Логика взаимодействия для Collection.xaml
    /// </summary>
    public partial class Collection : Page
    {
        private SteamEntities _context = new SteamEntities();
        private int _userId;
        public Collection()
        {
            InitializeComponent();
            _userId = CurrentClient.Id;
            LoadCollection();
            LoadStatuses();
        }
        private void LoadCollection()
        {
            var collectionData = _context.Колекция
                .Where(c => c.ID_User == _userId)
                .ToList(); 

            var collection = collectionData.Select(c => new CollectionViewModel
            {
                Колекция = c,
                Игра = c.Игра,
                Фото = c.Игра.GetPhoto, 
                Название = c.Игра.Название,
                Жанры = c.Игра.Жанры,
                Статус = c.Статус_прохождения?.Статус,
                СтатусID = c.ID_Сатуса_прохождения
            }).ToList();

            CollectionDataGrid.ItemsSource = collection;
        }

        private void LoadStatuses()
        {
            StatusComboBox.ItemsSource = _context.Статус_прохождения.ToList();
            StatusComboBox.DisplayMemberPath = "Статус";
            StatusComboBox.SelectedValuePath = "ID_Статуса_прохождения";
        }

        private void CollectionDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CollectionDataGrid.SelectedItem is CollectionViewModel selected)
            {
                StatusComboBox.SelectedValue = selected.СтатусID;
            }
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CollectionDataGrid.SelectedItem is CollectionViewModel selected)
            {
                var item = selected.Колекция as Колекция;
                if (item != null && StatusComboBox.SelectedValue is int newStatusId)
                {
                    item.ID_Сатуса_прохождения = newStatusId;
                    _context.SaveChanges();
                    LoadCollection();
                }
            }
        }

        private void CollectionDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CollectionDataGrid.SelectedItem is CollectionViewModel selected)
            {
                var game = selected.Игра as Игра;
                if (game != null)
                    NavigationService.Navigate(new Pages.Gamedetal(game));
            }
        }

        private void AddGameButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.AddCollection(_userId));
        }

        private void LeaveReviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (CollectionDataGrid.SelectedItem is CollectionViewModel selected)
            {
                var game = selected.Игра as Игра;
                if (game != null)
                    NavigationService.Navigate(new Pages.Review(_userId, game.ID_Игры));
            }
        }
        public class CollectionViewModel
        {
            public Колекция Колекция { get; set; }
            public Игра Игра { get; set; }
            public string Фото { get; set; }
            public string Название { get; set; }
            public string Жанры { get; set; }
            public string Статус { get; set; }
            public int СтатусID { get; set; }
        }
    }
}
