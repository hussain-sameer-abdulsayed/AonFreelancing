using AonFreelancing.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace AonFreelancing.DTOs.ProjectDTOs
{
    public class ViewProjectDto
    {
        public int Id { get; set; } // the fields in c sharp classes are required in default so i don't prefer using required attribute
        public string Title { get; set; }
        public string? Description { get; set; } // the question mark means that field is nullable
        public int ClientId { get; set; }
        public int? FreelancerId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
