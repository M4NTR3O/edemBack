namespace edemBack.Models
{
    public class Request
    {
        public int? Request_Id { get; set; }
        public int Role_Id { get; set; }
        public int? Worker_Id { get; set; }
        public int Object_Id { get; set; }
        public string Status { get; set; }
        public bool Urgency { get; set; }
    }
}
