using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    // НОВАЯ МОДЕЛЬ: Товар с количеством
    public class OrderItem : INotifyPropertyChanged
    {
        private int _productId;
        private int _quantity;

        // ID товара, который будет записываться в БД
        public int ProductId
        {
            get => _productId;
            set
            {
                if (_productId != value)
                {
                    _productId = value;
                    OnPropertyChanged();
                }
            }
        }

        // Количество этого товара
        public int Quantity
        {
            get => _quantity;
            set
            {
                // Простая валидация: количество должно быть положительным
                if (_quantity != value && value >= 0)
                {
                    _quantity = value;
                    OnPropertyChanged();
                }
            }
        }

        // Реализация INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    // Класс Order: Обновлен для использования OrderItem
    public class Order
    {
        public int Id { get; set; }
        public int ClientID { get; set; }
        // !!! УДАЛЯЕМ int[]? ProductID

        // !!! НОВОЕ СВОЙСТВО: Коллекция товаров с количеством
        // Используем List<OrderItem> для буфера и последующего сохранения
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public int EmployeeID { get; set; }
        public int[]? ServicesID { get; set; }
        public decimal Summ { get; set; }
        public bool Status { get; set; }

        // Вспомогательное свойство ProductsString удалено или модифицировано: 
        // Теперь оно отображает суммарное количество уникальных товаров для DataGrid
        public string ProductsDisplay
        {
            get
            {
                // 1. Проверка на пустую коллекцию
                if (OrderItems == null || OrderItems.Count == 0)
                    return "Нет товаров";

                // 2. Создаем список строк, где каждая строка описывает один OrderItem
                var itemsDescription = OrderItems
                    // Для каждого элемента (item) создаем строку, например: "ID 1: 2 шт"
                    .Select(item => $"ID {item.ProductId}: {item.Quantity} шт")
                    .ToList();

                // 3. Объединяем все эти строки через разделитель (запятая с пробелом)
                // В результате получается: "ID 1: 2 шт, ID 5: 1 шт, ID 7: 5 шт"
                return string.Join(", ", itemsDescription);
            }
        }

        // Оставим ServicesString, т.к. ServicesID не изменилось
        public string ServicesString
        {
            get => ServicesID != null ? string.Join(", ", ServicesID) : string.Empty;
            set
            {
                ServicesID = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                  .Select(s => int.TryParse(s, out int id) ? id : 0)
                                  .Where(id => id > 0)
                                  .ToArray();
            }
        }

        public string ServicesDisplay => ServicesString;
    }
}