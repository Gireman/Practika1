using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using WpfApp1;
using WpfApp1.data;
using WpfApp1.Models;
using WpfApp1.Utilities;
using Microsoft.EntityFrameworkCore;

namespace WpfApp1
{
    public partial class AdminConsoleEmployeeRedact : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Employee> _employeesList;

        private Employee? _currentEmployee;
        public Employee? CurrentEmployee
        {
            get { return _currentEmployee; }
            set
            {
                _currentEmployee = value;
                OnPropertyChanged();
            }
        }

        public AdminConsoleEmployeeRedact(ObservableCollection<Employee> employees)
        {
            InitializeComponent();
            _employeesList = employees;
            DataContext = this;
            // Инициализируем пустым объектом, чтобы привязки не сломались
            CurrentEmployee = new Employee();
        }

        //public AdminConsoleEmployeeRedact() : this(new ObservableCollection<Employee>()) { }

        // --- ГЛАВНАЯ ЛОГИКА: ПОИСК ПО ID В БАЗЕ ДАННЫХ ---
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Проверяем ввод ID
            if (!int.TryParse(SearchIdTextBox.Text, out int searchId) || searchId <= 0)
            {
                MessageBox.Show("Пожалуйста, введите корректный ID сотрудника (число больше 0).", "Ошибка ввода");
                return;
            }

            try
            {
                // 2. Инициализируем контекст БД
                using (var db = new ApplicationContext())
                {
                    // 3. Запрос к БД: ищем EmployeeEntity по ID пользователя (User.Id)
                    // Используем Include() для загрузки связанных данных (User и Post)
                    var employeeEntity = db.EmployeeEntities
                        .Include(ee => ee.User)
                        .Include(ee => ee.Post)
                        // Ищем Employee, у которого User.Id соответствует введенному searchId
                        .FirstOrDefault(ee => ee.User.Id == searchId);

                    if (employeeEntity != null)
                    {
                        // 4. Если найден, проектируем его в отображаемую модель Employee
                        CurrentEmployee = new Employee
                        {
                            Id = employeeEntity.User.Id,
                            Name = employeeEntity.User.Name,
                            Surname = employeeEntity.User.Surname,
                            Patronymic = employeeEntity.User.Patronymic,
                            Login = employeeEntity.User.Login,
                            Password = employeeEntity.User.Password,
                            Phone = employeeEntity.User.Phone,
                            Email = employeeEntity.User.Email,
                            Birthday = employeeEntity.User.Birthday,
                            PostName = employeeEntity.Post.PostName, // PostName из таблицы posts
                            Salary = employeeEntity.Salary
                        };
                        MessageBox.Show($"Сотрудник ID: {searchId} найден и загружен в форму.", "Успех");
                    }
                    else
                    {
                        // 5. Если не найден, очищаем форму и сообщаем об этом
                        CurrentEmployee = new Employee { Id = searchId }; // Подставляем ID, чтобы пользователь мог начать ввод
                        MessageBox.Show($"Сотрудник с ID: {searchId} не найден. Вы можете создать его, заполнив данные.", "Не найдено");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске данных в БД: {ex.Message}", "Ошибка БД");
            }
        }

private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Здесь будет логика сохранения/обновления (Update) в БД
            MessageBox.Show("Здесь будет логика сохранения в базу данных.", "Заглушка");
        }
        
        private void Button_ClickCreate(object sender, RoutedEventArgs e)
        {
            // Здесь будет логика создания (Create) в БД
            MessageBox.Show("Здесь будет логика создания в базу данных.", "Заглушка");
        }

        private void Button_ClickDelete(object sender, RoutedEventArgs e)
        {
            // Здесь будет логика удаления (Delete) из БД
            MessageBox.Show("Здесь будет логика удаления из базы данных.", "Заглушка");
        }

        // --- РЕАЛИЗАЦИЯ INotifyPropertyChanged ---
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // --- МЕТОДЫ НАВИГАЦИИ ---
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

        private void Button_ClickBack(object sender, RoutedEventArgs e)
        {
            AdminConsoleEmployee adminConsoleEmployee = new AdminConsoleEmployee();
            adminConsoleEmployee.Show();
            this.Close();
        }

        private void Button_ClickUsers(object sender, RoutedEventArgs e)
        {
            AdminConsoleUsers adminConsoleUsers = new AdminConsoleUsers();
            adminConsoleUsers.Show();
            this.Close();
        }
    }
}


