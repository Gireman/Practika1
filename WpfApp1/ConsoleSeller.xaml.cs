using System;
using System.Collections.Generic;
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
using WpfApp1.Utilities;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для ConsoleSeller.xaml
    /// </summary>
    public partial class ConsoleSeller : Window
    {
        public ConsoleSeller()
        {
            InitializeComponent();
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

        private void Button_ClickOrders(object sender, RoutedEventArgs e)
        {
            ConsoleSellerOrders consoleSellerOrders = new ConsoleSellerOrders();
            consoleSellerOrders.Show();
            this.Close();
        }

        private void Button_ClickProducts(object sender, RoutedEventArgs e)
        {
            ConsoleSellerProducts consoleSellerProducts = new ConsoleSellerProducts();
            consoleSellerProducts.Show();
            this.Close();
        }
    }
}
