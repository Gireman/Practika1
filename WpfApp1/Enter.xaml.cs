using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
using WpfApp1.models;
using WpfApp1.Models;    // Для модели Users
using WpfApp1.Utilities; // Для AuthManager

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Enter.xaml
    /// </summary>
    public partial class Enter : Window
    {
        // 1. Локальный список пользователей для имитации БД
        //private List<LoginData> _usersList = new List<LoginData>()
        //{
        //    new LoginData { Login = "admin", Password = "123", Role = "Admin" },
        //    new LoginData { Login = "seller", Password = "123", Role = "Seller" },
        //    new LoginData { Login = "user", Password = "123", Role = "User" }
        //};

        public Enter()
        {
            InitializeComponent();
        }

//// 1. Локальный список данных для входа(LoginData)
//        private List<LoginData> _usersList = new List<LoginData>()
//        {
//            new LoginData { Login = "admin", Password = "123", Role = "Admin" },
//            new LoginData { Login = "seller", Password = "123", Role = "Seller" },
//            new LoginData { Login = "user", Password = "123", Role = "User" }
//        };

//        // 2. Имитация полного списка пользователей для сохранения в сессию (Users)
//        // !!! ВАЖНО: Эти данные должны соответствовать логинам из _usersList
//        private List<Users> _usersFullDataList = new List<Users>()
//        {
//            new Users { Id = 1, Login = "admin", Name = "Администратор", Surname = "Системы", Password = "123", Birthday = new DateTime(1999,10,2), },
//            new Users { Id = 2, Login = "seller", Name = "Продавец", Surname = "Магазина", Password = "123", Birthday = new DateTime(1999,10,2), },
//            new Users { Id = 3, Login = "user", Name = "Обычный", Surname = "Пользователь", Password = "123", Birthday = new DateTime(1999,10,2), Phone = "+79999999999" },
//        };

        // --------------------------------------------------------------------
        // ИСПРАВЛЕННАЯ ЛОГИКА ВХОДА
        // --------------------------------------------------------------------
        private void Button_ClickEnter(object sender, RoutedEventArgs e)
        {
            // Получаем данные из полей ввода
            string login = LoginTextBox.Text;
            // В WPF для паролей лучше использовать PasswordBox. 
            // Если у вас TextBox, убедитесь, что это не текст по умолчанию "Пароль"
            string password = PasswordTextBox.Text;

            // Проверка на заполнение полей
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) || password == "Пароль" || login == "Логин")
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль.", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new ApplicationContext())
                {
                    // 1. Ищем пользователя по логину и паролю
                    // Важно: для сотрудников (Id_role = 1) нам нужны связанные данные должности (Post)
                    var userEntity = db.Users
                        .Include(u => u.EmployeeEntity) // Связь с таблицей сотрудников (если role=1)
                        .FirstOrDefault(u => u.Login == login && u.Password == password);
                    // !!! В реальном приложении ПАРОЛЬ ДОЛЖЕН БЫТЬ ХЕШИРОВАН!

                    if (userEntity == null)
                    {
                        MessageBox.Show("Неверный логин или пароль.", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Переменные для навигации
                    Window targetWindow;
                    int postId = 0; // По умолчанию 0

                    // 2. Определяем роль и целевое окно

                    if (userEntity.IdRole == 2)
                    {
                        // РОЛЬ: КЛИЕНТ (Пользователь)
                        // Направляем в главное окно (MainWindow)
                        targetWindow = new MainWindow();
                    }
                    else if (userEntity.IdRole == 1)
                    {
                        // РОЛЬ: СОТРУДНИК
                        // Проверяем, есть ли связанная сущность EmployeeEntity
                        if (userEntity.EmployeeEntity == null)
                        {
                            MessageBox.Show("Сотрудник найден, но его должность не определена. Обратитесь к администратору.", "Ошибка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        postId = userEntity.EmployeeEntity.IdPost;

                        if (postId == 3)
                        {
                            // ДОЛЖНОСТЬ: АДМИНИСТРАТОР (Id_post = 3)
                            // Направляем в Админ Панель
                            targetWindow = new AdminConsole();
                        }
                        else
                        {
                            // ДОЛЖНОСТЬ: ПРОДАВЕЦ ИЛИ ДРУГАЯ РОЛЬ (Id_post != 3)
                            // Направляем в Панель Продавца (предположим, что это окно называется SellerConsole)
                            // ВАЖНО: замените "SellerConsole" на имя вашего окна продавца
                            targetWindow = new ConsoleSeller(); // <-- ЗАМЕНИТЬ НА ИМЯ ВАШЕГО ОКНА ПРОДАВЦА
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Неизвестная роль пользователя (ID роли: {userEntity.IdRole}).", "Ошибка роли", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // 3. Устанавливаем текущую сессию
                    AuthManager.Login(userEntity, postId);

                    // 4. Открываем целевое окно и закрываем текущее
                    targetWindow.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при входе: {ex.Message}", "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // --------------------------------------------------------------------
        // ОБРАБОТЧИКИ ТЕКСТА-ЗАГЛУШКИ (Placeholder Handlers)
        // --------------------------------------------------------------------

        private void LoginTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text == "Логин")
                ((TextBox)sender).Text = string.Empty;
        }

        private void LoginTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((TextBox)sender).Text))
                ((TextBox)sender).Text = "Логин";
        }

        private void PasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Очищаем текст "Пароль" при фокусе
            if (((TextBox)sender).Text == "Пароль")
                ((TextBox)sender).Text = string.Empty;
        }

        private void PasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Возвращаем текст "Пароль", если поле осталось пустым
            if (string.IsNullOrWhiteSpace(((TextBox)sender).Text))
                ((TextBox)sender).Text = "Пароль";
        }

        // --------------------------------------------------------------------
        // ВАШИ СУЩЕСТВУЮЩИЕ МЕТОДЫ НАВИГАЦИИ
        // --------------------------------------------------------------------

        private void Button_ClickRegistration(object sender, RoutedEventArgs e)
        {
            // Здесь должна быть логика открытия окна регистрации
            Registration registration = new Registration(); 
            registration.Show();
            this.Close();
        }

        private void Button_ClickBack(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
