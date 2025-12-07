using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.models;
using WpfApp1.Models;

namespace WpfApp1.viewmodels
{
    class ViewEmployees
    {
        public ObservableCollection<Employee> Employees { get; set; }
        public ViewEmployees()
        {
            LoadData();
        }
        private void LoadData()
        {
            using (var context = new AppDbContext())
            {
                Employees = new ObservableCollection<Employee>(context.Employees
                    .Include(e => e.Post) // Загружаем данные из Posts
                    .Include(e => e.User) // Загружаем данные из Users
                    .ToList());
            }
        }
    }
}
