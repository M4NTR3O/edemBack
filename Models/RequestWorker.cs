using System.ComponentModel.DataAnnotations;

namespace edemBack.Models
{
    public class RequestWorker
    {
        public int? Id { get; set; }
        public int Request { get; set; }
        public int Worker { get; set; }
    }
}
