using AonFreelancing.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AonFreelancing.DTOs.ProjectDTOs
{
    public class CreateProjectDto
    {
        [MinLength(4), MaxLength(100)]
        public string Title { get; set; }
        [MinLength(4), MaxLength(250)]
        public string? Description { get; set; }
        public int ClientId { get; set; }
    }
}
