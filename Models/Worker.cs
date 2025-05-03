namespace edemBack.Models
{
    /// <summary>
    /// Модель работника
    /// </summary>
    public class Worker
    {
        /// <summary>Идентификатор работника</summary>
        public int Id { get; set; }
        /// <summary>Фамилия работника</summary>
        public string Surname { get; set; }
        /// <summary>Имя работника</summary>
        public string Name { get; set; }
        /// <summary>Отчество работника (при наличии, может быть null)</summary>
        public string? Fathername { get; set; }
        /// <summary>Номер телефона работника</summary>
        public string PhoneNumber { get; set; }
        /// <summary>Email работника</summary>
        public string Email { get; set; }
        /// <summary>Дата рождения работника</summary>
        public DateTime Birthday { get; set; }
        /// <summary>Идентификатор роли</summary>
        public int Id_Role { get; set; }
        //Добавить Название роли
        /// <summary>Иднтификатор офиса</summary>
        public int Id_Office { get; set; }
        /// <summary>Пароль пользователя</summary>
        public string Password { get; set; }
    }
}
