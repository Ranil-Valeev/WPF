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
using System.Windows.Shapes;
using WpfApp2.Models;
using static WpfApp2.Windows.Client;

namespace WpfApp2.Windows
{
    /// <summary>
    /// Логика взаимодействия для Sotrudnik.xaml
    /// </summary>
    public partial class Sotrudnik : Window
    {
        public int _currentUserId;
        public Sotrudnik()
        {
            InitializeComponent();
        }
        public void SetUser(int currentID)
        {
            _currentUserId = currentID;
            MessageBox.Show(currentID.ToString());
            CurrentSotrudnik.Id = GetClientIdByUserId(_currentUserId);
            MessageBox.Show(CurrentSotrudnik.Id.ToString());

        }
        private void Group_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Content = null;
        }
        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        private void Minimize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Profile_Click(object sender, RoutedEventArgs e)
        {
        MainFrame.NavigationService.Navigate(new Pages.ProfileSotrudnik());
        }
        private void Service_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.ServiseSotrudnik());
        }
        private void Record_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.RecordSotrudnik());
        }
        private void EXIT_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            if (MainFrame.IsEnabled == true)
            {
                ST_Dobro.Visibility = Visibility.Collapsed;
            }
            else
            {
                ST_Dobro.Visibility = Visibility.Visible;
            }
        }
        public static class CurrentSotrudnik
        {
            public static int Id { get; set; }


        }
        public int GetClientIdByUserId(int userId)
        {
            using (var db = new VetClinikumEntities())
            {
                var client = db.Сотрудник
                              .FirstOrDefault(c => c.ID_user == userId);

                return client?.ID_сотрудника ?? 0; 
            }
        } 
    }
}
