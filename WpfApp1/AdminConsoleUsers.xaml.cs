using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfApp1.Utilities;
using Microsoft.EntityFrameworkCore; // Для Include()

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AdminConsoleUsers.xaml
    /// </summary>
    public partial class AdminConsoleUsers : Window
    {
        public ObservableCollection<Users> User { get; set; }



        public AdminConsoleUsers()
        {
            InitializeComponent();

            // Инициализация коллекции
            User = new ObservableCollection<Users>();

            // Загрузка данных из БД
            LoadUsersData(); // <-- Вызываем новый метод

            DataContext = this;
        }

        private void LoadUsersData()
        {
            try
            {
                using (var db = new ApplicationContext())
                {
                    // 1. Запрос к БД: Загружаем User и связанные с ним данные Client
                    // Для корректной работы этого запроса нужно, чтобы в ApplicationContext
                    // был определен DbSet<Client>, а в моделях User и Client - навигационные свойства.
                    var usersWithClients = db.Users
                        // Предполагаем, что Client связан с User отношением One-to-One
                        // и что навигационное свойство называется ClientEntity
                        // Если у вас нет явного DbSet<Client>, то нужно использовать JOIN
                        .Include(u => u.ClientEntity) // <- Предполагаемое имя навигационного свойства
                        .Where(u => u.IdRole == 2)
                        .ToList();

                    // 2. Проекция (маппинг) данных в вашу отображаемую модель Users
                    var userData = usersWithClients.Select(u => new Users // Users - это ваша модель для UI
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Surname = u.Surname,
                        Patronymic = u.Patronymic ?? string.Empty,
                        Login = u.Login,
                        Password = u.Password,
                        Phone = u.Phone,
                        Email = u.Email ?? string.Empty,
                        Birthday = u.Birthday,

                        // Получаем Adress из связанной таблицы Client
                        // Проверяем, что ClientEntity не null
                        Adress = u.ClientEntity.Adress ?? string.Empty // <-- САМЫЙ ВАЖНЫЙ МОМЕНТ
                    }).ToList();

                    // 3. Очищаем старые данные и добавляем новые в ObservableCollection
                    User.Clear();
                    foreach (var u in userData)
                    {
                        User.Add(u);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных пользователей: {ex.Message}", "Ошибка БД");
            }
        }

        private void Button_ClickEmployee(object sender, RoutedEventArgs e)
        {
            AdminConsoleEmployee adminConsoleEmployee = new AdminConsoleEmployee();
            adminConsoleEmployee.Show();
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

        private void Button_ClickRedact(object sender, RoutedEventArgs e)
        {
            // !!! Ключевое изменение: передаем коллекцию User в конструктор
            AdminConsoleUsersRedact redactWindow = new AdminConsoleUsersRedact(User);
            redactWindow.Show();
            this.Close();
        }
    }
}
