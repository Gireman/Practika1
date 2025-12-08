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
using WpfApp1.Models;


namespace WpfApp1
    {
    public partial class Registration : Window
    {
        // СТАТИЧЕСКИЙ СПИСОК: Буферный массив для хранения всех зарегистрированных пользователей
        public static List<Users> RegisteredUsers { get; set; } = new List<Users>();

        // Для автоматического присвоения ID
        private static int _nextId = 1;

        public Registration()
        {
            InitializeComponent();
        }

        // --------------------------------------------------------------------
        // ЛОГИКА РЕГИСТРАЦИИ
        // --------------------------------------------------------------------
        private void Button_ClickRegistration(object sender, RoutedEventArgs e)
        {
            // 1. Получаем данные из полей
            string login = (FindName("LoginTextBox") as TextBox)?.Text.Trim() ?? string.Empty;
            string password = (FindName("PasswordTextBox") as TextBox)?.Text.Trim() ?? string.Empty;
            string name = (FindName("NameTextBox") as TextBox)?.Text.Trim() ?? string.Empty;
            string surname = (FindName("SurnameTextBox") as TextBox)?.Text.Trim() ?? string.Empty;
            string patronymic = (FindName("PatronymicTextBox") as TextBox)?.Text.Trim() ?? string.Empty;
            string phone = (FindName("PhoneTextBox") as TextBox)?.Text.Trim() ?? string.Empty;
            string email = (FindName("EmailTextBox") as TextBox)?.Text.Trim() ?? string.Empty;
            string birthdayText = (FindName("BirthdayTextBox") as TextBox)?.Text.Trim() ?? string.Empty;


            // 2. Валидация ОБЯЗАТЕЛЬНЫХ полей 
            List<string> requiredFields = new List<string>();

            // Логин
            if (string.IsNullOrWhiteSpace(login) || login == "Логин*")
                requiredFields.Add("Логин");

            // Пароль
            if (string.IsNullOrWhiteSpace(password) || password == "Пароль*")
                requiredFields.Add("Пароль");

            // Имя
            if (string.IsNullOrWhiteSpace(name) || name == "Имя*")
                requiredFields.Add("Имя");

            // Фамилия
            if (string.IsNullOrWhiteSpace(surname) || surname == "Фамилия*")
                requiredFields.Add("Фамилия");

            // Дата рождения
            if (string.IsNullOrWhiteSpace(birthdayText) || birthdayText == "Дата рождения*")
                requiredFields.Add("Дата рождения");

            // Телефон
            if (string.IsNullOrWhiteSpace(phone) || phone == "Телефон" || phone == "Телефон*")
                requiredFields.Add("Телефон");

            // Проверка наличия незаполненных обязательных полей
            if (requiredFields.Count > 0)
            {
                MessageBox.Show($"Пожалуйста, заполните следующие обязательные поля (*):\n\n- {string.Join("\n- ", requiredFields)}", "Ошибка регистрации");
                return;
            }

            // 3. Дополнительная валидация (Дата рождения)
            DateTime birthday;
            // !!! ИСПОЛЬЗУЕМ DateOnly.TryParse
            if (!DateTime.TryParse(birthdayText.Replace("*", "").Trim(), out birthday))
            {
                MessageBox.Show("Некорректный формат даты рождения. Используйте формат ГГГГ-ММ-ДД (например, 1990-12-31).", "Ошибка формата");
                return;
            }

            // 4. Проверка на дубликат логина
            if (RegisteredUsers.Any(u => u.Login == login))
            {
                MessageBox.Show("Пользователь с таким логином уже зарегистрирован.", "Ошибка регистрации");
                return;
            }


            // 5. Создание нового пользователя и добавление в буфер
            Users newUser = new Users
            {
                Id = _nextId++,
                Login = login,
                Password = password,
                Name = name,
                Surname = surname,
                Birthday = birthday, // Тип DateOnly

                // Необязательные поля
                Patronymic = (patronymic == "Отчество" ? string.Empty : patronymic),
                Email = (email == "Почта" ? string.Empty : email),
                Adress = string.Empty, // Адрес оставляем пустым
                Phone = phone
            };

            RegisteredUsers.Add(newUser);

            // 6. Сообщение об успехе и переход на вход
            MessageBox.Show($"Пользователь {newUser.Login} (ID: {newUser.Id}) успешно зарегистрирован!", "Успешная регистрация");

            // Перебрасываем на окно входа
            Button_ClickEnter(sender, e);
        }

        // --------------------------------------------------------------------
        // ОБРАБОТЧИКИ ТЕКСТА-ЗАГЛУШКИ (Placeholder Handlers)
        // --------------------------------------------------------------------

        // Общий обработчик для GotFocus
        private void RegistrationTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            // Убираем текст-заглушку, если она присутствует
            if (textBox.Text.EndsWith("*") || textBox.Text == "Почта" || textBox.Text == "Отчество" || textBox.Text == "Адрес")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.Black; // Меняем цвет на стандартный при фокусе
            }
        }

        // Общий обработчик для LostFocus
        private void RegistrationTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            // Если поле пустое после потери фокуса, восстанавливаем текст-заглушку
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                string originalText = string.Empty;

                if (textBox.Name == "LoginTextBox") originalText = "Логин*";
                else if (textBox.Name == "PasswordTextBox") originalText = "Пароль*";
                else if (textBox.Name == "NameTextBox") originalText = "Имя*";
                else if (textBox.Name == "SurnameTextBox") originalText = "Фамилия*";
                else if (textBox.Name == "PatronymicTextBox") originalText = "Отчество";
                else if (textBox.Name == "PhoneTextBox") originalText = "Телефон*";
                else if (textBox.Name == "EmailTextBox") originalText = "Почта";
                else if (textBox.Name == "BirthdayTextBox") originalText = "Дата рождения*";
                else if (textBox.Name == "AdressTextBox") originalText = "Адрес";

                textBox.Text = originalText;

                // Восстановление полупрозрачного цвета
                //textBox.Foreground = new SolidColorBrush(Color.FromArgb(0x55, 0x10, 0x01, 0x00));
            }
        }

        // --------------------------------------------------------------------
        // МЕТОДЫ НАВИГАЦИИ (Ваши существующие)
        // --------------------------------------------------------------------

        private void Button_ClickBack(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_ClickEnter(object sender, RoutedEventArgs e)
        {
            // Переход на окно входа
            Enter enter = new Enter();
            enter.Show();
            this.Close();
        }
    }
}
