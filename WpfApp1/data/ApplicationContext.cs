using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.models;
using WpfApp1.Models;

namespace WpfApp1.data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<EmployeeEntity> EmployeeEntities { get; set; }
        public DbSet<Post> Posts { get; set; }

        // Вы можете добавить DbSet<T> для каждой таблицы, 
        // но пока оставим его пустым, как вы и просили.
        // public DbSet<YourModel> YourModels { get; set; }

        public ApplicationContext()
        {
            // Здесь мы не вызываем Database.EnsureCreated(), 
            // так как вы просто хотите увидеть подключение.
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Строка подключения к вашей БД
            string connectionString =
                "Server=tompsons.beget.tech;" +
                "Port=3306;" + // Стандартный порт MySQL, проверьте, если у вас другой
                "Database=tompsons_stud19;" +
                "User=tompsons_stud19;" +
                $"Password=Hrizalit1;" +
                "Connection Timeout=30;"; ; // Вставьте сюда ваш пароль

            // Используем провайдер Pomelo для MySQL
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        // Настройка связей между моделями
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Связь User <-> EmployeeEntity (один к одному)
            modelBuilder.Entity<User>()
                .HasOne(u => u.EmployeeEntity)
                .WithOne(e => e.User)
                .HasForeignKey<EmployeeEntity>(e => e.IdUser);

            // Связь EmployeeEntity <-> Post (многие к одному)
            modelBuilder.Entity<EmployeeEntity>()
                .HasOne(e => e.Post)
                .WithMany(p => p.EmployeeEntities) // ТЕПЕРЬ ОШИБКИ НЕТ, Т.К. EmployeeEntities ЕСТЬ В Post.cs
                .HasForeignKey(e => e.IdPost);

            base.OnModelCreating(modelBuilder);
        }
    }
}
