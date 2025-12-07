using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp1.Models
{
    // Реализуем INotifyPropertyChanged
    public class Employee : INotifyPropertyChanged
    {
        // Переменные для хранения значений (backing fields)
        private int _id;
        private string _name = string.Empty;
        private string _surname = string.Empty;
        private string? _patronymic; // Nullable
        private string _login = string.Empty;
        private string _password = string.Empty;
        private string _phone = string.Empty;
        private string? _email; // Nullable
        private DateTime _birthday;
        private string _post = string.Empty;
        private decimal _salary;

        // Определяем свойства с вызовом OnPropertyChanged

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }
        public string Surname
        {
            get => _surname;
            set { _surname = value; OnPropertyChanged(); }
        }
        // 👇 Учитываем, что Patronymic и Email могут быть NULL (string?)
        public string? Patronymic
        {
            get => _patronymic;
            set { _patronymic = value; OnPropertyChanged(); }
        }
        public string Login
        {
            get => _login;
            set { _login = value; OnPropertyChanged(); }
        }
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }
        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(); }
        }
        public string? Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }
        public DateTime Birthday
        {
            get => _birthday;
            set { _birthday = value; OnPropertyChanged(); }
        }
        public string PostName
        {
            get => _post;
            set { _post = value; OnPropertyChanged(); }
        }
        public decimal Salary
        {
            get => _salary;
            set { _salary = value; OnPropertyChanged(); }
        }

        // Реализация INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
    //public class Employee
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string Surname { get; set; }
    //    public string? Patronymic { get; set; }
    //    public string Login { get; set; }
    //    public string Password { get; set; }
    //    public string Phone { get; set; }
    //    public string? Email { get; set; }
    //    public DateTime Birthday { get; set; }

    //    // Переименовано в PostName, чтобы соответствовать привязке в XAML
    //    public string PostName { get; set; }

    //    public decimal Salary { get; set; }
    //}
}