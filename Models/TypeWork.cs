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
        ///<summary>Список ролей, которые могут выполнить данный тип задачи</summary>
        public List<Models.Role>? Roles { get; set; }
        ///<summary>Список ролей (целочисленно), которые могут выполнить данный тип задачи</summary>
        public List<int>? Roles_Id { get; set; }
    }
}
