using System.ComponentModel.DataAnnotations;

namespace AonFreelancing.Models
{
    public class OTP
    {
        public int Id { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [Length(6,6)]
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpireAt { get; set; }
        public bool IsUsed { get; set; } = false;

        public OTP()
        {
            ExpireAt = CreatedAt.AddMinutes(10);
        }
    }
}
