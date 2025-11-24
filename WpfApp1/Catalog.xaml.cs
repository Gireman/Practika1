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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Catalog.xaml
    /// </summary>
    public partial class Catalog : Window
    {
        public Catalog()
        {
            InitializeComponent();
        }

        private readonly SolidColorBrush DefaultColor = (SolidColorBrush)new BrushConverter().ConvertFromString("#100100");
        private readonly SolidColorBrush HoverColor = (SolidColorBrush)new BrushConverter().ConvertFromString("#85D5DC");

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

        private void MainButton_MouseEnter(object sender, MouseEventArgs e)
        {
            // Меняем цвет текста на цвет при наведении
            if (MainTextBlock != null)
            {
                MainTextBlock.Foreground = HoverColor;
            }
        }

        // Метод, который срабатывает при уходе мыши с кнопки
        private void MainButton_MouseLeave(object sender, MouseEventArgs e)
        {
            // Возвращаем цвет текста к исходному
            if (MainTextBlock != null)
            {
                MainTextBlock.Foreground = DefaultColor;
            }
        }

        private void Button_ClickMain(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void Button_ClickBasket(object sender, RoutedEventArgs e)
        {
            Basket basket = new Basket();
            basket.Show();
            this.Close();
        }
    }
}
