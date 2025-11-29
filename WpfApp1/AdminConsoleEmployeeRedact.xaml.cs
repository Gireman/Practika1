using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AdminConsoleEmployeeRedact.xaml
    /// </summary>
    public partial class AdminConsoleEmployeeRedact : Window
    {

        private void Button_ClickExit(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_ClickUsers(object sender, RoutedEventArgs e)
        {

        }

        public ObservableCollection<Order> Orders { get; set; }

        // --- Новый код для выбранного заказа ---
        private Order? _selectedOrder;
        public Order? SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                if (_selectedOrder != value)
                {
                    _selectedOrder = value;
                    OnPropertyChanged();
                }
            }
        }
        // --- Конец нового кода ---

        // Конструктор (ваш исправленный)
        public AdminConsoleEmployeeRedact()
        {
            InitializeComponent();
            // ... (Ваша инициализация коллекции Orders) ...
            DataContext = this;
        }

        // --- Реализация INotifyPropertyChanged ---
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // --- Обработчик для кнопки "Сохранить" ---
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOrder != null)
            {
                // При двусторонней привязке (Mode=TwoWay) изменения уже применены к объекту SelectedOrder
                // и отразились в коллекции Orders. Здесь просто сообщаем пользователю.
                MessageBox.Show($"Изменения для заказа ID: {SelectedOrder.Id} успешно применены.", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);

                // Здесь будет код для сохранения в базу данных, если вы ее используете.
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для сохранения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
