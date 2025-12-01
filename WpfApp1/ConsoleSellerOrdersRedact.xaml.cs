using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using WpfApp1.Models;

namespace WpfApp1
{
    // Реализуем INotifyPropertyChanged для обновления полей формы при поиске
    public partial class ConsoleSellerOrdersRedact : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Order> _ordersList;

        // Объект, к которому привязаны все поля формы
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

        // Основной конструктор, принимающий коллекцию заказов
        public ConsoleSellerOrdersRedact(ObservableCollection<Order> orders)
        {
            InitializeComponent();
            _ordersList = orders;

            // Устанавливаем DataContext, чтобы привязки XAML работали
            DataContext = this;

            // Инициализируем пустым объектом, чтобы привязка не "падала"
            CurrentOrder = new Order();
        }

        // Перегрузка для совместимости с XAML-дизайнером
        public ConsoleSellerOrdersRedact() : this(new ObservableCollection<Order>()) { }

        // Обработчик кнопки "Поиск"
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(SearchIdTextBox.Text, out int searchId))
            {
                Order? foundOrder = _ordersList.FirstOrDefault(o => o.Id == searchId);

                if (foundOrder != null)
                {
                    // Создаем КОПИЮ объекта. 
                    // Это важно, чтобы изменения не применялись к списку до нажатия "Подтвердить".
                    CurrentOrder = new Order
                    {
                        Id = foundOrder.Id,
                        ClientID = foundOrder.ClientID,
                        Delivery = foundOrder.Delivery,
                        ProductID = foundOrder.ProductID?.ToArray(), // Копируем массив
                        ServicesID = foundOrder.ServicesID?.ToArray(), // Копируем массив
                        Summ = foundOrder.Summ,
                        Status = foundOrder.Status
                    };

                    MessageBox.Show($"Заказ ID: {searchId} найден и загружен для редактирования.", "Успех");
                }
                else
                {
                    MessageBox.Show($"Заказ с ID: {searchId} не найден.", "Ошибка");
                    CurrentOrder = new Order(); // Очищаем форму
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректный ID заказа.", "Ошибка ввода");
            }
        }

        // Обработчик кнопки "Подтвердить"
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentOrder == null || CurrentOrder.Id == 0)
            {
                MessageBox.Show("Сначала найдите заказ для редактирования.", "Ошибка сохранения");
                return;
            }

            // 1. Ищем оригинальный заказ по ID в основной коллекции
            Order? originalOrder = _ordersList.FirstOrDefault(o => o.Id == CurrentOrder.Id);

            if (originalOrder != null)
            {
                // 2. Копируем все измененные данные обратно в оригинальный объект
                originalOrder.ClientID = CurrentOrder.ClientID;
                originalOrder.Delivery = CurrentOrder.Delivery;
                originalOrder.ProductID = CurrentOrder.ProductID; // Обновленный массив из ProductsString
                originalOrder.ServicesID = CurrentOrder.ServicesID; // Обновленный массив из ServicesString
                originalOrder.Summ = CurrentOrder.Summ;
                originalOrder.Status = CurrentOrder.Status;

                MessageBox.Show($"Данные заказа ID: {originalOrder.Id} успешно сохранены.", "Сохранение завершено");

                CurrentOrder = new Order(); // Очищаем форму после сохранения
            }
            else
            {
                MessageBox.Show($"Произошла ошибка: оригинальный заказ ID: {CurrentOrder.Id} не найден.", "Критическая ошибка");
            }
        }

        // Реализация INotifyPropertyChanged для обновления UI
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ConsoleSellerOrders consoleSellerOrders = new ConsoleSellerOrders();
            consoleSellerOrders.Show();
            this.Close();
        }
    }
}