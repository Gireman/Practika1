using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp1.Models
{
    // Класс для товара в корзине (включает логику уведомления об изменениях)
    public class BasketItem : INotifyPropertyChanged
    {
        private int _productId; // ID товара (для будущей привязки к БД)
        private string _name = string.Empty;
        private decimal _price;
        private int _quantity;
        private string _imagePath = string.Empty; // Путь к изображению

        public int ProductId
        {
            get => _productId;
            set
            {
                _productId = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                // Количество не может быть меньше 1 (для корзины)
                if (value >= 1)
                {
                    _quantity = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalItemPrice)); // Обновляем общую стоимость при изменении количества
                }
            }
        }

        // Вычисляемое свойство для отображения общей стоимости этого товара
        public decimal TotalItemPrice => Price * Quantity;

        // Свойство для пути к изображению (пока пустая форма)
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        // Реализация INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
