using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.models
{
    public class ProductsInterface
    {
        public int Id { get; set; }
        public int Category { get; set; }
        public string Product { get; set; }
        public int Count { get; set; }
        public int Storage { get; set; }
        public int Supplier { get; set; }
        public decimal Price { get; set; }
    }
}
