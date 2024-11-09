using AonFreelancing.Context;
using AonFreelancing.DTOs;
using AonFreelancing.DTOs.ClientDTOs;
using AonFreelancing.DTOs.FreelancerDTOs;
using AonFreelancing.DTOs.ProjectDTOs;
using AonFreelancing.Models;
using AonFreelancing.Repositories.IRepos;
using AonFreelancing.Utilites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AonFreelancing.Repositories.Repos
{
    
    public class UserRepo : IUserRepo
    {
        private readonly MyContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepo(MyContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<object> GetUserDtoByTypeAsync(string userType, User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = await _roleManager.FindByNameAsync(roles[0]);
            if(userType == Constants.USER_TYPE_FREELANCER)
            {
                return await _context.Users.OfType<Freelancer>()
                                .Where(u => u.Id == user.Id)
                                .Select(u => new ViewFreelancerDto()
                                {
                                    Id = u.Id,
                                    Name = u.Name,
                                    Username = u.UserName,
                                    PhoneNumber = u.PhoneNumber,
                                    Skills = u.Skills,
                                    UserType = Constants.USER_TYPE_FREELANCER,
                                    // role
                                    Role = new ViewRoleDto
                                    {
                                        Id = role.Id,
                                        Name = role.Name
                                    }
                                })
                                .FirstOrDefaultAsync();
            }
            else if(userType == Constants.USER_TYPE_CLIENT)
            {
                return await _context.Users.OfType<Client>()
                                .Where(u => u.Id == user.Id)
                                .Select(u => new ViewClientDto()
                                {
                                    Id = u.Id,
                                    Name = u.Name,
                                    Username = u.UserName,
                                    PhoneNumber = u.PhoneNumber,
                                    Projects = u.Projects.Select(u => new ViewProjectDto
                                    {
                                        Id = u.Id,
                                        ClientId = u.ClientId,
                                        CreatedAt = u.CreatedAt,
                                        Description = u.Description,
                                        FreelancerId = u.FreelancerId,
                                        Title = u.Title,
                                    }).ToList(),
                                    // role
                                    Role = new ViewRoleDto
                                    {
                                        Id = role.Id,
                                        Name = role.Name
                                    },
                                    UserType = Constants.USER_TYPE_FREELANCER,
                                    CompanyName = u.CompanyName,
                                })
                                .FirstOrDefaultAsync();
            }
            return null;
        }
    }
}
