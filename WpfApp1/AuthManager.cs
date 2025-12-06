using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Файл: Utilities/AuthManager.cs
using WpfApp1.Models;

namespace WpfApp1.Utilities
{
    // Статический класс для хранения данных сессии
    public static class AuthManager
    {
        public static Users? CurrentUser { get; private set; }

        public static bool IsLoggedIn => CurrentUser != null;

        public static void Login(Users user)
        {
            CurrentUser = user;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}
