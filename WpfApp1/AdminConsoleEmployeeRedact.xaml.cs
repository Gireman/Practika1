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

        // НОВОЕ СВОЙСТВО: Список всех должностей для ComboBox
        private ObservableCollection<Post> _allPosts;
        public ObservableCollection<Post> AllPosts // <-- НОВОЕ СВОЙСТВО
        {
            get { return _allPosts; }
            set
            {
                _allPosts = value;
                OnPropertyChanged();
            }
        }

        public AdminConsoleEmployeeRedact(ObservableCollection<Employee> employees)
        {
            InitializeComponent();
            _employeesList = employees;
            DataContext = this;
            // Инициализируем коллекцию должностей
            AllPosts = new ObservableCollection<Post>(); // <-- Инициализация
            LoadAllPosts(); // <-- Загрузка данных

            // Инициализируем пустым объектом, чтобы привязки не сломались
            CurrentEmployee = new Employee();
        }

        //public AdminConsoleEmployeeRedact() : this(new ObservableCollection<Employee>()) { }

            // НОВЫЙ МЕТОД: Загрузка всех должностей
        private void LoadAllPosts()
        {
            try
            {
                using (var db = new ApplicationContext())
                {
                    var posts = db.Posts.ToList(); // Загружаем все должности
                    AllPosts.Clear();
                    foreach (var post in posts)
                    {
                        AllPosts.Add(post);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке списка должностей: {ex.Message}", "Ошибка БД");
            }
        }

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
                            // ИЗМЕНЕНИЕ: Устанавливаем IdPost (именно его выводим в TextBox)
                            IdPost = employeeEntity.IdPost, // <-- ИСПОЛЬЗУЕМ IdPost
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
            // 1. Проверка, был ли найден сотрудник для редактирования
            if (CurrentEmployee == null || CurrentEmployee.Id == 0)
            {
                MessageBox.Show("Сначала найдите сотрудника для редактирования.", "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new ApplicationContext())
                {
                    // 2. Ищем записи в БД, соответствующие отредактированному сотруднику
                    // Ищем User по Id (т.к. Id Employee == Id User)
                    var userToUpdate = db.Users.FirstOrDefault(u => u.Id == CurrentEmployee.Id);

                    // Ищем EmployeeEntity по IdUser
                    var employeeEntityToUpdate = db.EmployeeEntities.FirstOrDefault(ee => ee.IdUser == CurrentEmployee.Id);

                    if (userToUpdate == null || employeeEntityToUpdate == null)
                    {
                        MessageBox.Show("Не удалось найти записи сотрудника в базе данных.", "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // 3. Обновляем данные в таблице 'users'
                    userToUpdate.Name = CurrentEmployee.Name;
                    userToUpdate.Surname = CurrentEmployee.Surname;
                    userToUpdate.Patronymic = CurrentEmployee.Patronymic;
                    userToUpdate.Login = CurrentEmployee.Login;
                    userToUpdate.Password = CurrentEmployee.Password;
                    userToUpdate.Phone = CurrentEmployee.Phone;
                    userToUpdate.Email = CurrentEmployee.Email;
                    userToUpdate.Birthday = CurrentEmployee.Birthday;

                    // 4. Обновляем данные в таблице 'employees'
                    // Теперь здесь используется IdPost, который изменился благодаря ComboBox
                    employeeEntityToUpdate.IdPost = CurrentEmployee.IdPost;
                    employeeEntityToUpdate.Salary = CurrentEmployee.Salary;

                    // 5. Отправляем изменения в базу данных
                    db.SaveChanges();

                    // 6. Обновляем локальную коллекцию для немедленного отображения в AdminConsoleEmployee
                    UpdateLocalEmployeeList(CurrentEmployee);

                    MessageBox.Show($"Данные сотрудника {CurrentEmployee.Surname} успешно обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    // 7. Возвращаемся в главное окно администрирования сотрудников
                    Button_ClickBack(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}", "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // --- ДОБАВИТЬ ЭТОТ НОВЫЙ МЕТОД ---
        private void UpdateLocalEmployeeList(Employee updatedEmployee)
        {
            if (_employeesList != null)
            {
                // Ищем старый объект в коллекции
                var oldEmployee = _employeesList.FirstOrDefault(e => e.Id == updatedEmployee.Id);

                if (oldEmployee != null)
                {
                    // Обновляем все свойства старого объекта
                    oldEmployee.Name = updatedEmployee.Name;
                    oldEmployee.Surname = updatedEmployee.Surname;
                    oldEmployee.Patronymic = updatedEmployee.Patronymic;
                    oldEmployee.Login = updatedEmployee.Login;
                    oldEmployee.Password = updatedEmployee.Password;
                    oldEmployee.Phone = updatedEmployee.Phone;
                    oldEmployee.Email = updatedEmployee.Email;
                    oldEmployee.Birthday = updatedEmployee.Birthday;
                    oldEmployee.IdPost = updatedEmployee.IdPost;
                    oldEmployee.Salary = updatedEmployee.Salary;

                    // Дополнительно: находим название должности по Id для корректного отображения в DataGrid
                    var post = AllPosts.FirstOrDefault(p => p.Id == updatedEmployee.IdPost);
                    oldEmployee.PostName = post?.PostName ?? "Неизвестно";
                }
            }
        }

        private void Button_ClickCreate(object sender, RoutedEventArgs e)
        {
            // 1. Проверка на пустые/невалидные данные
            if (CurrentEmployee == null ||
                string.IsNullOrWhiteSpace(CurrentEmployee.Name) ||
                string.IsNullOrWhiteSpace(CurrentEmployee.Surname) ||
                string.IsNullOrWhiteSpace(CurrentEmployee.Login) ||
                string.IsNullOrWhiteSpace(CurrentEmployee.Password) ||
                string.IsNullOrWhiteSpace(CurrentEmployee.Phone) ||
                CurrentEmployee.IdPost == 0 || // Проверяем, что должность выбрана (IdPost > 0)
                CurrentEmployee.Salary <= 0 ||
                CurrentEmployee.Birthday == default(DateTime))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля (Имя, Фамилия, Логин, Пароль, Телефон, ID Должности, Зарплата, Дата Рождения).", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new ApplicationContext())
                {
                    // 2. Создаем новую сущность User
                    var newUser = new User
                    {
                        // ID не задаем, он будет сгенерирован БД (auto increment)
                        IdRole = 1, // Предполагаем, что 2 - это ID роли для обычных сотрудников
                        Name = CurrentEmployee.Name,
                        Surname = CurrentEmployee.Surname,
                        Patronymic = CurrentEmployee.Patronymic,
                        Login = CurrentEmployee.Login,
                        Password = CurrentEmployee.Password, // В реальном приложении нужно хэшировать!
                        Phone = CurrentEmployee.Phone,
                        Email = CurrentEmployee.Email,
                        Birthday = CurrentEmployee.Birthday
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges(); // Сохраняем User, чтобы получить сгенерированный ID

                    // ID User теперь доступен в newUser.Id
                    int newUserId = newUser.Id;

                    // 3. Создаем новую сущность EmployeeEntity, используя ID нового пользователя
                    var newEmployeeEntity = new EmployeeEntity
                    {
                        IdUser = newUserId,
                        IdPost = CurrentEmployee.IdPost, // Используем IdPost из ComboBox/TextBox
                        Salary = CurrentEmployee.Salary
                    };

                    db.EmployeeEntities.Add(newEmployeeEntity);
                    db.SaveChanges(); // Сохраняем EmployeeEntity

                    // 4. Обновляем локальную коллекцию и UI
                    // Присваиваем ID созданному объекту Employee для использования в UI
                    CurrentEmployee.Id = newUserId;

                    // Создаем новый объект Employee для DataGrid главного окна
                    var newEmployeeForList = new Employee
                    {
                        Id = newUserId,
                        Name = CurrentEmployee.Name,
                        Surname = CurrentEmployee.Surname,
                        Patronymic = CurrentEmployee.Patronymic,
                        Login = CurrentEmployee.Login,
                        Password = CurrentEmployee.Password,
                        Phone = CurrentEmployee.Phone,
                        Email = CurrentEmployee.Email,
                        Birthday = CurrentEmployee.Birthday,
                        IdPost = CurrentEmployee.IdPost,
                        Salary = CurrentEmployee.Salary
                    };

                    // Находим название должности для DataGrid
                    var post = AllPosts.FirstOrDefault(p => p.Id == CurrentEmployee.IdPost);
                    newEmployeeForList.PostName = post?.PostName ?? "Неизвестно";

                    _employeesList.Add(newEmployeeForList); // Добавляем в коллекцию главного окна

                    MessageBox.Show($"Сотрудник {CurrentEmployee.Surname} успешно создан с ID: {newUserId}.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Очищаем форму для следующего ввода или возвращаемся назад
                    Button_ClickBack(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании нового сотрудника: {ex.Message}", "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_ClickDelete(object sender, RoutedEventArgs e)
        {
            // 1. Проверка, был ли найден сотрудник для удаления
            if (CurrentEmployee == null || CurrentEmployee.Id == 0)
            {
                MessageBox.Show("Сначала найдите сотрудника, которого нужно удалить, используя поле ID.", "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Запрос подтверждения у пользователя
            MessageBoxResult result = MessageBox.Show(
                $"Вы уверены, что хотите удалить сотрудника: {CurrentEmployee.Surname} {CurrentEmployee.Name} (ID: {CurrentEmployee.Id})?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                return; // Пользователь отменил удаление
            }

            try
            {
                using (var db = new ApplicationContext())
                {
                    int employeeId = CurrentEmployee.Id;

                    // --- ШАГ 3. Удаление из EmployeeEntity (зависимая таблица) ---
                    var employeeEntityToDelete = db.EmployeeEntities
                        .FirstOrDefault(ee => ee.IdUser == employeeId);

                    if (employeeEntityToDelete != null)
                    {
                        db.EmployeeEntities.Remove(employeeEntityToDelete);
                    }

                    // --- ШАГ 4. Удаление из Users (главная таблица) ---
                    var userToDelete = db.Users.FirstOrDefault(u => u.Id == employeeId);

                    if (userToDelete != null)
                    {
                        db.Users.Remove(userToDelete);
                    }
                    else
                    {
                        // Это может случиться, если запись в users уже была удалена, 
                        // или если поиск не сработал. В любом случае, продолжаем, 
                        // чтобы сохранить изменения и обновить список.
                        MessageBox.Show("Запись пользователя не найдена в таблице users, но удаление будет продолжено.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    // 5. Сохраняем все изменения в базе данных
                    db.SaveChanges();

                    // 6. Обновляем локальную коллекцию
                    RemoveLocalEmployee(employeeId);

                    MessageBox.Show($"Сотрудник {CurrentEmployee.Surname} (ID: {employeeId}) успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    // 7. Возвращаемся в главное окно администрирования сотрудников
                    Button_ClickBack(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении сотрудника: {ex.Message}", "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // --- ДОБАВИТЬ ЭТОТ НОВЫЙ МЕТОД ---
        private void RemoveLocalEmployee(int id)
        {
            if (_employeesList != null)
            {
                // Ищем и удаляем объект из коллекции главного окна
                var employeeToRemove = _employeesList.FirstOrDefault(e => e.Id == id);
                if (employeeToRemove != null)
                {
                    _employeesList.Remove(employeeToRemove);
                }
            }
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


