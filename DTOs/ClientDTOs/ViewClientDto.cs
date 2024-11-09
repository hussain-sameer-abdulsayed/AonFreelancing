using AonFreelancing.DTOs.ProjectDTOs;
using AonFreelancing.DTOs.UserDTOs;
using AonFreelancing.Models;

namespace AonFreelancing.DTOs.ClientDTOs
{
    public class ViewClientDto : ViewUserDto
    {
        public string CompanyName { get; set; }

        public List<ViewProjectDto>? Projects { get; set; } = new List<ViewProjectDto>();
    }
}
