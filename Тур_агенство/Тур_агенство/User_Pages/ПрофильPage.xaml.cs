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
using Тур_агенство.model;

namespace Тур_агенство.User_Pages
{
    /// <summary>
    /// Логика взаимодействия для ПрофильPage.xaml
    /// </summary>
    public partial class ПрофильPage : Page
    {
        private int _userId;
        private ТурАгентствоEntities _context = new ТурАгентствоEntities();

        public ПрофильPage(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadProfile();
        }

        private void LoadProfile()
        {
            var user = _context.Клиенты.FirstOrDefault(u => u.Id == _userId);
            if (user != null)
            {
                NameBox.Text = user.ФИО;
                EmailBox.Text = user.ЭлектроннаяПочта;
                PhoneBox.Text = user.Телефон;
            }
        }

        //private void SaveChanges_Click(object sender, RoutedEventArgs e)
        //{
        //    var user = _context.Клиенты.FirstOrDefault(u => u.Id == _userId);
        //    if (user != null)
        //    {
        //        user.ФИО = NameBox.Text;
        //        user.ЭлектроннаяПочта = EmailBox.Text;
        //        user.Телефон = PhoneBox.Text;

        //        _context.SaveChanges();
        //        MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        //    }
        //}
    }
}
