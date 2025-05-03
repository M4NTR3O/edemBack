namespace edemBack.Models
{
    /// <summary>
    /// Модель связи фото и заявки
    /// </summary>
    public class Photo_Request
    {
        /// <summary>Идентификатор фото</summary>
        public int Photo_id { get; set; }
        /// <summary>Идентификатор заявки</summary>
        public int Request_id { get; set; }
        /// <summary>Время фотографии (или загрузки)</summary>
        public DateTime Time { get; set; }
    }
}
