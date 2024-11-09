namespace AonFreelancing.DTOs.UserDTOs
{
    public class ViewUserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Username { get; set; }
        public string PhoneNumber { get; set; }

        public bool IsPhoneNumberVerified { get; set; }
        public string UserType { get; set; }
        public ViewRoleDto Role { get; set; }
    }
}
