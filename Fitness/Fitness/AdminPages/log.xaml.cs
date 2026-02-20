using Fitness.Model;
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

namespace Fitness.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для log.xaml
    /// </summary>
    public partial class log : Page
    {
        private Фитнес_ЗалEntities db = new Фитнес_ЗалEntities();

        public log()
        {
            InitializeComponent();
            LoadClients();
        }
        private void LoadClients()
        {
            var clients = db.Клиенты
                .Select(c => new {
                    c.Id_клиента,
                    FullName = c.Логин + " " + c.Email
                })
                .ToList();

            ClientComboBox.ItemsSource = clients;
        }

        private void ClientComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientComboBox.SelectedValue is int clientId)
            {
                var journal = db.Журнал
                    .Where(j => j.Id_клиента == clientId)
                    .Select(j => new {
                        j.Действие,
                        j.Дата_и_время
                    })
                    .ToList();

                JournalDataGrid.ItemsSource = journal;
            }
        }
    }
}
