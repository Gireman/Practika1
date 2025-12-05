using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using WpfApp1.Models;

namespace WpfApp1.Utilities
{
    // Статический класс для управления глобальной корзиной
    public static class BasketManager
    {
        // ObservableCollection автоматически уведомит UI об изменениях
        public static ObservableCollection<BasketItem> Items { get; private set; } = new ObservableCollection<BasketItem>();

        // Метод для добавления товара
        public static void AddItem(int productId, string name, decimal price, string imagePath = "")
        {
            // 1. Ищем, есть ли уже этот товар в корзине
            var existingItem = Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                // 2. Если товар есть, увеличиваем количество на 1
                existingItem.Quantity++;
            }
            else
            {
                // 3. Если товара нет, добавляем новый с количеством 1
                var newItem = new BasketItem
                {
                    ProductId = productId,
                    Name = name,
                    Price = price,
                    Quantity = 1,
                    ImagePath = imagePath
                };
                Items.Add(newItem);
            }
        }

        // Метод для удаления товара из корзины (если нужно)
        public static void RemoveItem(BasketItem item)
        {
            Items.Remove(item);
        }

        // Метод для очистки корзины
        public static void ClearBasket()
        {
            Items.Clear();
        }
    }
}
