// User.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp1.Models
{
    // Указываем, что этот класс соответствует таблице 'users'
    [Table("users")]
    public class User
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("Id_role")]
        public int IdRole { get; set; } 

        [Column("Name")]
        public string Name { get; set; }

        [Column("Surname")]
        public string Surname { get; set; }

        [Column("Patronymic")]
        public string? Patronymic { get; set; }

        [Column("Login")]
        public string Login { get; set; }

        [Column("Password")]
        public string Password { get; set; }

        [Column("Phone")]
        public string Phone { get; set; }

        [Column("Email")]
        public string? Email { get; set; }

        [Column("Birthday")]
        public DateTime Birthday { get; set; }

        // Навигационное свойство для связи "один к одному"
        public EmployeeEntity EmployeeEntity { get; set; }
    }
}