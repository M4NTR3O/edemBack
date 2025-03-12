namespace edemBack.Models
{
    public class Object
    {
        public int? Id { get; set; }
        public int Office_Id { get; set; }
        public string? Street { get; set; }
        public string? House { get; set; }
        public string? Apartment { get; set; }
        public int Rooms { get; set; }
        public string? Status { get; set; }
        public decimal Area { get; set; }
        public bool Kitchen { get; set; }
        public bool Balcony { get ; set ; }
    }
}
