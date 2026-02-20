using Steam.Model;
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

namespace Steam.Pages
{
    /// <summary>
    /// Логика взаимодействия для Gamedetal.xaml
    /// </summary>
    public partial class Gamedetal : Page
    {
        private Игра _game;
        public Gamedetal(Игра game)
        {
            InitializeComponent();
            
            _game = game;
            DataContext = _game;
        }
    }
}
