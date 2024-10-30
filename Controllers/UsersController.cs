using AonFreelancing.Context;
using AonFreelancing.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AonFreelancing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyContext _context;

        public UsersController(MyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = new List<ViewUserDto>();
            users = await _context.Users
                            .Select(u=>new ViewUserDto
                            {
                                Id = u.Id,
                                Name = u.Name,
                                UserName = u.Username,
                            })
                            .ToListAsync();
            return Ok(users);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = new ViewUserDto();
            user = await _context.Users
                            .Select(u=>new ViewUserDto
                            {
                                Id=u.Id,
                                Name=u.Name,
                                UserName=u.Username,
                            })
                            .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound("resource not found");
            }
            return Ok(user);
        }
    }
}
