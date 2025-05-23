namespace edemBack.Models
{
    /// <summary>
    /// Модель заявки на работы (Будет пересмотрена в свзяи с добавлением и изменением параметров)
    /// </summary>
    public class Request
    {
        /// <summary>ID заявки</summary>
        public int Request_Id { get; set; }
        /// <summary>Тип задания</summary>
        public string Type_Work { get; set; }
        /// <summary>Текстовое описание работ (может быть null)</summary>
        public string? Description { get; set; }

        /// <summary>Лист ID ролей, которые могут быть назначены на работу</summary>
        public List<Work_Role>? Roles_Id { get; set; }
        /// <summary>Флаг, назначен ли сотрудник или нет</summary>
        public bool Worker { get; set; }
        /// <summary>ID объекта</summary>
        public int Object_Id { get; set; }
        /// <summary>Статус заявки (строковая или числовая переменная)</summary>
        public string Status { get; set; }
        /// <summary>Срочность</summary>
        public bool Urgency { get; set; }
        /// <summary>Массив ссылок на фотографии (может быть null)</summary>
        public List<string>? Photos { get; set; }
        ///<summary>Лист работников (может быть null)</summary>
        public List<int>? Workers_id { get; set; }
        ///<summary>Лист работников (может быть null)</summary>
        public List<RequestWorker>? Workers_Request { get; set; }
    }
}
