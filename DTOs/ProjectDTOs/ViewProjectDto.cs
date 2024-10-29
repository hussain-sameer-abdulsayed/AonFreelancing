using AonFreelancing.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace AonFreelancing.DTOs.ProjectDTOs
{
    public class ViewProjectDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int ClientId { get; set; }
        public int? FreelancerId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
