namespace edemBack.Models
{
    /// <summary>
    /// Модель авторизации
    /// </summary>
    public class AuthRequest
    {
        /// <summary>Логин пользователя (номер телефона)</summary>
        public string Login { get; set; }
        /// <summary>Пароль</summary>
        public string Password { get; set; }
    }
}
