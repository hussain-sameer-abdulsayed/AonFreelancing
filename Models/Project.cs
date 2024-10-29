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
        public int? FreelancerId { get; set; } // when project created we don't need freelancer then in future hw will take it 
        [ForeignKey("FreelancerId")]
        public Freelancer? Freelancer { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
