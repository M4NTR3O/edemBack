namespace edemBack.Models
{
    /// <summary>
    /// Модель объекта
    /// </summary>
    public class Object
    {
        /// <summary>ID объекта</summary>
        public int? Id { get; set; }
        /// <summary>ID офиса</summary>
        public int Office_Id { get; set; }
        /// <summary>Улица, на которой расположен объект</summary>
        public string? Street { get; set; }
        /// <summary>Дом, в котором располагается объект</summary>
        public string? House { get; set; }
        /// <summary>Номер квартиры объекта (при необходимости, может быть null)</summary>
        public string? Apartment { get; set; }
        /// <summary>Количество комнат у объекта</summary>
        public int Rooms { get; set; }
        /// <summary>Статус объекта, может быть как числовой, так и текстовый</summary>
        public string? Status { get; set; }
        /// <summary>Площадь объекта</summary>
        public decimal Area { get; set; }
        /// <summary>Наличие кухни</summary>
        public bool Kitchen { get; set; }
        /// <summary>Наличие балкона</summary>
        public bool Balcony { get ; set ; }
    }
}
