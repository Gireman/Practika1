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
using WpfApp1.Models;
using WpfApp1.models;

namespace WpfApp1
    {
    public partial class Registration : Window
    {
        // СТАТИЧЕСКИЙ СПИСОК: Буферный массив для хранения всех зарегистрированных пользователей
        public static List<Users> RegisteredUsers { get; set; } = new List<Users>();

        // Для автоматического присвоения ID
        private static int _nextId = 1;

        // Определяем константы для заглушек (плейсхолдеров)
        private const string PLACEHOLDER_LOGIN = "Логин*";
        private const string PLACEHOLDER_PASSWORD = "Пароль*";
        private const string PLACEHOLDER_NAME = "Имя*";
        private const string PLACEHOLDER_SURNAME = "Фамилия*";
        private const string PLACEHOLDER_PATRONYMIC = "Отчество";
        private const string PLACEHOLDER_PHONE = "Телефон*";
        private const string PLACEHOLDER_EMAIL = "Почта";
        private const string PLACEHOLDER_BIRTHDAY = "Дата рождения*";
        private const string PLACEHOLDER_ADRESS = "Адрес"; // Хотя адрес не вводится, проверяем на всякий случай

        public Registration()
        {
            InitializeComponent();
        }

        // --------------------------------------------------------------------
        // ЛОГИКА РЕГИСТРАЦИИ (Button_ClickRegistration)
        // --------------------------------------------------------------------
        private void Button_ClickRegistration(object sender, RoutedEventArgs e)
        {
            // 1. Получаем данные из полей ввода
            string login = (FindName("LoginTextBox") as TextBox)?.Text.Trim() ?? string.Empty;
            string password = (FindName("PasswordTextBox") as TextBox)?.Text.Trim() ?? string.Empty;
            string name = (FindName("NameTextBox") as TextBox)?.Text.Trim() ?? string.Empty;
            string surname = (FindName("SurnameTextBox") as TextBox)?.Text.Trim() ?? string.Empty;
            string patronymic = (FindName("PatronymicTextBox") as TextBox)?.Text.Trim() ?? string.Empty;
            string phone = (FindName("PhoneTextBox") as TextBox)?.Text.Trim() ?? string.Empty;
            string email = (FindName("EmailTextBox") as TextBox)?.Text.Trim() ?? string.Empty;
            string birthdayStr = (FindName("BirthdayTextBox") as TextBox)?.Text.Trim() ?? string.Empty;
            string adress = (FindName("AdressTextBox") as TextBox)?.Text.Trim() ?? string.Empty; // Хотя не вводится, его нужно получить

            DateTime birthday;

            // 2. Валидация обязательных полей (со звездочкой)
            if (string.IsNullOrWhiteSpace(login) || login == PLACEHOLDER_LOGIN)
            {
                MessageBox.Show("Пожалуйста, введите Логин.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(password) || password == PLACEHOLDER_PASSWORD)
            {
                MessageBox.Show("Пожалуйста, введите Пароль.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(name) || name == PLACEHOLDER_NAME)
            {
                MessageBox.Show("Пожалуйста, введите Имя.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(surname) || surname == PLACEHOLDER_SURNAME)
            {
                MessageBox.Show("Пожалуйста, введите Фамилию.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(phone) || phone == PLACEHOLDER_PHONE)
            {
                MessageBox.Show("Пожалуйста, введите Телефон.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(birthdayStr) || birthdayStr == PLACEHOLDER_BIRTHDAY || !DateTime.TryParse(birthdayStr, out birthday))
            {
                MessageBox.Show("Пожалуйста, введите корректную Дату рождения (ГГГГ-ММ-ДД).", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new ApplicationContext())
                {
                    // Проверка на уникальность Логина
                    if (db.Users.Any(u => u.Login == login))
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует.", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // 3. Создаем сущность Client (для связи)
                    // Adress может быть пустым (NULL), как запрошено
                    var newClientEntity = new Client
                    {
                        // Адрес при регистрации не вводится, оставляем его NULL или пустым
                        Adress = null // Или (string.IsNullOrWhiteSpace(adress) ? null : adress)
                    };

                    // 4. Создаем сущность User, преобразуя опциональные поля в NULL, если они содержат заглушку
                    var newUserEntity = new User
                    {
                        IdRole = 2, // Только Пользователь
                        Name = name,
                        Surname = surname,
                        // Если Отчество пустое или равно заглушке, то NULL, иначе - значение
                        Patronymic = (string.IsNullOrWhiteSpace(patronymic) || patronymic == PLACEHOLDER_PATRONYMIC) ? null : patronymic,
                        Login = login,
                        Password = password,
                        Phone = phone,
                        // Если Почта пустая или равно заглушке, то NULL, иначе - значение
                        Email = (string.IsNullOrWhiteSpace(email) || email == PLACEHOLDER_EMAIL) ? null : email,
                        Birthday = birthday,

                        // Связываем с Clients
                        ClientEntity = newClientEntity
                    };

                    // 5. Добавляем и сохраняем
                    db.Users.Add(newUserEntity);
                    db.SaveChanges();

                    // 6. Успешная регистрация и навигация
                    MessageBox.Show("Регистрация успешно завершена! Теперь вы можете войти.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Переходим на окно входа
                    Button_ClickEnter(sender, e);
                }
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.InnerException?.Message ?? ex.Message}", "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Непредвиденная ошибка при регистрации: {ex.Message}", "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
