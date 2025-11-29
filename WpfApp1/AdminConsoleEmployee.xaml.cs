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
    /// Логика взаимодействия для AdminConsoleEmployee.xaml
    /// </summary>
    public partial class AdminConsoleEmployee : Window
    {
        public ObservableCollection<Employees> Employee { get; set; }
        public AdminConsoleEmployee()
        {
            InitializeComponent();

            Employee = new ObservableCollection<Employees>()
            {
                new Employees
                {
                    Id = 1,
                    Name = "1",
                    Surname = "1",
                    Patronymic = "1",
                    Login = "1",
                    Password = "1",
                    Phone = "1",
                    Email = "1",
                    Birthday = "1",
                    Post = "1",
                    Salary = 15000
                }
            };

            DataContext = this;

        }

        private void Button_ClickUsers(object sender, RoutedEventArgs e)
        {
            AdminConsoleUsers adminConsoleUsers = new AdminConsoleUsers();
            adminConsoleUsers.Show();
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
