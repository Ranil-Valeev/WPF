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
using Учебное_Заведение.model;

namespace Учебное_Заведение.Teacher_pages
{
    /// <summary>
    /// Логика взаимодействия для Teacher.xaml
    /// </summary>
    public partial class Teacher : Window
    {
        private Пользователи _currentUser;
        private Преподаватели _currentTeacher;
        public Teacher(Пользователи user)
        {
            InitializeComponent();
            _currentUser = user;
            var context = УчебноеЗаведениеEntities.Getcontext();
            _currentTeacher = context.Преподаватели.FirstOrDefault(s => s.ID_Пользователя == _currentUser.ID_Пользователя);

            if (_currentTeacher == null)
            {
                MessageBox.Show("Ошибка: Преподоватлеь не найден для данного пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }
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
        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Profile(_currentTeacher));
        }

        private void lesson_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new lesson(_currentTeacher));
        }

        private void schedule_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new schedule(_currentTeacher));
        }
    }
}
