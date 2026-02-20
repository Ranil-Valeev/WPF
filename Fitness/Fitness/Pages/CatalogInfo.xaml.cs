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
    /// Логика взаимодействия для CatalogInfo.xaml
    /// </summary>
    public partial class CatalogInfo : Page
    {
        private Тренировки _catalog;
        public CatalogInfo(Тренировки catalog)
        {
            InitializeComponent();
            _catalog = catalog;
            DataContext = _catalog;
        }
    }
}
