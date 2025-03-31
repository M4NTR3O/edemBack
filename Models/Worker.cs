namespace edemBack.Models
{
    public class Worker
    {
        public int Id { get; set; }
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? Fathername { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime Birthday { get; set; }
        public int Id_Role { get; set; }
        public int Id_Office { get; set; }
        public string? Password { get; set; }
    }
}
