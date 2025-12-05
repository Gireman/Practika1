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
using WpfApp1.Models;
using WpfApp1.Utilities;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Basket.xaml
    /// </summary>
    public partial class Basket : Window
    {
        // Удалите private int currentValue = 1;

        public Basket()
        {
            InitializeComponent();

            // Привязываем DataContext всего окна к статическому BasketManager
            this.DataContext = BasketManager.Items;

            // Запускаем метод для обновления видимости
            UpdateBasketDisplay();

            // Подписываемся на событие изменения коллекции, чтобы обновлять видимость
            BasketManager.Items.CollectionChanged += (s, e) => UpdateBasketDisplay();

            // Удалите вызов UpdateTextBlock();
        }

        // Удалите DefaultColor и HoverColor, если они не используются для других элементов

        private readonly SolidColorBrush DefaultColor = (SolidColorBrush)new BrushConverter().ConvertFromString("#100100");
        private readonly SolidColorBrush HoverColor = (SolidColorBrush)new BrushConverter().ConvertFromString("#85D5DC");

        // ... все методы MainButton_MouseEnter/Leave, CatalogButton_MouseEnter/Leave ...

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


        // Удалите методы IncrementButton_Click, DecrementButton_Click, UpdateTextBlock()

        // Новый метод для управления видимостью
        private void UpdateBasketDisplay()
        {
            // Проверяем, есть ли товары в корзине
            bool hasItems = BasketManager.Items.Any();

            // Показываем/скрываем ListBox
            if (BasketListBox != null)
            {
                BasketListBox.Visibility = hasItems ? Visibility.Visible : Visibility.Collapsed;
            }

            // Показываем/скрываем текст "Вы ничего не покупаете"
            if (EmptyBasketTextBlock != null)
            {
                EmptyBasketTextBlock.Visibility = hasItems ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        // Новый обработчик для увеличения количества товара
        private void IncrementItemButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем кнопку, которая вызвала событие
            if (sender is Button button)
            {
                // Находим DataContext (BasketItem) кнопки. 
                // Button находится внутри DataTemplate, поэтому его DataContext - это BasketItem.
                if (button.DataContext is BasketItem item)
                {
                    // Увеличиваем количество. Свойство Quantity само обновит TotalItemPrice и UI.
                    item.Quantity++;
                }
            }
        }

        // Новый обработчик для уменьшения количества товара
        private void DecrementItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.DataContext is BasketItem item)
                {
                    if (item.Quantity > 1)
                    {
                        // Уменьшаем количество
                        item.Quantity--;
                    }
                    else
                    {
                        // Если количество стало 1 и пользователь нажимает "минус", удаляем товар
                        MessageBoxResult result = MessageBox.Show(
                            $"Удалить {item.Name} из корзины?",
                            "Подтверждение",
                            MessageBoxButton.YesNo);

                        if (result == MessageBoxResult.Yes)
                        {
                            BasketManager.RemoveItem(item);
                        }
                    }
                }
            }
        }
    }
}
