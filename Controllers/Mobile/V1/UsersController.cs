using AonFreelancing.Context;
using AonFreelancing.DTOs.ClientDTOs;
using AonFreelancing.DTOs.FreelancerDTOs;
using AonFreelancing.DTOs.ProjectDTOs;
using AonFreelancing.Models;
using AonFreelancing.Utilites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Twilio.TwiML.Voice;
using static AonFreelancing.DTOs.ResponseDto;

namespace AonFreelancing.Controllers.Mobile.V1
{
    [Route("api/mobile/v1/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(MyContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("{id}/profile")]
        public async Task<IActionResult> GetUserProfile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            List<Project> projects = new List<Project>();

            if (user == null)
            {
                return NotFound(new ApiResponse<string>
                {
                    IsSuccess = false,
                    Results = "NotFound",
                    Errors = []
                });
            }
            if(user is Freelancer)
            {
                projects = await _context.Projects.Where(p =>p.FreelancerId == id).ToListAsync();
                var freelancer = await _context.Users.OfType<Freelancer>().FirstOrDefaultAsync(u=>u.Id == id);
                return Ok(new ApiResponse<object>
                {
                    IsSuccess = true,
                    Results = new ViewFreelancerDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Username = user.UserName,
                        PhoneNumber = user.PhoneNumber,
                        UserType = Constants.USER_TYPE_FREELANCER,
                        IsPhoneNumberVerified = user.PhoneNumberConfirmed,
                        Skills = freelancer.Skills,
                        Projects = projects.Select(p => new ViewProjectDto
                        {
                            Id = p.Id,
                            Title = p.Title,
                            ClientId = p.ClientId,
                            FreelancerId = p.FreelancerId,
                            CreatedAt = p.CreatedAt,
                            Description = p.Description,
                        }).ToList()
                        

                    }
                });
            }
            if (user is Models.Client)
            {
                projects = await _context.Projects.Where(p => p.ClientId == id).ToListAsync();
                var client = await _context.Users.OfType<Models.Client>().FirstOrDefaultAsync(u => u.Id == id);
                return Ok(new ApiResponse<object>
                {
                    IsSuccess = true,
                    Results = new ViewClientDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Username = user.UserName,
                        PhoneNumber = user.PhoneNumber,
                        UserType = Constants.USER_TYPE_CLIENT,
                        IsPhoneNumberVerified = user.PhoneNumberConfirmed,
                        CompanyName = client.CompanyName,
                        Projects = client.Projects.Select(p => new ViewProjectDto
                        {
                            Id= p.Id,
                            Title = p.Title,
                            ClientId = p.ClientId,
                            FreelancerId= p.FreelancerId,
                            CreatedAt = p.CreatedAt,
                            Description = p.Description,
                        }).ToList(),
                    }
                });
            }
            return BadRequest();
        }
    }
}
