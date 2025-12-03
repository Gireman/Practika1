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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Enter.xaml
    /// </summary>
    public partial class Enter : Window
    {
        // 1. Локальный список пользователей для имитации БД
        private List<LoginData> _usersList = new List<LoginData>()
        {
            new LoginData { Login = "admin", Password = "123", Role = "Admin" },
            new LoginData { Login = "seller", Password = "123", Role = "Seller" },
            new LoginData { Login = "user", Password = "123", Role = "User" }
        };

        public Enter()
        {
            InitializeComponent();
        }

        // --------------------------------------------------------------------
        // ЛОГИКА ВХОДА
        // --------------------------------------------------------------------
        private void Button_ClickEnter(object sender, RoutedEventArgs e)
        {
            // Получаем элементы по имени
            TextBox? loginBox = (TextBox)FindName("LoginTextBox");
            TextBox? passwordBox = (TextBox)FindName("PasswordTextBox"); // Используем TextBox

            // Проверяем, что элементы найдены
            if (loginBox == null || passwordBox == null)
            {
                MessageBox.Show("Ошибка: Поля ввода не найдены (проверьте x:Name в XAML).", "Критическая ошибка");
                return;
            }

            string login = loginBox.Text.Trim();
            string password = passwordBox.Text.Trim(); // Используем .Text для TextBox

            // !!! ГЛАВНОЕ ИСПРАВЛЕНИЕ: Проверяем, не остался ли текст-заглушка
            if (string.IsNullOrWhiteSpace(login) || login == "Логин" ||
                string.IsNullOrWhiteSpace(password) || password == "Пароль")
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль.", "Ошибка ввода");
                return;
            }

            // Ищем пользователя в списке
            LoginData? currentUser = _usersList.FirstOrDefault(u =>
                u.Login == login && u.Password == password);

            if (currentUser != null)
            {
                MessageBox.Show($"Добро пожаловать, {currentUser.Login}! Ваша роль: {currentUser.Role}.", "Успешный вход");

                // Открываем окно в зависимости от роли
                Window nextWindow = currentUser.Role switch
                {
                    "Admin" => new AdminConsole(),
                    "Seller" => new ConsoleSeller(),
                    "User" => new MainWindow(),
                    _ => new MainWindow(),
                };

                nextWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.", "Ошибка аутентификации");
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
