using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.models
{
    class AppDbContext : DbContext 
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User_E> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public static string ConnectionString { get; } =
     "Server=tompsons.beget.tech;Database=tompsons_stud19;Uid=tompsons_stud19;Pwd=Hrizalit1;Port=3306;SslMode=Preferred;CharSet=utf8;ConnectionTimeout=30;";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer
            (
            // !!! ЗАМЕНИТЕ ВАШИМИ ЛОГИНОМ И ПАРОЛЕМ !!!
            @"Server=tompsons.beget.tech,1433;Database=tompsons_stud19;Uid=tompsons_stud19;Pwd=Hrizalit1;Port=3306;SslMode=Preferred;CharSet=utf8;ConnectionTimeout=30;"
            //@"Server =tompsons.beget.tech;Database=tompsons_stud19;Trusted_Connection=True;"
            );
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Отношение Employee <-> User_E (Один-к-Одному)
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.User) // Сотрудник имеет ОДНОГО пользователя
                .WithOne(u => u.Employee) // Пользователь имеет ОДНОГО сотрудника
                .HasForeignKey<Employee>(e => e.Id_user); // Внешний ключ находится в Employee (Id_user)

            // 2. Отношение Employee <-> Post (Многие-к-Одному)
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Post) // Сотрудник имеет ОДИН пост
                .WithMany(p => p.Employees) // Пост имеет МНОГО сотрудников
                .HasForeignKey(e => e.Id_post); // Внешний ключ находится в Employee (Id_post)

            // Указываем, что Post_n в модели Post соответствует полю Post в БД
            modelBuilder.Entity<Post>()
                .Property(p => p.Post_n)
                .HasColumnName("Post");
        }
    }
}
