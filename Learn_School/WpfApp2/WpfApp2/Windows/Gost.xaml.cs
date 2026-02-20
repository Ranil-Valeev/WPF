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
    /// Логика взаимодействия для Gost.xaml
    /// </summary>
    public partial class Gost : Window
    {
        public Gost()
        {
            InitializeComponent();
            MainFrame.NavigationService.Navigate(new Pages.ClientService());
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Login login = new Login();
            login.Show();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult x = MessageBox.Show("Вы действительно хотите закрыть", "Выйти", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (x == MessageBoxResult.Cancel)
                e.Cancel = true;
        }
        private void open_Z_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вы должны обратьтся к сотруднику чтобы вас зарегистрировали");
        }
    }
}
