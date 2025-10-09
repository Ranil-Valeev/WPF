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

namespace Справочник_Книг.User_Pages
{
    /// <summary>
    /// Логика взаимодействия для User_Window.xaml
    /// </summary>
    public partial class User_Window : Window
    {
        private void Group_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.NavigationService.GoBack();
        }
        public User_Window()
        {
            InitializeComponent();
        }
        public void User(int ID)
        {
            Client.Id = ID;
        }

        public static class Client
        {
            public static int Id { get; set; }
        }

        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Minimize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void EXIT_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
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
        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new User_Pages.Profile());
        }
        private void read_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UserBooks());
        }

        private void main_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new RecommendationsPage());
        }

        private void author_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Authors());
        }

        private void reviw_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UserReviews());
        }

        private void all_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new User_Pages.AllBooks());
        }
    }
}
