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
using WpfApp1.data;
using WpfApp1.Models;
using WpfApp1.Utilities;
using Microsoft.EntityFrameworkCore; // Важно для использования Include()

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

            // Инициализация коллекции
            Employees = new ObservableCollection<Employee>();

            // Загрузка данных из БД
            LoadEmployeesData();

            DataContext = this;

        }

        private void LoadEmployeesData()
        {
            try
            {
                using (var db = new ApplicationContext())
                {
                    // 1. Загружаем данные из таблицы employees
                    // 2. Используем Include() для подтягивания связанных данных (users и posts)
                    var employeeEntities = db.EmployeeEntities
                        .Include(ee => ee.User)
                        .Include(ee => ee.Post)
                        .ToList();

                    // 3. Используем LINQ для проекции (объединения) данных 
                    // в вашу отображаемую модель Employee
                    var employeeData = employeeEntities.Select(ee => new Employee
                    {
                        Id = ee.User.Id, // Используем Id из таблицы users
                        Name = ee.User.Name,
                        Surname = ee.User.Surname,
                        Patronymic = ee.User.Patronymic,
                        Login = ee.User.Login,
                        Password = ee.User.Password,
                        Phone = ee.User.Phone,
                        Email = ee.User.Email,
                        Birthday = ee.User.Birthday,
                        // НОВОЕ: Загружаем IdPost 
                        IdPost = ee.IdPost, // <-- ДОБАВЛЕНО
                        PostName = ee.Post.PostName, // Используем PostName из таблицы posts
                        Salary = ee.Salary
                    });

                    // 4. Очищаем старые данные и добавляем новые в ObservableCollection
                    Employees.Clear();
                    foreach (var emp in employeeData)
                    {
                        Employees.Add(emp);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка БД");
            }
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
