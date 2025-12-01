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
using WpfApp1.models;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для ConsoleSellerProducts.xaml
    /// </summary>
    public partial class ConsoleSellerProducts : Window
    {
        // Объявляем коллекцию товаров
        public ObservableCollection<ProductsInterface> Products { get; set; }

        public ConsoleSellerProducts()
        {
            InitializeComponent();

            // Инициализация тестовыми данными
            Products = new ObservableCollection<ProductsInterface>()
            {
                new ProductsInterface
                {
                    Id = 1, Category = 101, Product = "Laptop Model X",
                    Count = 50, Storage = 1, Supplier = 5, Price = 750.00m
                },
                new ProductsInterface
                {
                    Id = 2, Category = 102, Product = "Monitor 27-inch",
                    Count = 120, Storage = 2, Supplier = 6, Price = 250.50m
                },
                new ProductsInterface
                {
                    Id = 3, Category = 101, Product = "Gaming Mouse",
                    Count = 300, Storage = 1, Supplier = 5, Price = 45.00m
                }
            };

            DataContext = this;
        }

        private void Button_ClickOrders(object sender, RoutedEventArgs e)
        {
            ConsoleSellerOrders orders = new ConsoleSellerOrders();
            orders.Show();
            this.Close();
        }

        private void Button_ClickExit(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_ClickRedactProducts(object sender, RoutedEventArgs e)
        {
            // Передаем коллекцию Products в конструктор окна редактирования
            ConsoleSellerProductsRedact productsRedact = new ConsoleSellerProductsRedact(Products);
            productsRedact.Show();
            this.Close();
        }

        private void Button_ClickCreateProducts(object sender, RoutedEventArgs e)
        {
            // Логика создания нового товара (если есть)
        }
    }
}
