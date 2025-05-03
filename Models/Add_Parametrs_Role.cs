namespace edemBack.Models
{
    /// <summary>
    /// Модель добавление дополнительных параметров для работников
    /// </summary>
    public class Add_Parametrs_Role
    {
        /// <summary>ID параметра</summary>
        public int Id_Parametr { get; set; }
        /// <summary>ID роли</summary>
        public int Role_Id { get; set; }
        /// <summary>Наименование параметра</summary>
        public string Parametr { get; set; }
    }
}
