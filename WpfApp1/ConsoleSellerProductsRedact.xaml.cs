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
using WpfApp1.models;
using WpfApp1.Utilities;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для ConsoleSellerProductsRedact.xaml
    /// </summary>
    public partial class ConsoleSellerProductsRedact : Window, INotifyPropertyChanged
    {
        private ObservableCollection<ProductsInterface> _productsList;

        // Буферный объект, к которому привязаны поля формы
        private ProductsInterface? _currentProduct;
        public ProductsInterface? CurrentProduct
        {
            get { return _currentProduct; }
            set
            {
                _currentProduct = value;
                OnPropertyChanged(); // Уведомляет UI об изменении
            }
        }

        // Конструктор, принимающий коллекцию товаров
        public ConsoleSellerProductsRedact(ObservableCollection<ProductsInterface> products)
        {
            InitializeComponent();
            _productsList = products;
            DataContext = this;
            CurrentProduct = new ProductsInterface(); // Инициализируем пустой буфер
        }

        // Конструктор по умолчанию для XAML-дизайнера
        public ConsoleSellerProductsRedact() : this(new ObservableCollection<ProductsInterface>()) { }

        // --------------------------------------------------------------------
        // ЛОГИКА ПОИСКА
        // --------------------------------------------------------------------
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Предполагаем, что TextBox для ID назван "SearchIdTextBox" в XAML
            TextBox searchBox = (TextBox)FindName("SearchIdTextBox");
            if (searchBox == null || !int.TryParse(searchBox.Text, out int searchId))
            {
                MessageBox.Show("Пожалуйста, введите корректный ID товара.", "Ошибка ввода");
                return;
            }

            ProductsInterface? foundProduct = _productsList.FirstOrDefault(p => p.Id == searchId);

            if (foundProduct != null)
            {
                // Создаем КОПИЮ объекта (БУФЕР) для редактирования
                CurrentProduct = new ProductsInterface
                {
                    Id = foundProduct.Id,
                    Category = foundProduct.Category,
                    Product = foundProduct.Product,
                    Count = foundProduct.Count,
                    Storage = foundProduct.Storage,
                    Supplier = foundProduct.Supplier,
                    Price = foundProduct.Price
                };

                MessageBox.Show($"Товар ID: {searchId} найден и загружен для редактирования.", "Успех");
            }
            else
            {
                MessageBox.Show($"Товар с ID: {searchId} не найден.", "Ошибка");
                CurrentProduct = new ProductsInterface(); // Очищаем форму
            }
        }

        // --- СОЗДАНИЕ (CreateButton_Click) ---
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentProduct == null) return;
            if (!ValidateProduct(CurrentProduct)) return;

            ProductsInterface newProd = new ProductsInterface
            {
                Category = CurrentProduct.Category,
                Product = CurrentProduct.Product,
                Count = CurrentProduct.Count,
                Storage = CurrentProduct.Storage,
                Supplier = CurrentProduct.Supplier,
                Price = CurrentProduct.Price
            };

            int newId = _productsList.Any() ? _productsList.Max(p => p.Id) + 1 : 1;
            newProd.Id = newId;

            _productsList.Add(newProd);
            MessageBox.Show($"Товар ID: {newId} создан.", "Создано");

            CurrentProduct = new ProductsInterface();
            TextBox searchBox = (TextBox)FindName("SearchIdTextBox");
            if (searchBox != null) searchBox.Text = "";
        }

        // --------------------------------------------------------------------
        // ЛОГИКА СОХРАНЕНИЯ (РЕДАКТИРОВАНИЯ)
        // --------------------------------------------------------------------
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentProduct == null || CurrentProduct.Id == 0)
            {
                MessageBox.Show("Сначала найдите товар для редактирования.", "Ошибка сохранения");
                return;
            }

            ProductsInterface? originalProduct = _productsList.FirstOrDefault(p => p.Id == CurrentProduct.Id);

            if (originalProduct != null)
            {
                // Копируем изменения из буфера (CurrentProduct) обратно в оригинальный объект
                originalProduct.Category = CurrentProduct.Category;
                originalProduct.Product = CurrentProduct.Product;
                originalProduct.Count = CurrentProduct.Count;
                originalProduct.Storage = CurrentProduct.Storage;
                originalProduct.Supplier = CurrentProduct.Supplier;
                originalProduct.Price = CurrentProduct.Price;

                MessageBox.Show($"Данные товара ID: {originalProduct.Id} успешно сохранены (в памяти).", "Сохранение завершено");

                CurrentProduct = new ProductsInterface(); // Очистка формы после сохранения
            }
        }

        // --------------------------------------------------------------------
        // ЛОГИКА УДАЛЕНИЯ
        // --------------------------------------------------------------------
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Проверяем, загружен ли текущий товар
            if (CurrentProduct == null || CurrentProduct.Id == 0)
            {
                MessageBox.Show("Сначала найдите товар для удаления.", "Ошибка удаления");
                return;
            }

            // 2. Запрашиваем подтверждение
            MessageBoxResult result = MessageBox.Show(
                $"Вы уверены, что хотите удалить товар ID: {CurrentProduct.Id} ({CurrentProduct.Product})?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                // 3. Находим оригинальный объект в коллекции (_productsList) по ID
                ProductsInterface? productToDelete = _productsList.FirstOrDefault(p => p.Id == CurrentProduct.Id);

                if (productToDelete != null)
                {
                    // 4. Удаляем объект из ObservableCollection
                    _productsList.Remove(productToDelete);

                    // 5. Очищаем форму редактирования после удаления
                    CurrentProduct = new ProductsInterface();

                    MessageBox.Show("Товар успешно удален (из буферного массива).", "Удалено");
                }
                else
                {
                    MessageBox.Show($"Ошибка: Товар ID: {CurrentProduct.Id} не найден в списке.", "Ошибка");
                }
            }
        }

        // --- ВАЛИДАЦИЯ ---
        private bool ValidateProduct(ProductsInterface p)
        {
            if (string.IsNullOrWhiteSpace(p.Product))
            {
                MessageBox.Show("Название товара не может быть пустым.", "Ошибка");
                return false;
            }
            if (p.Price < 0 || p.Count < 0)
            {
                MessageBox.Show("Цена и количество не могут быть отрицательными.", "Ошибка");
                return false;
            }
            return true;
        }

        // Реализация INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // --------------------------------------------------------------------
        // МЕТОДЫ НАВИГАЦИИ (Уже существующие)
        // --------------------------------------------------------------------
        private void Button_ClickOrders(object sender, RoutedEventArgs e)
        {
            ConsoleSellerOrders orders = new ConsoleSellerOrders();
            orders.Show();
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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ConsoleSellerProducts products = new ConsoleSellerProducts();
            products.Show();
            this.Close();
        }
    }
}
