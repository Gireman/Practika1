// EmployeeEntity.cs
using System.ComponentModel.DataAnnotations.Schema;
using WpfApp1.models;

namespace WpfApp1.Models
{
    [Table("employees")]
    public class EmployeeEntity
    {
        // Первичный ключ
        [Column("id")]
        public int Id { get; set; }

        // Внешний ключ к таблице 'users'. 
        // 👇 ИСПРАВЛЕНИЕ: ТОЧНОЕ ИМЯ СТОЛБЦА С #
        [Column("Id_user")]
        public int IdUser { get; set; }

        // Внешний ключ к таблице 'posts'.
        // 👇 ИСПРАВЛЕНИЕ: ТОЧНОЕ ИМЯ СТОЛБЦА С #
        [Column("Id_post")]
        public int IdPost { get; set; }

        // Зарплата.
        // 👇 ИСПРАВЛЕНИЕ: ТОЧНОЕ ИМЯ СТОЛБЦА С #
        [Column("Salary", TypeName = "decimal(10, 0)")]
        public decimal Salary { get; set; }

        // --- Навигационные свойства ---
        public User User { get; set; }
        public Post Post { get; set; }
    }
}