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

namespace WpfApp2.Windows
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        public Admin()
        {
            InitializeComponent();
            MainFrame.NavigationService.Navigate(new Pages.Adminpage());
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            Close();
            login.Show();
        }
        private void Open_Z_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.Registr());
        }
        private void profile_sotrudnik_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.Add_Sotrudnik());
        }
        private void add_Records_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.Add_Servise());
        }
        private void Servise_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.EditService());
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.Adminpage());
        }

        private void ClientEdit_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.EditClient());
        }

        private void EditSotrudnik_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.EditSotrudnik());
        }
    }
}
