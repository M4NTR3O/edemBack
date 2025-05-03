namespace edemBack.Models
{
    /// <summary>
    /// Модель типа работ
    /// </summary>
    public class TypeWork
    {
        /// <summary>ID работ</summary>
        public int Id_Work { get; set; }
        /// <summary>Наименование работы</summary>
        public string Name { get; set; }
        ///Список ролей, которые могут выполнить данный тип задачи
        public List<Models.Role>? Roles { get; set; }
    }
}
