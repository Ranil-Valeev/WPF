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

namespace Справочник_Книг.Admin_pages
{
    /// <summary>
    /// Логика взаимодействия для Admin_window.xaml
    /// </summary>
    public partial class Admin_window : Window
    {
        public Admin_window()
        {
            InitializeComponent();
        }

        private void book_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Admin_pages.BooksPage());
        }

        private void author_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Admin_pages.AddAutor());
        }

        private void EXIT_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
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
        private void Minimize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Group_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.NavigationService.GoBack();
        }

        private void genre_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Admin_pages.AddGenre());
        }
    }
}
