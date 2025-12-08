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
using WpfApp1.models;
using WpfApp1.Utilities; // Для AuthManager
using WpfApp1.Models;    // Для модели Users

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

// 1. Локальный список данных для входа(LoginData)
        private List<LoginData> _usersList = new List<LoginData>()
        {
            new LoginData { Login = "admin", Password = "123", Role = "Admin" },
            new LoginData { Login = "seller", Password = "123", Role = "Seller" },
            new LoginData { Login = "user", Password = "123", Role = "User" }
        };

        // 2. Имитация полного списка пользователей для сохранения в сессию (Users)
        // !!! ВАЖНО: Эти данные должны соответствовать логинам из _usersList
        private List<Users> _usersFullDataList = new List<Users>()
        {
            new Users { Id = 1, Login = "admin", Name = "Администратор", Surname = "Системы", Password = "123", Birthday = new DateTime(1999,10,2), },
            new Users { Id = 2, Login = "seller", Name = "Продавец", Surname = "Магазина", Password = "123", Birthday = new DateTime(1999,10,2), },
            new Users { Id = 3, Login = "user", Name = "Обычный", Surname = "Пользователь", Password = "123", Birthday = new DateTime(1999,10,2), Phone = "+79999999999" },
        };

        // --------------------------------------------------------------------
        // ИСПРАВЛЕННАЯ ЛОГИКА ВХОДА
        // --------------------------------------------------------------------
        private void Button_ClickEnter(object sender, RoutedEventArgs e)
        {
            TextBox? loginBox = this.FindName("LoginTextBox") as TextBox;
            TextBox? passwordBox = this.FindName("PasswordTextBox") as TextBox;

            if (loginBox == null || passwordBox == null || loginBox.Text == "Логин" || passwordBox.Text == "Пароль")
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль.", "Ошибка");
                return;
            }

            string login = loginBox.Text;
            string password = passwordBox.Text;

            // 1. Проверка учетных данных
            LoginData? validLoginData = _usersList.FirstOrDefault(u =>
                u.Login == login && u.Password == password);

            if (validLoginData != null)
            {
                // 2. Вход успешен. Получаем полный объект пользователя.
                Users? currentUser = _usersFullDataList.FirstOrDefault(u => u.Login == login);

                if (currentUser != null)
                {
                    // 3. Сохраняем данные пользователя в глобальной сессии
                    AuthManager.Login(currentUser);

                    // 4. Уведомление без имени пользователя
                    MessageBox.Show("Вход выполнен успешно!", "Вход");

                    // 5. Ролевая навигация
                    Window nextWindow;
                    switch (validLoginData.Role)
                    {
                        case "Admin":
                            // Навигация для Администратора
                            // Вам нужно убедиться, что класс AdminConsole существует
                            // Примечание: предполагается, что вы замените этот класс на нужный
                            nextWindow = new AdminConsole();
                            break;
                        case "Seller":
                            // Навигация для Продавца
                            // Класс ConsoleSeller был упомянут в ваших предыдущих файлах
                            nextWindow = new ConsoleSeller();
                            break;
                        case "User":
                        default:
                            // Навигация для Обычного Пользователя (остается на главном окне)
                            nextWindow = new MainWindow();
                            break;
                    }

                    nextWindow.Show();
                    this.Close(); // Закрываем окно входа
                }
                else
                {
                    MessageBox.Show("Ошибка: Полные пользовательские данные не найдены.", "Ошибка");
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.", "Ошибка входа");
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
