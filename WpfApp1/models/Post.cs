using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;
using System.Collections.Generic; // 👈 УБЕДИТЕСЬ, ЧТО ОН ЕСТЬ!

namespace WpfApp1.Models
{
    [Table("posts")]
    public class Post : INotifyPropertyChanged
    {
        private int _id;
        private string _postName = string.Empty;

        [Column("Id")]
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        [Column("Post")]
        public string PostName
        {
            get => _postName;
            set { _postName = value; OnPropertyChanged(); }
        }

        // ВОССТАНОВЛЕНО: Навигационное свойство для связи "один ко многим"
        public ICollection<EmployeeEntity>? EmployeeEntities { get; set; }

        // Метод для отображения в ComboBox
        public override string ToString() => PostName;

        // Реализация INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
