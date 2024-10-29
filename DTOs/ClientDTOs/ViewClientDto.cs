using AonFreelancing.DTOs.ProjectDTOs;
using AonFreelancing.Models;

namespace AonFreelancing.DTOs.ClientDTOs
{
    public class ViewClientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string CompanyName { get; set; }

        public List<ViewProjectDto>? Projects { get; set; } = new List<ViewProjectDto>();
    }
}
