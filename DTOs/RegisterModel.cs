using System.ComponentModel.DataAnnotations;

namespace AonFreelancing.DTOs
{
    public class RegisterModel
    {
        [MinLength(2)]
        public string Name { get; set; }

        [MinLength(4)]
        public string Username { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }


        [MinLength(4, ErrorMessage = "Too short password")]
        public string Password { get; set; }

        [AllowedValues("Freelancer", "Client")]
        public string UserType { get; set; }

        // For freelancer type only
        public string? Skills { get; set; }

        // For Client user type only


        public string? CompanyName { get; set; }
    }
}
