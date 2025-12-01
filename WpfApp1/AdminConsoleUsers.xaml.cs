using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AdminConsoleUsers.xaml
    /// </summary>
    public partial class AdminConsoleUsers : Window
    {
        public ObservableCollection<Users> User { get; set; }
        public AdminConsoleUsers()
        {
            InitializeComponent();

            User = new ObservableCollection<Users>()
            {
                new Users
                {
                    Id = 1,
                    Name = "1",
                    Surname = "1",
                    Patronymic = "1",
                    Login = "1",
                    Password = "1",
                    Phone = "1",
                    Email = "1",
                    Birthday = new DateOnly(1999,10,1),
                    Adress = "1"
                }
            };

            DataContext = this;

        }

        private void Button_ClickEmployee(object sender, RoutedEventArgs e)
        {
            AdminConsoleEmployee adminConsoleEmployee = new AdminConsoleEmployee();
            adminConsoleEmployee.Show();
            this.Close();
        }

        private void Button_ClickExit(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
