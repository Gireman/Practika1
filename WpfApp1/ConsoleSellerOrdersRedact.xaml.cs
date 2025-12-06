using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;
using WpfApp1.Utilities;

namespace WpfApp1
{
    // Реализуем INotifyPropertyChanged для обновления полей формы при поиске
    // Класс ConsoleSellerOrdersRedact, реализующий INotifyPropertyChanged для обновления UI
    public partial class ConsoleSellerOrdersRedact : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Order> _ordersList;

        // Буферный объект, который будет отображаться в TextBoxes (поля заказа)
        private Order? _currentOrder;
        public Order? CurrentOrder
        {
            get { return _currentOrder; }
            set
            {
                _currentOrder = value;
                OnPropertyChanged(); // Уведомляет UI об изменении
            }
        }

        // Коллекция для DataGrid: хранит список товаров с количеством (OrderItem)
        // Используем ObservableCollection<OrderItem>, чтобы DataGrid обновлялся при добавлении/удалении
        private ObservableCollection<OrderItem> _editableOrderItems = new ObservableCollection<OrderItem>();
        public ObservableCollection<OrderItem> EditableOrderItems
        {
            get { return _editableOrderItems; }
            set
            {
                _editableOrderItems = value;
                OnPropertyChanged();
            }
        }

        // Конструктор, принимающий коллекцию заказов
        public ConsoleSellerOrdersRedact(ObservableCollection<Order> orders)
        {
            InitializeComponent();
            _ordersList = orders;
            DataContext = this;

            // Инициализируем буферный заказ и пустую коллекцию товаров
            CurrentOrder = new Order();
            EditableOrderItems = new ObservableCollection<OrderItem>();
        }

        // Конструктор по умолчанию для XAML-дизайнера
        public ConsoleSellerOrdersRedact() : this(new ObservableCollection<Order>()) { }

        // Логика кнопки "Поиск"
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Находим поле поиска по ID (Name="SearchIdTextBox" в XAML)
            TextBox searchBox = (TextBox)FindName("SearchIdTextBox");
            if (searchBox == null || !int.TryParse(searchBox.Text, out int searchId))
            {
                MessageBox.Show("Пожалуйста, введите корректный ID заказа.", "Ошибка ввода");
                return;
            }

            Order? foundOrder = _ordersList.FirstOrDefault(o => o.Id == searchId);

