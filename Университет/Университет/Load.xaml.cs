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
using System.Windows.Shapes;
using Университет.model;

namespace Университет
{
    /// <summary>
    /// Логика взаимодействия для Load.xaml
    /// </summary>
    public partial class Load : Window
    {
        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;
        public Load()
        {
            InitializeComponent();
            LVLoad.ItemsSource = УниверситетEntities.GetContext()
                .Проведение_предмета
                .OrderBy(p => p.ID_нагрузки)
                .ToList();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Teachers teachers = new Teachers();
            teachers.Show();
            this.Close();
        }
        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked == _lastHeaderClicked)
                {
                    if (_lastDirection == ListSortDirection.Ascending)
                        direction = ListSortDirection.Descending;
                    else
                        direction = ListSortDirection.Ascending;
                }
                else
                {
                    direction = ListSortDirection.Ascending;
                }

                string sortBy = headerClicked.Content.ToString();

                switch (sortBy)
                {
                    case "Фамилия":
                        sortBy = "Преподы.Фамилия";
                        break;
                    case "Предметы":
                        sortBy = "Предмет.Название_предмета";
                        break;
                    case "Объем часов":
                        sortBy = "Предмет.Часы";
                        break;
                }
                LVLoad.Items.SortDescriptions.Clear();
                LVLoad.Items.SortDescriptions.Add(new SortDescription(sortBy, direction));
                LVLoad.Items.Refresh();
                _lastHeaderClicked = headerClicked;
                _lastDirection = direction;
            }
        }
    }
}