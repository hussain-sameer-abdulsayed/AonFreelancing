using System.ComponentModel.DataAnnotations.Schema;

namespace AonFreelancing.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
