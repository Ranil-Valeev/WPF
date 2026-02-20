using Fitness.Model;
using System;
using System.Windows;
using System.Windows.Input;

namespace Fitness
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        private Журнал _currentClient = new Журнал();
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
            MainFrame.NavigationService.GoBack();  // Возвращает назад
            //MainFrame.Content = null; // просто закрывает страницу 
        }
        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _currentClient.Id_клиента = CurrentClient.Id;
            _currentClient.Действие = "Выход из системы";
            _currentClient.Дата_и_время = DateTime.Now;
            Фитнес_ЗалEntities.GetContext().Журнал.Add(_currentClient);
            Фитнес_ЗалEntities.GetContext().SaveChanges();
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

        private void EXIT_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();

            _currentClient.Id_клиента = CurrentClient.Id;
            _currentClient.Действие = "Выход из системы";
            _currentClient.Дата_и_время = DateTime.Now;
            Фитнес_ЗалEntities.GetContext().Журнал.Add(_currentClient);
            Фитнес_ЗалEntities.GetContext().SaveChanges();
            mainWindow.Show();
            this.Close();
        }


        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.Profile());
        }

        private void Catalog_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.Catalog());
        }

        private void Rec_Click(object sender, RoutedEventArgs e)
        {
            _currentClient.Id_клиента = CurrentClient.Id;
            _currentClient.Действие = "Просмотр рекомендаций";
            _currentClient.Дата_и_время = DateTime.Now;
            Фитнес_ЗалEntities.GetContext().Журнал.Add(_currentClient);
            Фитнес_ЗалEntities.GetContext().SaveChanges();
            MainFrame.NavigationService.Navigate(new Pages.Rec());
        }

        private void Records_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.Records());
        }

        private void Branches_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.Branches());
        }
    }

}
