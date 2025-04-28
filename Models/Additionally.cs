namespace edemBack.Models
{
    /// <summary>
    /// Модель дополнительного параметра для объекта
    /// </summary>
    public class Additionally
    {
        /// <summary>ID объекта</summary>
        public int ID_object{get; set;}

        /// <summary>Наименование параметра</summary>
        public string Parametr { get; set; }

        /// <summary>Значение параметра</summary>
        public string Value { get; set; }
    }
}
