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
using WpfApp1.Models;
using WpfApp1.Utilities;

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
            CurrentEmployee = new Employee();
        }

        public AdminConsoleEmployeeRedact() : this(new ObservableCollection<Employee>()) { }

        // --- ЛОГИКА ПОИСКА (ИСПРАВЛЕНО) ---
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // !!! ИСПРАВЛЕНИЕ: Читаем ID из TextBox, а не из CurrentEmployee
            // Предполагаем, что имя TextBox в XAML - SearchIdTextBox
            if (!int.TryParse(SearchIdTextBox.Text, out int searchId) || searchId <= 0)
            {
                MessageBox.Show("Пожалуйста, введите корректный ID для поиска (число больше 0).", "Ошибка ввода");
                return;
            }

            // Ищем сотрудника в коллекции по ID, который ввели в TextBox
            Employee? foundEmployee = _employeesList.FirstOrDefault(emp => emp.Id == searchId);

            if (foundEmployee != null)
            {
                // Если найден, копируем его в буфер для редактирования
                CurrentEmployee = new Employee
                {
                    Id = foundEmployee.Id,
                    Name = foundEmployee.Name,
                    Surname = foundEmployee.Surname,
                    Patronymic = foundEmployee.Patronymic,
                    Login = foundEmployee.Login,
                    Password = foundEmployee.Password,
                    Phone = foundEmployee.Phone,
                    Email = foundEmployee.Email,
                    Birthday = foundEmployee.Birthday,
                    Post = foundEmployee.Post,
                    Salary = foundEmployee.Salary
                };
                MessageBox.Show($"Сотрудник ID: {searchId} найден и загружен.", "Успех");
            }
            else
            {
                // Если не найден, предлагаем создать
                // Можно автоматически подставить введенный ID в форму создания
                CurrentEmployee = new Employee { Id = 0 };
                MessageBox.Show($"Сотрудник с ID: {searchId} не найден. Вы можете создать нового сотрудника, заполнив данные и нажав 'Создать'.", "Не найдено");
            }
        }

        // --- НОВЫЙ МЕТОД: СОЗДАНИЕ СОТРУДНИКА (CreateButton_Click) ---
        private void Button_ClickCreate(object sender, RoutedEventArgs e)
        {
            if (CurrentEmployee == null) return;

            // 1. Валидация перед созданием
            if (!ValidateEmployeeData(CurrentEmployee))
            {
                return; // Сообщение об ошибке уже показано в ValidateEmployeeData
            }

            // 2. Логика создания
            // Создаем НОВЫЙ объект, чтобы убедиться, что он не связан с ID из буфера
            Employee newEmployee = new Employee
            {
                // Id оставляем 0 (или не устанавливаем), чтобы имитировать автогенерацию БД
                Id = 0,
                Name = CurrentEmployee.Name,
                Surname = CurrentEmployee.Surname,
                Patronymic = CurrentEmployee.Patronymic,
                Login = CurrentEmployee.Login,
                Password = CurrentEmployee.Password,
                Phone = CurrentEmployee.Phone,
                Email = CurrentEmployee.Email,
                Birthday = CurrentEmployee.Birthday,
                Post = CurrentEmployee.Post,
                Salary = CurrentEmployee.Salary
            };

            // !!! Имитация присвоения ID базой данных
            // Мы временно присваиваем ID в клиенте, чтобы ObservableCollection увидела новый элемент
            // В реальном приложении это происходит на сервере после успешного запроса.
            int newId = _employeesList.Any() ? _employeesList.Max(emp => emp.Id) + 1 : 1;
            newEmployee.Id = newId;

            // 3. Добавляем новый объект в коллекцию
            _employeesList.Add(newEmployee);

            MessageBox.Show($"Новый сотрудник ID: {newEmployee.Id} успешно создан.", "Создание завершено");

            // 4. Очищаем форму (для создания следующего)
            CurrentEmployee = new Employee();
        }

        // --- ИЗМЕНЕННЫЙ МЕТОД: РЕДАКТИРОВАНИЕ СОТРУДНИКА (ConfirmButton_Click) ---
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentEmployee == null) return;

            // 1. Проверяем, что ID указан и существует
            if (CurrentEmployee.Id <= 0)
            {
                MessageBox.Show("Для сохранения/редактирования необходимо ввести корректный ID сотрудника. Для создания используйте кнопку 'Создать'.", "Ошибка редактирования");
                return;
            }

            Employee? originalEmployee = _employeesList.FirstOrDefault(emp => emp.Id == CurrentEmployee.Id);

            if (originalEmployee == null)
            {
                MessageBox.Show($"Сотрудник ID: {CurrentEmployee.Id} не найден для редактирования.", "Ошибка редактирования");
                return;
            }

            // 2. Валидация данных
            if (!ValidateEmployeeData(CurrentEmployee))
            {
                return;
            }

            // 3. Копируем данные из буфера обратно в оригинальный объект ObservableCollection
            originalEmployee.Name = CurrentEmployee.Name;
            originalEmployee.Surname = CurrentEmployee.Surname;
            originalEmployee.Patronymic = CurrentEmployee.Patronymic;
            originalEmployee.Login = CurrentEmployee.Login;
            originalEmployee.Password = CurrentEmployee.Password;
            originalEmployee.Phone = CurrentEmployee.Phone;
            originalEmployee.Email = CurrentEmployee.Email;
            originalEmployee.Birthday = CurrentEmployee.Birthday;
            originalEmployee.Post = CurrentEmployee.Post;
            originalEmployee.Salary = CurrentEmployee.Salary;

            MessageBox.Show($"Данные сотрудника ID: {originalEmployee.Id} успешно сохранены.", "Сохранение завершено");

            // 4. Очищаем буфер (форму) после сохранения
            CurrentEmployee = new Employee();
        }

        // --- МЕТОД ВАЛИДАЦИИ (Общий для Создания и Редактирования) ---
        private bool ValidateEmployeeData(Employee employee)
        {
            // Проверка обязательных строковых полей
            if (string.IsNullOrWhiteSpace(employee.Name) ||
                string.IsNullOrWhiteSpace(employee.Surname) ||
                string.IsNullOrWhiteSpace(employee.Login) ||
                string.IsNullOrWhiteSpace(employee.Password) ||
                string.IsNullOrWhiteSpace(employee.Phone) ||
                employee.Post <= 0)
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля (Имя, Фамилия, Логин, Пароль, Телефон, Должность).", "Ошибка валидации");
                return false;
            }

            // Проверка даты рождения
            if (employee.Birthday == default(DateOnly))
            {
                MessageBox.Show($"Пожалуйста, введите корректную дату рождения в формате ГГГГ-ММ-ДД.", "Ошибка формата");
                return false;
            }

            // Дополнительная валидация (например, зарплата > 0)
            if (employee.Salary <= 0)
            {
                MessageBox.Show("Зарплата должна быть больше нуля.", "Ошибка валидации");
                return false;
            }

            return true;
        }


        // --- ЛОГИКА УДАЛЕНИЯ (DeleteButton_Click) ---
        private void Button_ClickDelete(object sender, RoutedEventArgs e)
        {
            if (CurrentEmployee == null || CurrentEmployee.Id <= 0)
            {
                MessageBox.Show("Пожалуйста, введите ID сотрудника для удаления.", "Ошибка");
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"Вы действительно хотите удалить сотрудника ID: {CurrentEmployee.Id} ({CurrentEmployee.Surname} {CurrentEmployee.Name})?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Employee? employeeToDelete = _employeesList.FirstOrDefault(emp => emp.Id == CurrentEmployee.Id);

                if (employeeToDelete != null)
                {
                    _employeesList.Remove(employeeToDelete);
                    CurrentEmployee = new Employee();

                    MessageBox.Show("Сотрудник успешно удален (из буферного массива).", "Удалено");
                }
                else
                {
                    MessageBox.Show($"Ошибка: Сотрудник ID: {CurrentEmployee.Id} не найден в списке.", "Ошибка");
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


