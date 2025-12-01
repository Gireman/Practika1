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
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для ConsoleSellerOrders.xaml
    /// </summary>
    public partial class ConsoleSellerOrders : Window
    {
        public ObservableCollection<Order> Orders { get; set; }

        public ConsoleSellerOrders()
        {
            InitializeComponent();

            Orders = new ObservableCollection<Order>()
            {
                new Order
                {
                    Id = 1,
                    ClientID = 1,
                    Delivery = "Adress1",
                    ProductID = [1],
                    ServicesID = [1],
                    Summ = 50000,
                    Status = true
                },
                new Order
                {
                    Id = 2,
                    ClientID = 3,
                    Delivery = "Adress2",
                    ProductID = [1,2],
                    ServicesID = [],
                    Summ = 50000,
                    Status = false
                }
            };

            DataContext = this;

        }
        private void Button_ClickProducts(object sender, RoutedEventArgs e)
        {

        }

        // В файле ConsoleSellerOrders.xaml.cs
        private void Button_ClickRedactOrders(object sender, RoutedEventArgs e)
        {
            // !!! КЛЮЧЕВОЕ ИЗМЕНЕНИЕ: передаем Orders в конструктор нового окна
            ConsoleSellerOrdersRedact consoleSellerOrdersRedact = new ConsoleSellerOrdersRedact(Orders);
            consoleSellerOrdersRedact.Show();
            this.Close();
        }
    }
}
