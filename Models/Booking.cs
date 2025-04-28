namespace edemBack.Models
{
    /// <summary>
    /// Модель бронирования
    /// </summary>
    public class Booking
    {
        /// <summary>ID бронирования</summary>
        public int Id_Booking { get; set; }

        /// <summary>ID объекта, на которое применяется бронирование</summary>
        public int Object_id { get; set; }
        /// <summary>Дата начала бронирования</summary>
        public DateTime Date_Start { get; set; }
        /// <summary>Дата завершения бронирования</summary>
        public DateTime Date_End { get; set; }

        /// <summary>ID статуса бронирования (либо текст, необходимо будет реализовать функцию</summary>
        public string Status { get; set; }
    }
}
