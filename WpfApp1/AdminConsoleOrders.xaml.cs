using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Логика взаимодействия для AdminConsoleOrders.xaml
    /// </summary>
    public partial class AdminConsoleOrders : Window
    {
        public AdminConsoleOrders()
        {
            InitializeComponent();
        }

        


    }

    public class Order
    {
        public string ID { get; set; }
        public string ClientID { get; set; }
        public string Products { get; set; }
        public object Delivery { get; set; } // Используем object для демонстрации NULL
        public string Services { get; set; }
        public int Sum { get; set; }
        public string Status { get; set; }
    }
}
