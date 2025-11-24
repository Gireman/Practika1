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
        private int currentValue = 1;

        public Basket()
        {
            InitializeComponent();

            UpdateTextBlock();
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

        private void UpdateTextBlock()
        {
            // Проверяем, существует ли TextBlock, прежде чем менять его
            if (CountTextBlock != null)
            {
                CountTextBlock.Text = $"{currentValue}";
            }
        }

            private void IncrementButton_Click(object sender, RoutedEventArgs e)
        {
            // Увеличиваем значение
            if(currentValue >= 99)
            {
                currentValue = 99;
            }
            else
            {
                currentValue++;
            }

            // Обновляем текст в TextBlock
            UpdateTextBlock();
        }

        private void DecrementButton_Click(object sender, RoutedEventArgs e)
        {
            // Уменьшаем значение
            if (currentValue <= 0)
            {
                currentValue = 0;
            }
            else
            {
                currentValue--;
            }

            // Обновляем текст в TextBlock
            UpdateTextBlock();
        }
    }
}
