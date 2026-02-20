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

namespace Учебное_Заведение.Admin_pages
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
        private void EXIT_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Minimize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Group_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.NavigationService.GoBack();
        }

        private void Students_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Students());
        }

        private void Groups_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Groups());
        }

        private void Teacher_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Teachers());
        }

        private void Subjects_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Subjects());
        }

        private void Schedule_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Schedule());
        }

        private void Stats_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Stats());
        }
    }
}
