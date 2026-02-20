using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
    /// Логика взаимодействия для БронированияPage.xaml
    /// </summary>
    public partial class БронированияPage : Page
    {
        private int _userId;

        public БронированияPage(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadBookings();
        }

        private void LoadBookings()
        {
            
            var context = new ТурАгентствоEntities();

            
            var bookings = context.Бронирования
                .Where(b => b.КлиентId == _userId) 
                .ToList();

            if (bookings.Count == 0)
            {
                NoBookingsText.Visibility = Visibility.Visible;
                BookingsList.ItemsSource = null;
            }
            else
            {
                NoBookingsText.Visibility = Visibility.Collapsed;
                BookingsList.ItemsSource = bookings;
            }
        }

    }
}
