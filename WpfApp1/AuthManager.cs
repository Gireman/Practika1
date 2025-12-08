using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Файл: Utilities/AuthManager.cs
using WpfApp1.Models;

namespace WpfApp1.Utilities
{
    public static class AuthManager
    {
        // Хранит данные о текущем вошедшем пользователе/сотруднике
        public static User? CurrentUser { get; private set; }

        // Должность для сотрудников (role_id = 1). 
        // Если пользователь (role_id = 2), это будет 0 или NULL, это не важно.
        public static int CurrentUserPostId { get; private set; }

        public static bool IsAuthenticated => CurrentUser != null;

        public static void Login(User user, int postId = 0)
        {
            CurrentUser = user;
            CurrentUserPostId = postId;
        }

        public static void Logout()
        {
            CurrentUser = null;
            CurrentUserPostId = 0;
        }
    }
}
