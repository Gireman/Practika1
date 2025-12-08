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
using WpfApp1.Utilities; // Для AuthManager

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Account.xaml
    /// </summary>
    public partial class Account : Window
    {
        // --------------------------------------------------------------------
        // КОНСТРУКТОР
        // --------------------------------------------------------------------
        public Account()
        {
            InitializeComponent();

            // 1. Проверка авторизации
            if (AuthManager.CurrentUser == null)
            {
                // Если пользователь не авторизован, возвращаем на страницу входа
                MessageBox.Show("Сначала необходимо войти в аккаунт.", "Ошибка доступа", MessageBoxButton.OK, MessageBoxImage.Error);

                // Переходим на главное окно (откуда обычно идет навигация на Enter)
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
                return;
            }

            // 2. Загружаем данные пользователя из БД, включая связанные данные клиента
            // Это гарантирует, что мы работаем с актуальной и полной сущностью, 
            // которая содержит ClientEntity (для Adress)
            LoadUserData();

            // Привязываем DataContext окна к текущему пользователю для отображения данных
            this.DataContext = AuthManager.CurrentUser;
        }

        /// <summary>
        /// Повторная загрузка данных пользователя из БД с включением ClientEntity.
        /// </summary>
        private void LoadUserData()
        {
            try
            {
                using (var db = new ApplicationContext())
                {
                    // Загружаем пользователя и связанного клиента
                    var userWithClient = db.Users
                        .Include(u => u.ClientEntity)
                        .FirstOrDefault(u => u.Id == AuthManager.CurrentUser.Id);

                    if (userWithClient != null)
                    {
                        // Обновляем AuthManager.CurrentUser, чтобы он был полной отслеживаемой сущностью
                        AuthManager.Login(userWithClient, AuthManager.CurrentUserPostId);
                    }
                    else
                    {
                        // В случае, если пользователя по какой-то причине не удалось найти
                        MessageBox.Show("Не удалось загрузить полные данные аккаунта. Выполняется выход.", "Ошибка загрузки");
                        AuthManager.Logout();
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка БД");
            }
        }


        // --------------------------------------------------------------------
        // ЛОГИКА СОХРАНЕНИЯ ДАННЫХ (Button_ClickChange)
        // --------------------------------------------------------------------
        private void Button_ClickChange(object sender, RoutedEventArgs e)
        {
            var userToUpdate = AuthManager.CurrentUser;

            if (userToUpdate == null)
            {
                MessageBox.Show("Ошибка: Пользователь не авторизован.", "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 1. Валидация обязательных полей
            if (string.IsNullOrWhiteSpace(userToUpdate.Name) ||
                string.IsNullOrWhiteSpace(userToUpdate.Surname) ||
                string.IsNullOrWhiteSpace(userToUpdate.Password) ||
                string.IsNullOrWhiteSpace(userToUpdate.Phone) ||
                userToUpdate.Birthday == default)
            {
                MessageBox.Show("Пожалуйста, заполните обязательные поля: Имя, Фамилия, Пароль, Телефон и Дата рождения.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new ApplicationContext())
                {
                    // Подключаем сущность к текущему контексту. 
                    // Поскольку мы ее обновили через DataContext, EF отследит изменения.
                    db.Users.Attach(userToUpdate);

                    // 2. Обновление связанных данных клиента (Adress)
                    // Поскольку ClientEntity был загружен в LoadUserData, он должен быть доступен
                    if (userToUpdate.ClientEntity != null)
                    {
                        // Adress привязан к ClientEntity.Adress через TwoWay Binding на свойство User.ClientEntity.Adress.
                        // Если в вашем XAML Binding на Adress был {Binding Adress}, то это привязано к свойству User.Adress, 
                        // которое, видимо, отсутствует.
                        // Для работы с {Binding Adress} вам нужна следующая строка:

                        // ПРИМЕЧАНИЕ: Предполагается, что вы поправили XAML и он привязан к полю Adress, 
                        // которое находится в ClientEntity.
                        // Поэтому мы просто сохраняем UserToUpdate.

                        // Если XAML привязан к свойству User.ClientEntity.Adress, то изменения уже в буфере.
                        // Если XAML привязан к {Binding Adress} (как в вашем файле Account.xaml),
                        // это привязано к User.Adress, которого нет в вашей модели.
                        // Чтобы избежать ошибок, я буду использовать текущий привязанный объект.

                        // Если AdressTextBox привязан к userToUpdate.ClientEntity.Adress, изменения уже в userToUpdate.
                        // Если AdressTextBox привязан к userToUpdate.Adress, и вы хотите, чтобы это работало:
                        // userToUpdate.ClientEntity.Adress = userToUpdate.Adress; // <- (Если бы у User было свойство Adress)

                        // ИСПОЛЬЗУЕМ БЕЗОПАСНЫЙ ВАРИАНТ, который работает с привязкой в Account.xaml
                        // Вам НУЖНО убедиться, что XAML-привязка для Adress работает через ClientEntity.Adress

                        // Если XAML: <TextBox ... Text="{Binding ClientEntity.Adress, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"/>
                        // Тогда ничего дополнительно не нужно. Просто:
                        db.Entry(userToUpdate).State = EntityState.Modified;
                        db.Entry(userToUpdate.ClientEntity).State = EntityState.Modified;
                    }

                    // 3. Сохранение
                    db.SaveChanges();

                    MessageBox.Show("Данные аккаунта успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show($"Ошибка при обновлении базы данных: {ex.InnerException?.Message ?? ex.Message}", "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Непредвиденная ошибка при сохранении: {ex.Message}", "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // --------------------------------------------------------------------
        // ЛОГИКА НАВИГАЦИИ
        // --------------------------------------------------------------------

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
            // Возврат на главное окно (MainWindow)
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}