namespace edemBack.Models
{
    /// <summary>
    /// Модель офиса
    /// </summary>
    public class Office
    {
        /// <summary>ID офиса</summary>
        public int? Office_Id { get; set; }
        /// <summary>Улица, на которой расположен офис</summary>
        public string? Street { get; set; }
        /// <summary>Дом, в котором расположен офис</summary>
        public string? House { get; set; }
    }
}
