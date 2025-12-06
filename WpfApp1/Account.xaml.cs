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
using WpfApp1.Utilities; // Для AuthManager

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Account.xaml
    /// </summary>
    public partial class Account : Window
    {
        public Account()
        {
            InitializeComponent();

            // Привязываем DataContext окна к текущему пользователю для отображения данных
            this.DataContext = AuthManager.CurrentUser;
        }

        // ...

        // --------------------------------------------------------------------
        // ЛОГИКА СОХРАНЕНИЯ ДАННЫХ
        // --------------------------------------------------------------------
        private void Button_ClickChange(object sender, RoutedEventArgs e)
        {
            var user = AuthManager.CurrentUser;

            // 1. Проверка: Вошел ли пользователь?
            if (user == null)
            {
                MessageBox.Show("Ошибка: Пользователь не авторизован.", "Ошибка сохранения");
                return;
            }

            // 2. Валидация (добавьте свои проверки)
            // Пример: Проверка, что имя и email не пустые после редактирования
            if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email) || user.Birthday == default)
            {
                MessageBox.Show("Пожалуйста, заполните обязательные поля (Имя, Email, Дата рождения).", "Ошибка валидации");
                return;
            }

            // 3. Сохранение (в вашем случае, данные уже изменены благодаря TwoWay Binding)
            // Здесь должна быть логика отправки запроса к БД (в будущем).

            // Имитация сохранения:
            MessageBox.Show($"Данные аккаунта пользователя '{user.Login}' успешно изменены (в буферном массиве).", "Сохранение завершено");
        }

        // Метод для выхода из аккаунта (предположительно, это Button_ClickEnter из Account.xaml)
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

        private void AccountTextBox_GotFocus(object sender, RoutedEventArgs e)
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
        private void AccountTextBox_LostFocus(object sender, RoutedEventArgs e)
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

        private void Button_ClickBack(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
