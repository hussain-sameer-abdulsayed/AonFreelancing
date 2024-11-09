using System.ComponentModel.DataAnnotations;

namespace AonFreelancing.DTOs
{
    public class VerifiyOTP
    {
        [Length(14, 14)]
        public string Phone { get; set; }

        [Length(6, 6)]
        public string Otp { get; set; }
    }
}
