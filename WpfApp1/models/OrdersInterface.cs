using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class Order
    {
        // Все поля заменены на публичные свойства
        public int Id { get; set; }
        public int ClientID { get; set; }

        public int[]? ProductID { get; set; }
        public string Delivery { get; set; }
        public int[]? ServicesID { get; set; }
        public decimal Summ { get; set; }
        public bool Status { get; set; }

        // Вспомогательное свойство для привязки к полю Products (позволяет вводить ID через запятую)
        public string ProductsString
        {
            get => ProductID != null ? string.Join(", ", ProductID) : string.Empty;
            set
            {
                // Парсинг строки "1, 2, 5" обратно в массив int[]
                ProductID = value?.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(s => int.TryParse(s.Trim(), out int n) ? n : -1)
                                 .Where(n => n != -1)
                                 .ToArray();
            }
        }

        // Вспомогательное свойство для привязки к полю Services
        public string ServicesString
        {
            get => ServicesID != null ? string.Join(", ", ServicesID) : string.Empty;
            set
            {
                ServicesID = value?.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                  .Select(s => int.TryParse(s.Trim(), out int n) ? n : -1)
                                  .Where(n => n != -1)
                                  .ToArray();
            }
        }
    }
}