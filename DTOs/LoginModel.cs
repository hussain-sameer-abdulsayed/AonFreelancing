using System.ComponentModel.DataAnnotations;

namespace AonFreelancing.DTOs
{
    public class LoginModel
    {
        [MinLength(4, ErrorMessage = "Invalid Username")]
        public string UserName { get; set; }
        [MinLength(4, ErrorMessage = "Invalid Password")]
        public string Password { get; set; }
    }
}
