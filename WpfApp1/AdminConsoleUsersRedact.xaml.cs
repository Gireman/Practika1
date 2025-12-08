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
using WpfApp1.Utilities;
// --- ДОБАВИТЬ ЭТИ using'и ---
using WpfApp1.data;
using Microsoft.EntityFrameworkCore;
using WpfApp1.models;
// ----------------------------

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

            // Инициализируем пустым объектом, чтобы привязки не сломались
            CurrentUser = new Users();
        }

        // Конструктор по умолчанию для XAML-дизайнера
        //public AdminConsoleUsersRedact() : this(new ObservableCollection<Users>()) { }

        // --------------------------------------------------------------------
        // НОВЫЙ МЕТОД: ПОИСК ПОЛЬЗОВАТЕЛЯ ПО ID
        // --------------------------------------------------------------------
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем ID для поиска из TextBox, который привязан к CurrentUser.Id
            int searchId = CurrentUser?.Id ?? 0;

            if (searchId <= 0)
            {
                MessageBox.Show("Введите корректный ID для поиска.", "Ошибка ввода");
                return;
            }

            try
            {
                using (var db = new ApplicationContext())
                {
                    // 1. Ищем запись User в БД, включая связанные данные Client
                    var userEntity = db.Users
                        .Include(u => u.ClientEntity) // Загружаем Адрес
                        .Where(u => u.IdRole == 2)
                        .FirstOrDefault(u => u.Id == searchId);

                    if (userEntity != null)
                    {
                        // 2. Если найден, проектируем его в отображаемую модель Users (UI Model)
                        CurrentUser = new Users
                        {
                            Id = userEntity.Id,
                            Name = userEntity.Name,
                            Surname = userEntity.Surname,
                            Patronymic = userEntity.Patronymic ?? string.Empty,
                            Login = userEntity.Login,
                            Password = userEntity.Password,
                            Phone = userEntity.Phone,
                            Email = userEntity.Email ?? string.Empty,
                            Birthday = userEntity.Birthday,

                            // Получаем Adress из связанной таблицы Client, используя безопасный доступ
                            Adress = userEntity.ClientEntity?.Adress ?? string.Empty
                        };

                        MessageBox.Show($"Пользователь ID {searchId} ({CurrentUser.Surname}) найден.", "Успех");
                    }
                    else
                    {
                        MessageBox.Show($"Пользователь с ID {searchId} не найден.", "Ошибка");
                        // Сбрасываем все поля, кроме ID
                        CurrentUser = new Users { Id = searchId };
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске в базе данных: {ex.Message}", "Ошибка БД");
            }
        }

        // --- ЛОГИКА СОЗДАНИЯ (CreateButton_Click) ---
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        // --------------------------------------------------------------------
        // ЛОГИКА СОХРАНЕНИЯ (РЕДАКТИРОВАНИЯ)
        // --------------------------------------------------------------------
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Проверка, был ли найден пользователь для редактирования
            if (CurrentUser == null || CurrentUser.Id == 0)
            {
                MessageBox.Show("Сначала найдите пользователя для редактирования.", "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new ApplicationContext())
                {
                    int userId = CurrentUser.Id;

                    // 1. Ищем записи в БД
                    var userToUpdate = db.Users.FirstOrDefault(u => u.Id == userId);

                    // Ищем связанную запись ClientEntity по ВНЕШНЕМУ КЛЮЧУ IdUser
                    // Так как отношение 1:1, мы ищем ОДНУ запись.
                    var clientEntityToUpdate = db.Clients.FirstOrDefault(c => c.IdUser == userId);

                    if (userToUpdate == null) { /* ... ошибка ... */ return; }

                    // 2. Обновляем данные в таблице 'users'
                    userToUpdate.Name = CurrentUser.Name;
                    userToUpdate.Surname = CurrentUser.Surname;
                    userToUpdate.Patronymic = CurrentUser.Patronymic;
                    userToUpdate.Login = CurrentUser.Login;
                    userToUpdate.Password = CurrentUser.Password;
                    userToUpdate.Phone = CurrentUser.Phone;
                    userToUpdate.Email = CurrentUser.Email;
                    userToUpdate.Birthday = CurrentUser.Birthday;

                    // 3. Обработка данных в таблице 'clients' (Адрес)
                    bool hasAdress = !string.IsNullOrWhiteSpace(CurrentUser.Adress);

                    if (clientEntityToUpdate != null)
                    {
                        // СЛУЧАЙ 1: Запись клиента СУЩЕСТВУЕТ (Id будет использован для UPDATE)
                        if (hasAdress)
                        {
                            // Обновляем существующий адрес
                            clientEntityToUpdate.Adress = CurrentUser.Adress;
                            // db.Clients.Update(clientEntityToUpdate); // Необязательно, EF отследит
                        }
                        else
                        {
                            // Если адрес очищен, удаляем запись клиента (потребует Id)
                            db.Clients.Remove(clientEntityToUpdate);
                        }
                    }
                    else if (hasAdress)
                    {
                        // СЛУЧАЙ 2: Записи клиента НЕТ, но адрес был введен. Создаем новую запись.
                        var newClientEntity = new Client
                        {
                            // Id не указываем (Auto Increment)
                            IdUser = userId, // Связываем с User
                            Adress = CurrentUser.Adress
                        };
                        db.Clients.Add(newClientEntity); // Добавляем новую запись (INSERT)
                    }

                    // 4. Отправляем изменения в базу данных
                    db.SaveChanges();

                    // ... (Остальной код)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}", "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // --- ДОБАВИТЬ ЭТОТ НОВЫЙ МЕТОД для обновления UI ---
        private void UpdateLocalUserList(Users updatedUser)
        {
            if (_usersList != null)
            {
                // Ищем старый объект в коллекции
                var oldUser = _usersList.FirstOrDefault(u => u.Id == updatedUser.Id);

                if (oldUser != null)
                {
                    // Обновляем все свойства старого объекта
                    oldUser.Name = updatedUser.Name;
                    oldUser.Surname = updatedUser.Surname;
                    oldUser.Patronymic = updatedUser.Patronymic;
                    oldUser.Login = updatedUser.Login;
                    oldUser.Password = updatedUser.Password;
                    oldUser.Phone = updatedUser.Phone;
                    oldUser.Email = updatedUser.Email;
                    oldUser.Birthday = updatedUser.Birthday;
                    oldUser.Adress = updatedUser.Adress; // Обновляем адрес
                }
            }
        }

        // --------------------------------------------------------------------
        // ЛОГИКА УДАЛЕНИЯ
        // --------------------------------------------------------------------
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            
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
            // Выход из сессии
            AuthManager.Logout();

            MessageBox.Show("Вы успешно вышли из аккаунта.", "Выход");

            // Возвращаемся на главное окно
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
