using System.ComponentModel.DataAnnotations;

namespace AonFreelancing.DTOs.ClientDTOs
{
    public class UpdateClientDto
    {
        [MinLength(4), MaxLength(100)]
        public string? Name { get; set; }
        [MinLength(4), MaxLength(512)]
        public string? CompanyName { get; set; }
    }
}
