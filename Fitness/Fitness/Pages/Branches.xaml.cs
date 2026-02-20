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

namespace Fitness.Pages
{
    /// <summary>
    /// Логика взаимодействия для Branches.xaml
    /// </summary>
    public partial class Branches : Page
    {
        private Фитнес_ЗалEntities _context = new Фитнес_ЗалEntities();
        public Branches()
        {
            InitializeComponent();
            LoadBranches();
        }
        private void LoadBranches()
        {
            var branches = _context.Филиалы.ToList();
            BranchListBox.ItemsSource = branches;
        }

        private void BranchListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedBranch = BranchListBox.SelectedItem as Филиалы;
            if (selectedBranch != null)
            {
                AddressTextBlock.Text = "Адрес: " + selectedBranch.Адрес;
                PhoneTextBlock.Text = "Телефон: " + selectedBranch.Контактный_телефон;
            }
        }
    }
}
