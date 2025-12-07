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
            if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Phone) || user.Birthday == default || string.IsNullOrWhiteSpace(user.Password) || string.IsNullOrWhiteSpace(user.Surname))
            {
                MessageBox.Show("Пожалуйста, заполните обязательные поля (Пароль, Имя, Фамилия, Телефон, Дата рождения).", "Ошибка валидации");
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

        private void Button_ClickBack(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
