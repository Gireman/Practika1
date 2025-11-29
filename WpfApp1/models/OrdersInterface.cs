using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    // Реализуем интерфейс INotifyPropertyChanged
    public class Order : INotifyPropertyChanged
    {
        // Приватные поля для хранения данных
        private int _id;
        private int _clientId;
        private int[]? _productID;
        private bool _delivery;
        private int[]? _servicesID;
        private decimal _summ;
        private bool _status;

        // Публичные свойства с логикой уведомления
        public int Id { get => _id; set { if (_id != value) { _id = value; OnPropertyChanged(); } } }
        public int ClientID { get => _clientId; set { if (_clientId != value) { _clientId = value; OnPropertyChanged(); } } }

        public int[]? ProductID
        {
            get => _productID;
            set
            {
                if (_productID != value)
                {
                    _productID = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ProductsDisplay)); // Обновляем и отображаемое свойство
                }
            }
        }

        public bool Delivery { get => _delivery; set { if (_delivery != value) { _delivery = value; OnPropertyChanged(); } } }

        public int[]? ServicesID
        {
            get => _servicesID;
            set
            {
                if (_servicesID != value)
                {
                    _servicesID = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ServicesDisplay)); // Обновляем и отображаемое свойство
                }
            }
        }

        public decimal Summ { get => _summ; set { if (_summ != value) { _summ = value; OnPropertyChanged(); } } }
        public bool Status { get => _status; set { if (_status != value) { _status = value; OnPropertyChanged(); } } }

        // Вспомогательные свойства для отображения массивов (как вы реализовали ранее)
        public string ProductsDisplay => ProductID != null ? string.Join(", ", ProductID) : string.Empty;
        public string ServicesDisplay => ServicesID != null ? string.Join(", ", ServicesID) : string.Empty;

        // Реализация INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
