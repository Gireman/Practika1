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
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AdminConsoleUsersRedact.xaml
    /// </summary>
    // Класс AdminConsoleUsersRedact, реализующий INotifyPropertyChanged для обновления UI
    public partial class AdminConsoleUsersRedact : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Users> _usersList;

        // Буферный объект, который будет отображаться в TextBoxes
        private Users? _currentUser;
        public Users? CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                OnPropertyChanged(); // Уведомляет UI об изменении
            }
        }

        // Конструктор, принимающий коллекцию пользователей
        public AdminConsoleUsersRedact(ObservableCollection<Users> users)
        {
            InitializeComponent();
            _usersList = users;
            DataContext = this;
            CurrentUser = new Users(); // Инициализируем пустой буфер
        }

        // Конструктор по умолчанию для XAML-дизайнера
        public AdminConsoleUsersRedact() : this(new ObservableCollection<Users>()) { }

        // --------------------------------------------------------------------
        // ЛОГИКА ПОИСКА
        // --------------------------------------------------------------------
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Находим поле поиска по ID (Предполагаем, что имя TextBox - SearchIdTextBox)
            TextBox searchBox = (TextBox)FindName("SearchIdTextBox");
            if (searchBox == null || !int.TryParse(searchBox.Text, out int searchId))
            {
                MessageBox.Show("Пожалуйста, введите корректный ID пользователя.", "Ошибка ввода");
                return;
            }

            Users? foundUser = _usersList.FirstOrDefault(u => u.Id == searchId);

            if (foundUser != null)
            {
                // Создаем КОПИЮ объекта (БУФЕР) для редактирования
                CurrentUser = new Users
                {
                    Id = foundUser.Id,
                    Name = foundUser.Name,
                    Surname = foundUser.Surname,
                    Patronymic = foundUser.Patronymic,
                    Login = foundUser.Login,
                    Password = foundUser.Password,
                    Phone = foundUser.Phone,
                    Email = foundUser.Email,
                    Birthday = foundUser.Birthday, // DateOnly - это структура, копируется по значению
                    Adress = foundUser.Adress
                };

                MessageBox.Show($"Пользователь ID: {searchId} найден и загружен для редактирования.", "Успех");
            }
            else
            {
                MessageBox.Show($"Пользователь с ID: {searchId} не найден.", "Ошибка");
                CurrentUser = new Users(); // Очищаем форму
            }
        }

        // --- ЛОГИКА СОЗДАНИЯ (CreateButton_Click) ---
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser == null) return;

            if (!ValidateUserData(CurrentUser)) return;

            // Создаем новый объект
            Users newUser = new Users
            {
                // ID сформируем автоматически
                Name = CurrentUser.Name,
                Surname = CurrentUser.Surname,
                Patronymic = CurrentUser.Patronymic,
                Login = CurrentUser.Login,
                Password = CurrentUser.Password,
                Phone = CurrentUser.Phone,
                Email = CurrentUser.Email,
                Birthday = CurrentUser.Birthday,
                Adress = CurrentUser.Adress
            };

            // Авто-генерация ID
            int newId = _usersList.Any() ? _usersList.Max(u => u.Id) + 1 : 1;
            newUser.Id = newId;

            _usersList.Add(newUser);

            MessageBox.Show($"Новый пользователь ID: {newId} успешно создан.", "Создано");

            // Очистка
            CurrentUser = new Users();
            TextBox searchBox = (TextBox)FindName("SearchIdTextBox");
            if (searchBox != null) searchBox.Text = "";
        }

        // --------------------------------------------------------------------
        // ЛОГИКА СОХРАНЕНИЯ (РЕДАКТИРОВАНИЯ)
        // --------------------------------------------------------------------
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser == null || CurrentUser.Id == 0)
            {
                MessageBox.Show("Сначала найдите пользователя для редактирования.", "Ошибка сохранения");
                return;
            }

            Users? originalUser = _usersList.FirstOrDefault(u => u.Id == CurrentUser.Id);

            if (originalUser != null)
            {
                // Копируем изменения из буфера (CurrentUser) обратно в оригинальный объект
                originalUser.Name = CurrentUser.Name;
                originalUser.Surname = CurrentUser.Surname;
                originalUser.Patronymic = CurrentUser.Patronymic;
                originalUser.Login = CurrentUser.Login;
                originalUser.Password = CurrentUser.Password;
                originalUser.Phone = CurrentUser.Phone;
                originalUser.Email = CurrentUser.Email;
                originalUser.Birthday = CurrentUser.Birthday;
                originalUser.Adress = CurrentUser.Adress;

                MessageBox.Show($"Данные пользователя ID: {originalUser.Id} успешно сохранены (в памяти).", "Сохранение завершено");

                CurrentUser = new Users(); // Очистка формы после сохранения
            }
        }

        // --- ВАЛИДАЦИЯ ---
        private bool ValidateUserData(Users user)
        {
            if (string.IsNullOrWhiteSpace(user.Name) ||
                string.IsNullOrWhiteSpace(user.Surname) ||
                string.IsNullOrWhiteSpace(user.Login) ||
                string.IsNullOrWhiteSpace(user.Password))
            {
                MessageBox.Show("Заполните обязательные поля: Имя, Фамилия, Логин, Пароль.", "Ошибка валидации");
                return false;
            }
            if (user.Birthday == default(DateOnly))
            {
                MessageBox.Show("Введите корректную дату рождения.", "Ошибка валидации");
                return false;
            }
            return true;
        }

        // --------------------------------------------------------------------
        // ЛОГИКА УДАЛЕНИЯ
        // --------------------------------------------------------------------
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Проверяем, загружен ли текущий пользователь
            if (CurrentUser == null || CurrentUser.Id == 0)
            {
                MessageBox.Show("Сначала найдите пользователя для удаления.", "Ошибка удаления");
                return;
            }

            // 2. Запрашиваем подтверждение
            MessageBoxResult result = MessageBox.Show(
                $"Вы уверены, что хотите удалить пользователя ID: {CurrentUser.Id} ({CurrentUser.Surname} {CurrentUser.Name})?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                // 3. Находим оригинальный объект в коллекции (_usersList) по ID
                Users? userToDelete = _usersList.FirstOrDefault(u => u.Id == CurrentUser.Id);

                if (userToDelete != null)
                {
                    // 4. Удаляем объект из ObservableCollection
                    _usersList.Remove(userToDelete);

                    // 5. Очищаем форму редактирования после удаления
                    CurrentUser = new Users();

                    MessageBox.Show("Пользователь успешно удален (из буферного массива).", "Удалено");
                }
                else
                {
                    MessageBox.Show($"Ошибка: Пользователь ID: {CurrentUser.Id} не найден в списке.", "Ошибка");
                }
            }
        }

        // Реализация INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // --------------------------------------------------------------------
        // МЕТОДЫ НАВИГАЦИИ (ОСТАВЛЯЕМ ВАШИ)
        // --------------------------------------------------------------------
        private void Button_ClickEmployee(object sender, RoutedEventArgs e)
        {
            AdminConsoleEmployee adminConsoleEmployee = new AdminConsoleEmployee();
            adminConsoleEmployee.Show();
            this.Close();
        }

        private void Button_ClickBack(object sender, RoutedEventArgs e)
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
