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
        public int? Salary { get; set; }
        ///<summary>Дополнительные параметры для роли (вопросы для заявок)</summary>
        public List<Models.Add_Parametrs_Role>? add_Parametrs { get; set; }
        ///<summary>Уровень приоритета</summary>
        public int? LevelImportant { get; set; }
    }
}
