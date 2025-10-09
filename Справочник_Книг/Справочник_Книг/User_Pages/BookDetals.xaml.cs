using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace Справочник_Книг.User_Pages
{
    /// <summary>
    /// Логика взаимодействия для BookDetals.xaml
    /// </summary>
    public partial class BookDetals : Window
    {
        private readonly int _bookID;
        public BookDetals(int bookID)
        {
            InitializeComponent();
            _bookID = bookID;
        }
    }
}
