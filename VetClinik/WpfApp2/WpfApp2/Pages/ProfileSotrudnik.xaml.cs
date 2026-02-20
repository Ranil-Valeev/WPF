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
using WpfApp2.Models;
using static WpfApp2.MainWindow;

namespace WpfApp2.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfileSotrudnik.xaml
    /// </summary>
    public partial class ProfileSotrudnik : Page
    {
        public ProfileSotrudnik()
        {
            InitializeComponent();
            LoadEmployeeData(CurrentUser.Id);
        }
        private void LoadEmployeeData(int employeeId)
        {
            using (var db = new VetClinikumEntities())
            {
                var employee = db.Сотрудник.FirstOrDefault(e => e.ID_user == employeeId);

                if (employee != null)
                {
                    
                    if (!string.IsNullOrEmpty(employee.Фото))
                    {
                        try
                        {
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(employee.Фото);
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                            employee.Фото = bitmap.ToString(); 
                        }
                        catch
                        {
                            employee.Фото = null;
                        }
                    }

                    DataContext = employee;
                }
            }
        }
    }
}
