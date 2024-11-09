using System.ComponentModel.DataAnnotations.Schema;

namespace AonFreelancing.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        // Belongs to a client
        public string ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        public string? FreelancerId { get; set; } // when project created we don't need freelancer then in future hw will take it 
        [ForeignKey("FreelancerId")]
        public Freelancer? Freelancer { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