            if (foundOrder != null)
            {
                // 1. Создаем КОПИЮ объекта (БУФЕР) для редактирования
                // Это предотвращает изменение оригинального объекта до нажатия "Сохранить"
                CurrentOrder = new Order
                {
                    Id = foundOrder.Id,
                    ClientID = foundOrder.ClientID,
                    EmployeeID = foundOrder.EmployeeID,
                    ServicesID = foundOrder.ServicesID?.ToArray(), // Копируем массив
                    Summ = foundOrder.Summ,
                    Status = foundOrder.Status,
                    // OrderItems не копируем в буфер CurrentOrder, а сразу загружаем в ObservableCollection
                };

                // 2. Загружаем буферный список OrderItems в коллекцию для DataGrid (EditableOrderItems)
                // Обязательно делаем глубокое копирование элементов OrderItem!
                var copiedItems = foundOrder.OrderItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList();

                EditableOrderItems = new ObservableCollection<OrderItem>(copiedItems);

                MessageBox.Show($"Заказ ID: {searchId} найден и загружен для редактирования.", "Успех");
            }
            else
            {
                MessageBox.Show($"Заказ с ID: {searchId} не найден.", "Ошибка");
                CurrentOrder = new Order(); // Очищаем поля заказа
                EditableOrderItems.Clear(); // Очищаем список товаров
            }
        }

        // Логика кнопки "Сохранить/Подтвердить"
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentOrder == null || CurrentOrder.Id == 0)
            {
                MessageBox.Show("Сначала найдите заказ для редактирования.", "Ошибка сохранения");
                return;
            }

            Order? originalOrder = _ordersList.FirstOrDefault(o => o.Id == CurrentOrder.Id);

            if (originalOrder != null)
            {
                // 1. Копируем изменения из буфера (CurrentOrder) обратно в оригинальный объект
                originalOrder.ClientID = CurrentOrder.ClientID;
                originalOrder.EmployeeID = CurrentOrder.EmployeeID;
                originalOrder.ServicesID = CurrentOrder.ServicesID;
                originalOrder.Summ = CurrentOrder.Summ;
                originalOrder.Status = CurrentOrder.Status;

                // 2. КЛЮЧЕВОЕ ИЗМЕНЕНИЕ: Обновляем список товаров в оригинальном объекте
                // EditableOrderItems содержит все изменения из DataGrid
                originalOrder.OrderItems = EditableOrderItems.ToList();

                MessageBox.Show($"Данные заказа ID: {originalOrder.Id} успешно сохранены (в памяти).", "Сохранение завершено");

                // Очистка формы после сохранения
                CurrentOrder = new Order();
                EditableOrderItems.Clear();
            }
        }

        // Реализация INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Методы навигации (оставляем без изменений)
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ConsoleSellerOrders consoleSellerOrders = new ConsoleSellerOrders();
            consoleSellerOrders.Show();
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

        private void Button_ClickProducts(object sender, RoutedEventArgs e)
        {
            ConsoleSellerProducts consoleSellerProducts = new ConsoleSellerProducts();
            consoleSellerProducts.Show();
            this.Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Проверяем, есть ли текущий загруженный заказ в буфере и имеет ли он ID
            if (CurrentOrder == null || CurrentOrder.Id == 0)
            {
                MessageBox.Show("Сначала найдите заказ для удаления.", "Ошибка удаления");
                return;
            }

            // 2. Запрашиваем подтверждение
            MessageBoxResult result = MessageBox.Show(
                $"Вы уверены, что хотите удалить заказ ID: {CurrentOrder.Id}?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                // 3. Находим оригинальный объект в коллекции (_ordersList) по ID
                Order? orderToDelete = _ordersList.FirstOrDefault(o => o.Id == CurrentOrder.Id);

                if (orderToDelete != null)
                {
                    // 4. Удаляем объект из ObservableCollection
                    // ObservableCollection автоматически уведомит DataGrid (в ConsoleSellerOrders) об удалении
                    _ordersList.Remove(orderToDelete);

                    // 5. Очищаем форму редактирования после удаления
                    CurrentOrder = new Order(); // Очистка всех полей
                    EditableOrderItems.Clear(); // Очистка списка товаров в DataGrid

                    MessageBox.Show("Заказ успешно удален (из буферного массива).", "Удалено");
                }
                else
                {
                    MessageBox.Show($"Ошибка: Заказ ID: {CurrentOrder.Id} не найден в списке.", "Ошибка");
                }
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Проверка: Убедимся, что буферный объект CurrentOrder существует.
            if (CurrentOrder == null)
            {
                MessageBox.Show("Ошибка: Буферный объект заказа отсутствует.", "Ошибка создания");
                return;
            }

            // Проверка заполнения основных полей
            // Проверяем ClientID, EmployeeID и наличие хотя бы одного товара
            if (CurrentOrder.ClientID == 0 || CurrentOrder.EmployeeID == 0 || EditableOrderItems.Count == 0)
            {
                MessageBox.Show("Пожалуйста, заполните поля IDClient, EmployeeID и добавьте хотя бы один товар.", "Ошибка ввода");
                return;
            }

            // Проверяем, что все OrderItem имеют ProductId и Quantity > 0
            if (EditableOrderItems.Any(item => item.ProductId == 0 || item.Quantity <= 0))
            {
                MessageBox.Show("Ошибка: Все товары должны иметь корректный ID товара и количество больше 0.", "Ошибка ввода");
                return;
            }


            // 2. Генерация нового ID (имитация работы БД)
            // Находим максимальный ID в текущей коллекции и добавляем 1.
            int newId = _ordersList.Any() ? _ordersList.Max(o => o.Id) + 1 : 1;

            // 3. Создание нового объекта Order с скопированными данными
            // Создаем НОВЫЙ объект для добавления в список, чтобы не добавлять буферный объект.
            var newOrder = new Order
            {
                Id = newId,
                ClientID = CurrentOrder.ClientID,
                EmployeeID = CurrentOrder.EmployeeID,
                ServicesID = CurrentOrder.ServicesID?.ToArray(), // Копируем массив услуг
                Summ = CurrentOrder.Summ,
                Status = CurrentOrder.Status,

                // КЛЮЧЕВОЕ ИЗМЕНЕНИЕ: Глубокое копирование OrderItems из DataGrid
                OrderItems = EditableOrderItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList()
            };

            // 4. Добавление в ObservableCollection
            // ObservableCollection автоматически уведомит DataGrid в ConsoleSellerOrders о новом элементе.
            _ordersList.Add(newOrder);

            MessageBox.Show($"Новый заказ ID: {newId} успешно создан и добавлен (в памяти).", "Создание завершено");

            // 5. Очистка формы после создания
            CurrentOrder = new Order(); // Создаем новый пустой буфер
            EditableOrderItems.Clear(); // Очищаем DataGrid
        }
    }
}