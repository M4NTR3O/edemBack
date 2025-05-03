namespace edemBack.Models
{
    /// <summary>
    /// Модель связи фото и объекта
    /// </summary>
    public class Photo_Object
    {
        /// <summary>Идентификатор фото</summary>
        public int Photo_id { get; set; }
        /// <summary>Идентификатор объекта</summary>
        public int Object_id { get; set; }
    }
}
