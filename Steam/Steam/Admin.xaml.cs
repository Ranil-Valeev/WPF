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

namespace Steam
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        public Admin()
        {
            InitializeComponent();
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
            MainFrame.NavigationService.Navigate(new AdminPages.Add_game());
        }
        private void Record_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new AdminPages.Add_DLS());
        }
        private void EXIT_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private void Kontact_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new AdminPages.Add_company());
        }

        private void TOP_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.TOP());
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new AdminPages.Add_develop());
        }
    }
}
