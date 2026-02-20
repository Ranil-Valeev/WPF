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
using Учебное_Заведение.model;

namespace Учебное_Заведение.Teacher_pages
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        private Преподаватели _teacher;

        public Profile(Преподаватели teacher)
        {
            InitializeComponent();
            _teacher = teacher;

            var user = УчебноеЗаведениеEntities.Getcontext().Пользователи.FirstOrDefault(x => x.ID_Пользователя == _teacher.ID_Пользователя);
            if (user != null)
            {
                FIOText.Text = _teacher.ФИО;
                LoginText.Text = user.Логин;
                RoleText.Text = user.Роль;
            }
        }
    }
}
