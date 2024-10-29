using AonFreelancing.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AonFreelancing.DTOs.ProjectDTOs
{
    public class UpdateProjectDto
    {
        [MinLength(4), MaxLength(100)]
        public string? Title { get; set; }
        [MinLength(4), MaxLength(250)]
        public string? Description { get; set; }
    }
}
