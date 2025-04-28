namespace edemBack.Models
{
    /// <summary>
    /// Модель роли
    /// </summary>
    public class Role
    {
        /// <summary>ID роли</summary>
        public int? Role_Id { get; set; }
        /// <summary>Наименование роли</summary>
        public string? Name { get; set; }
        /// <summary>Зарплата</summary>
        public int Salary { get; set; }
    }
}
