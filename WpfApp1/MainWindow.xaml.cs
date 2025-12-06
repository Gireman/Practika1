using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Utilities;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }


        private readonly SolidColorBrush DefaultColor = (SolidColorBrush)new BrushConverter().ConvertFromString("#100100");
        private readonly SolidColorBrush HoverColor = (SolidColorBrush)new BrushConverter().ConvertFromString("#85D5DC");


        private void CatalogButton_MouseEnter(object sender, MouseEventArgs e)
        {
            // Меняем цвет текста на цвет при наведении
            if (CatalogTextBlock != null)
            {
                CatalogTextBlock.Foreground = HoverColor;
            }
        }

        private void Acc_Click(object sender, RoutedEventArgs e)
        {
            if (AuthManager.IsLoggedIn)
            {
                // Пользователь вошел. Открываем окно аккаунта.
                Account account = new Account();
                account.Show();
                this.Close();
            }
            else
            {
                // Пользователь не вошел. Открываем окно входа.
                Enter enter = new Enter();
                enter.Show();
                this.Close();
            }
        }

        // Метод, который срабатывает при уходе мыши с кнопки
        private void CatalogButton_MouseLeave(object sender, MouseEventArgs e)
        {
            // Возвращаем цвет текста к исходному
            if (CatalogTextBlock != null)
            {
                CatalogTextBlock.Foreground = DefaultColor;
            }
        }

        private void BasketButton_MouseEnter(object sender, MouseEventArgs e)
        {
            // Меняем цвет текста на цвет при наведении
            if (BasketTextBlock != null)
            {
                BasketTextBlock.Foreground = HoverColor;
            }
        }

        // Метод, который срабатывает при уходе мыши с кнопки
        private void BasketButton_MouseLeave(object sender, MouseEventArgs e)
        {
            // Возвращаем цвет текста к исходному
            if (BasketTextBlock != null)
            {
                BasketTextBlock.Foreground = DefaultColor;
            }
        }

        
        private void Button_ClickCatalog(object sender, RoutedEventArgs e)
        {
            Catalog catalog = new Catalog();
            catalog.Show();
            this.Close();
        }

        private void Button_ClickBasket(object sender, RoutedEventArgs e)
        {
            Basket basket = new Basket();
            basket.Show();
            this.Close();
        }

        private void Button_ClickAdmin(object sender, RoutedEventArgs e)
        {
            AdminConsole console = new AdminConsole();
            console.Show();
            this.Close();
        }

        private void Button_ClickSeller(object sender, RoutedEventArgs e)
        {
            ConsoleSeller console = new ConsoleSeller();
            console.Show();
            this.Close();
        }

        /*private void Acc_Click(object sender, RoutedEventArgs e)
        {
            Enter enter = new Enter();
            enter.Show();
            this.Close();
        }*/

        private void Button_ClickReg(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            this.Close();
        }

        private void Button_Click22(object sender, RoutedEventArgs e)
        {
            // Добавление второго товара в корзину
            BasketManager.AddItem(
                productId: 102, // Уникальный ID товара
                name: "Xiaomi Redmi Poko-X4 5G ProMax Audi Chicken-MacNaggets OldSpice M3-Competition UltraSE с 18-ью камерами",
                price: 300000m
            );
            MessageBox.Show("Товар 'Xiaomi Redmi Poko-X4 5G ProMax Audi Chicken-MacNaggets OldSpice M3-Competition UltraSE с 18-ью камерами' добавлен в корзину!", "Корзина");
        }

        private void Button_Click11(object sender, RoutedEventArgs e)
        {
            // Добавление первого товара в корзину
            BasketManager.AddItem(
                productId: 101, // Уникальный ID товара
                name: "Amd GeForce RX 10060 TI Rog Strix",
                price: 150000m // Цена в формате decimal
                               // ImagePath по умолчанию пустой
            );
            MessageBox.Show("Товар 'Amd GeForce RX 10060 TI Rog Strix' добавлен в корзину!", "Корзина");
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            // Добавление первого товара в корзину
            BasketManager.AddItem(
                productId: 103, // Уникальный ID товара
                name: "Svastung",
                price: 15000m // Цена в формате decimal
                               // ImagePath по умолчанию пустой
            );
            MessageBox.Show("Товар 'Svastung' добавлен в корзину!", "Корзина");
        }
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            // Добавление первого товара в корзину
            BasketManager.AddItem(
                productId: 101, // Уникальный ID товара
                name: "Amd GeForce RX 10060 TI Rog Strix",
                price: 150000m // Цена в формате decimal
                               // ImagePath по умолчанию пустой
            );
            MessageBox.Show("Товар 'Amd GeForce RX 10060 TI Rog Strix' добавлен в корзину!", "Корзина");
        }
        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            // Добавление первого товара в корзину
            BasketManager.AddItem(
                productId: 101, // Уникальный ID товара
                name: "Amd GeForce RX 10060 TI Rog Strix",
                price: 150000m // Цена в формате decimal
                               // ImagePath по умолчанию пустой
            );
            MessageBox.Show("Товар 'Amd GeForce RX 10060 TI Rog Strix' добавлен в корзину!", "Корзина");
        }
        private void Button_Click4(object sender, RoutedEventArgs e)
        {
            // Добавление первого товара в корзину
            BasketManager.AddItem(
                productId: 101, // Уникальный ID товара
                name: "Amd GeForce RX 10060 TI Rog Strix",
                price: 150000m // Цена в формате decimal
                               // ImagePath по умолчанию пустой
            );
            MessageBox.Show("Товар 'Amd GeForce RX 10060 TI Rog Strix' добавлен в корзину!", "Корзина");
        }
        private void Button_Click5(object sender, RoutedEventArgs e)
        {
            // Добавление первого товара в корзину
            BasketManager.AddItem(
                productId: 101, // Уникальный ID товара
                name: "Amd GeForce RX 10060 TI Rog Strix",
                price: 150000m // Цена в формате decimal
                               // ImagePath по умолчанию пустой
            );
            MessageBox.Show("Товар 'Amd GeForce RX 10060 TI Rog Strix' добавлен в корзину!", "Корзина");
        }
    }
}