using System.ComponentModel.DataAnnotations;

namespace AonFreelancing.DTOs.FreelancerDTOs
{
    public class UpdateFreelancerDto
    {
        [MinLength(4), MaxLength(100)]
        public string? Name { get; set; }
        [MaxLength(512)]
        public string? Skills { get; set; }
    }
}
