using System.ComponentModel.DataAnnotations;

namespace AonFreelancing.DTOs.FreelancerDTOs
{
    public class AddFreelancerDto
    {
        [MinLength(4), MaxLength(100)]
        public string Name { get; set; }
        [MinLength(4), MaxLength(100)]
        public string UserName { get; set; }
        [MaxLength(512)]
        public string Skills { get; set; }
        [MinLength(4), MaxLength(512)]
        public string Password { get; set; }
    }
}
