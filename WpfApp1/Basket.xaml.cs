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
    /// Логика взаимодействия для Basket.xaml
    /// </summary>
    public partial class Basket : Window
    {
        public Basket()
        {
            InitializeComponent();
        }

        private readonly SolidColorBrush DefaultColor = (SolidColorBrush)new BrushConverter().ConvertFromString("#100100");
        private readonly SolidColorBrush HoverColor = (SolidColorBrush)new BrushConverter().ConvertFromString("#85D5DC");

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

        private void Button_ClickMain(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_ClickCatalog(object sender, RoutedEventArgs e)
        {
            Catalog catalog = new Catalog();    
            catalog.Show();
            this.Close();
        }
    }
}
