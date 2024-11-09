using AonFreelancing.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace AonFreelancing.DTOs.ProjectDTOs
{
    public class ViewProjectDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string ClientId { get; set; }
        public string? FreelancerId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
