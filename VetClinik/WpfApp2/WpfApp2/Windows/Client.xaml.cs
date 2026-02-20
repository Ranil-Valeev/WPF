using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
using WpfApp2.Pages;

namespace WpfApp2.Windows
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        public int _currentUserId;
        public Client()
        {
            InitializeComponent();
            
        }
        public void SetUser(int currentID)
        {
            _currentUserId = currentID;
            MessageBox.Show(currentID.ToString());
            CurrentClient.Id = GetClientIdByUserId(_currentUserId);
            MessageBox.Show(CurrentClient.Id.ToString());
        }
        public int GetClientIdByUserId(int userId)
        {
            using (var db = new VetClinikumEntities())
            {
                var client = db.Клиенты
                              .FirstOrDefault(c => c.ID_user == userId);

                return client?.ID_клиента ?? 0; // Возвращает 0, если клиент не найден
            }
        }
        private void Group_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.NavigationService.GoBack(); // Возвращает назад
            //MainFrame.Content = null; // просто закрывает страницу 
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
            using (var db = new VetClinikumEntities())
            {
                var clientProfile = db.Клиенты.FirstOrDefault(c => c.ID_user == _currentUserId);
                if (clientProfile == null)
                {             
                    MainFrame.NavigationService.Navigate(new Pages.Profile()); 
                }
                else
                {
                    MainFrame.NavigationService.Navigate(new Pages.ProfileOn()); 
                }
                ST_Dobro.Visibility = Visibility.Collapsed; 
                MainFrame.IsEnabled = true; 
            }
        }

        private void Service_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.ServiseClient());
        }
        private void Record_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.RecordClient());
        }
        private void EXIT_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private void Kontact_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.Kontact());
        }
        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            if(MainFrame.IsEnabled == true)
            {
                ST_Dobro.Visibility = Visibility.Collapsed;
            }
            else
            {
                ST_Dobro.Visibility = Visibility.Visible;
            }
        }
        public static class CurrentClient
        {
            public static int Id { get; set; }


        }
    }
}
