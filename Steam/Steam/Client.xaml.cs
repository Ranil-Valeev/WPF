using Steam.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace Steam
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        private static SteamEntities _context;
        public int _currentUserId;
        public Client()
        {
            InitializeComponent();
        }
        public void SetUser(int currentID)
        {       
            CurrentClient.Id = currentID;
            MessageBox.Show(CurrentClient.Id.ToString());

        }
        public static class CurrentClient
        {
            public static int Id { get; set; }

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

        private void Service_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.Game());
        }
        private void Record_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.Collection());
        }
        private void EXIT_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private void Kontact_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.Company());
        }

        private void TOP_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.TOP());
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.Profile());
        }

        private void Recom_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.Recom()); 
        }
    }
}
