using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class Users
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Patronymic { get; set; }
        public required string Login { get; set; }
        public required string Password { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public required string Birthday { get; set; }
        public required string Adress { get; set; }
    }
}