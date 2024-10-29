using AonFreelancing.DTOs.ProjectDTOs;
using AonFreelancing.Models;

namespace AonFreelancing.DTOs.FreelancerDTOs
{
    public class ViewFreelancerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Skills { get; set; }

        public List<ViewProjectDto>? Projects { get; set; } = new List<ViewProjectDto>();
    }
}
