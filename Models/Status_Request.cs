namespace edemBack.Models
{
    /// <summary>
    /// Модель статусов для заявок
    /// </summary>
    public class Status_Request
    {
        /// <summary>ID статуса</summary>
        public int Id_Status { get; set; }
        /// <summary>Наименование статуса</summary>
        public string Name { get; set; }
    }
}
