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
    }
}