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
using WpfApp1.Utilities;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AdminConsoleEmployee.xaml
    /// </summary>
    public partial class AdminConsoleEmployee : Window
    {
        public ObservableCollection<Employee> Employees { get; set; }
        public AdminConsoleEmployee()
        {
            InitializeComponent();

            Employees = new ObservableCollection<Employee>()
            {
                new Employee
                {
                    Id = 1,
                    Name = "Name",
                    Surname = "Surname",
                    Patronymic = "Patronymic",
                    Login = "Login",
                    Password = "Password",
                    Phone = "+79313122078",
                    Email = "Email",
                    Birthday = new DateOnly(1990,1,1),
                    Post = 1,
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
            // Выход из сессии
            AuthManager.Logout();

            MessageBox.Show("Вы успешно вышли из аккаунта.", "Выход");

            // Возвращаемся на главное окно
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_ClickRedactEmployee(object sender, RoutedEventArgs e)
        {
            // Передаем коллекцию Employees в конструктор окна редактирования
            AdminConsoleEmployeeRedact redactWindow = new AdminConsoleEmployeeRedact(Employees);
            redactWindow.Show();
            this.Close();
        }
    }
}
