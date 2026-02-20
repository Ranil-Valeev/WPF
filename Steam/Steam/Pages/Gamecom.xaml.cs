using Steam.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace Steam.Pages
{
    /// <summary>
    /// Логика взаимодействия для Gamecom.xaml
    /// </summary>
    public partial class Gamecom : Page
    {
        private int IDCom;
        public Gamecom(int ID)
        {
            IDCom = ID;
            InitializeComponent();
            Load();
        }
        
        public void Load()
        {
            using (var db = new SteamEntities())
            {
                var company = db.Издатель.Find(IDCom);
                if (company != null)
                {
                    NameCom.Text = company.Название;
                }

                var Servise = db.Игра.Where(s => s.ID_Издателя == IDCom).OrderBy(s => s.Название).ToList();

                ServicesGrid.ItemsSource = Servise;
            }
        } 
    }
}
