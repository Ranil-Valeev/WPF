using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для Adminpage.xaml
    /// </summary>
    public partial class Adminpage : Page
    {
        public Adminpage()
        {
            InitializeComponent();
            Listservice.ItemsSource = РКИСEntities.GetContext().Услуги.OrderBy(p => p.ID_Услуги).ToList();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(Listservice.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("Стоимость", ListSortDirection.Ascending));
            view.Refresh();
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(Listservice.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("Стоимость", ListSortDirection.Descending));
            view.Refresh();
        }
    }
}
