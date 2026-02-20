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
using static WpfApp2.Windows.Login;
using WpfApp2.Models;

namespace WpfApp2.Pages
{
    /// <summary>
    /// Логика взаимодействия для Pofile.xaml
    /// </summary>
    public partial class Pofile : Page
    {
        public Pofile()
        {
            InitializeComponent();
            LoadProfile();
        }
        private void LoadProfile()
        {
            using (var db = new РКИСEntities())
            {
                var user = db.Клиент.Find(CurrentUser.Id);
                if (user != null)
                {
                    DataContext = user;
                }
            }
        }

    }
}
