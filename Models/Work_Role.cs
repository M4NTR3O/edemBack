namespace edemBack.Models
{
    /// <summary>
    /// Модель связи работ с ролями
    /// </summary>
    public class Work_Role
    {
        ///<summary>Идентификатор связи роли с работой</summary>
        public int ID { get; set; }
        /// <summary>ID работа</summary>
        public string Work { get; set; }
        /// <summary>ID Роли</summary>
        public string Role { get; set; }
    }
}
