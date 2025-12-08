using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.models
{
    [Table("clients")] // Имя вашей таблицы с клиентами
    public class Client
    {
        // Первичный ключ и Внешний ключ к таблице users (One-to-One)
        // В EF Core принято использовать ID как PK/FK в отношении 1:1, 
        // если нет отдельного поля для FK. Здесь я использую IdUser.

        // 1. НОВОЕ: Первичный ключ с автоинкрементом (Id)
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Id_user")] // Поле, которое связывает с таблицей users
        public int IdUser { get; set; }

        [Column("Adress")] // Поле, содержащее адрес
        public string? Adress { get; set; } = string.Empty;

        // Навигационное свойство для обратной связи к пользователю
        public User User { get; set; } = null!;
    }
}
