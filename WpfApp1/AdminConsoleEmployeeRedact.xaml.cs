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

namespace WpfApp1
{
    // Добавляем INotifyPropertyChanged для обновления полей формы
    public partial class AdminConsoleEmployeeRedact : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Employee> _employeesList;

        // Буферный объект, к которому привязаны все поля формы
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

        // Конструктор, который получает коллекцию из главного окна
        public AdminConsoleEmployeeRedact(ObservableCollection<Employee> employees)
        {
            InitializeComponent();
            _employeesList = employees;
            DataContext = this;
            CurrentEmployee = new Employee(); // Инициализируем пустой буфер
        }

        // Конструктор по умолчанию (для XAML-дизайнера)
        public AdminConsoleEmployeeRedact() : this(new ObservableCollection<Employee>()) { }

        // *** Обработчик кнопки "Поиск" (SearchButton_Click) ***
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Здесь предполагается, что TextBox для поиска ID назван SearchIdTextBox
            if (int.TryParse(SearchIdTextBox.Text, out int searchId))
            {
                Employee? foundEmployee = _employeesList.FirstOrDefault(emp => emp.Id == searchId);

                if (foundEmployee != null)
                {
                    // Создаем КОПИЮ объекта (буфер), чтобы не изменять оригинал до нажатия "Сохранить"
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

                    MessageBox.Show($"Сотрудник ID: {searchId} найден и загружен для редактирования.", "Успех");
                }
                else
                {
                    MessageBox.Show($"Сотрудник с ID: {searchId} не найден.", "Ошибка");
                    CurrentEmployee = new Employee(); // Очищаем форму
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректный ID сотрудника.", "Ошибка ввода");
            }
        }

        // *** Обработчик кнопки "Сохранить" (ConfirmButton_Click) ***
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentEmployee == null || CurrentEmployee.Id == 0)
            {
                MessageBox.Show("Сначала найдите сотрудника для редактирования.", "Ошибка сохранения");
                return;
            }

            // 1. Ищем оригинальный объект в основной коллекции
            Employee? originalEmployee = _employeesList.FirstOrDefault(emp => emp.Id == CurrentEmployee.Id);

            if (originalEmployee != null)
            {
                // 2. Копируем изменения из буфера (CurrentEmployee) в оригинальный объект
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

                CurrentEmployee = new Employee(); // Очищаем буфер (форму) после сохранения
            }
            else
            {
                MessageBox.Show($"Произошла ошибка: оригинальный сотрудник ID: {CurrentEmployee.Id} не найден.", "Критическая ошибка");
            }
        }

        // Реализация INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // ... (Ваши существующие методы Button_ClickExit и Button_ClickBack)
        private void Button_ClickExit(object sender, RoutedEventArgs e)
        {
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

        private void Button_ClickDelete(object sender, RoutedEventArgs e)
        {
            // 1. Проверяем, загружен ли текущий сотрудник
            if (CurrentEmployee == null || CurrentEmployee.Id == 0)
            {
                MessageBox.Show("Сначала найдите сотрудника для удаления.", "Ошибка удаления");
                return;
            }

            // 2. Запрашиваем подтверждение
            MessageBoxResult result = MessageBox.Show(
                $"Вы уверены, что хотите удалить сотрудника ID: {CurrentEmployee.Id} ({CurrentEmployee.Surname} {CurrentEmployee.Name})?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                // 3. Находим оригинальный объект в коллекции (_employeesList) по ID
                Employee? employeeToDelete = _employeesList.FirstOrDefault(emp => emp.Id == CurrentEmployee.Id);

                if (employeeToDelete != null)
                {
                    // 4. Удаляем объект из ObservableCollection
                    // Это автоматически обновит DataGrid в главном окне.
                    _employeesList.Remove(employeeToDelete);

                    // 5. Очищаем форму редактирования после удаления
                    CurrentEmployee = new Employee(); // Очистка всех полей

                    MessageBox.Show("Сотрудник успешно удален (из буферного массива).", "Удалено");

                    // Опционально: можно вернуться на главный экран после удаления
                    // AdminConsoleEmployee adminConsoleEmployee = new AdminConsoleEmployee();
                    // adminConsoleEmployee.Show();
                    // this.Close();
                }
                else
                {
                    MessageBox.Show($"Ошибка: Сотрудник ID: {CurrentEmployee.Id} не найден в списке.", "Ошибка");
                }
            }
        }
    }
}


