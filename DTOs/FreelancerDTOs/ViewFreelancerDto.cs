using AonFreelancing.DTOs.ProjectDTOs;
using AonFreelancing.DTOs.UserDTOs;
using AonFreelancing.Models;

namespace AonFreelancing.DTOs.FreelancerDTOs
{
    public class ViewFreelancerDto : ViewUserDto
    {
        public string Skills { get; set; }

        public List<ViewProjectDto>? Projects { get; set; } = new List<ViewProjectDto>();
    }
}
