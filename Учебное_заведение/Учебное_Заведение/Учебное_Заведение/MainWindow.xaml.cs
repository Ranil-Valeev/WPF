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

namespace Учебное_Заведение
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text.Trim();
            string password = Pass.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль.");
                return;
            }

            var context = УчебноеЗаведениеEntities.Getcontext();

            var user = context.Пользователи
                .FirstOrDefault(u => u.Логин == login && u.Пароль == password);

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль.");
                return;
            }

            
            switch (user.Роль)
            {
                case "Админ":
                    Admin_pages.Admin admin = new Admin_pages.Admin();
                    admin.Show();
                    break;

                case "Преподаватель":
                    Teacher_pages.Teacher teacher = new Teacher_pages.Teacher(user);
                    teacher.Show();        
                    break;

                case "Студент":
                    Student_pages.Student student = new Student_pages.Student(user);
                    student.Show();
                    
                    break;

                default:
                    MessageBox.Show("Неизвестная роль пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }

            this.Close();
        }
        

        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
