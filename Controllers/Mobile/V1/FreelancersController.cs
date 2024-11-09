using AonFreelancing.Context;
using AonFreelancing.DTOs.ClientDTOs;
using AonFreelancing.DTOs.FreelancerDTOs;
using AonFreelancing.DTOs.ProjectDTOs;
using AonFreelancing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AonFreelancing.Controllers.Mobile.V1
{
    [Route("api/mobile/v1/freelancers")]
    [ApiController]
    public class FreelancersController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly UserManager<User> _userManager;

        public FreelancersController(MyContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }




        [HttpGet]
        public async Task<IActionResult> GetAllFreelancers()
        {
            var frDtoList = new List<ViewFreelancerDto>();
            frDtoList = await _context.Users.OfType<Freelancer>()
                            .Select(f => new ViewFreelancerDto
                            {
                                Id = f.Id,
                                Name = f.Name,
                                Username = f.UserName,
                                Skills = f.Skills,
                                
                                /*
                                Projects = f.Projects.Select(p=>new ViewProjectDto
                                {
                                    Id=p.Id,
                                    Title=p.Title,
                                    Description=p.Description,
                                    CreatedAt=p.CreatedAt,
                                    ClientId=p.ClientId,
                                }).ToList()
                                */

                            })
                            .ToListAsync();
            return Ok(frDtoList);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetFreelancerById(string id)
        {
            var fr = new ViewFreelancerDto();
            fr = await _context.Users.OfType<Freelancer>()
                        .Select(f => new ViewFreelancerDto
                        {
                            Id = f.Id,
                            Name = f.Name,
                            Username = f.UserName,
                            Skills = f.Skills,
                            Projects = f.Projects.Select(p => new ViewProjectDto
                            {
                                Id = p.Id,
                                Title = p.Title,
                                Description = p.Description,
                                CreatedAt = p.CreatedAt,
                                ClientId = p.ClientId,
                            }).ToList()
                        })
                        .FirstOrDefaultAsync(f => f.Id == id);

            if (fr == null)
            {
                return NotFound("The resoucre is not found!");
            }
            return Ok(fr);
        }


        [HttpGet("{id}/loadprojects")]
        public async Task<IActionResult> GetFreelancerByIdWithProjects([FromQuery] string loadprojects, string id)
        {
            var frDto = new ViewFreelancerDto();
            if (loadprojects == "0")
            {
                frDto = await _context.Users.OfType<Freelancer>()
                        .Select(f => new ViewFreelancerDto
                        {
                            Id = f.Id,
                            Name = f.Name,
                            Username = f.UserName,
                            Skills = f.Skills
                        })
                        .FirstOrDefaultAsync(f => f.Id == id);
            }
            else if (loadprojects == "1")
            {
                frDto = await _context.Users.OfType<Freelancer>()
                        .Select(f => new ViewFreelancerDto
                        {
                            Id = f.Id,
                            Name = f.Name,
                            Username = f.UserName,
                            Skills = f.Skills,
                            Projects = f.Projects.Select(p => new ViewProjectDto
                            {
                                Id = p.Id,
                                Title = p.Title,
                                Description = p.Description,
                                CreatedAt = p.CreatedAt,
                                ClientId = p.ClientId,
                            }).ToList()
                        })
                        .FirstOrDefaultAsync(f => f.Id == id);
            }


            if (frDto == null)
            {
                return NotFound("no resource");
            }
            return Ok(frDto);
        }


        [HttpPost]
        public async Task<IActionResult> CreateFreelancer([FromBody] AddFreelancerDto freelancerDto)
        {
            Freelancer freelancer = new Freelancer()
            {
                Name = freelancerDto.Name,
                UserName = freelancerDto.Username,
                Skills = freelancerDto.Skills,
            };
            //await _context.Freelancers.AddAsync(freelancer);
            //await _context.SaveChangesAsync();
            await _userManager.CreateAsync(freelancer, freelancerDto.Password);
            return CreatedAtAction("GetFreelancerById", new { id = freelancer.Id }, freelancer);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFreelancer(string id)
        {
            Freelancer freelancer = await _context.Users.OfType<Freelancer>().FirstOrDefaultAsync(f => f.Id == id);
            if (freelancer != null)
            {
                _context.Users.Remove(freelancer);
                await _context.SaveChangesAsync();
                return Ok("Deleted");
            }

            return NotFound("not found");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFreelancer([FromBody] UpdateFreelancerDto freelancerDto, string id)
        {
            Freelancer freelancer = _context.Users.OfType<Freelancer>().FirstOrDefault(f => f.Id == id);
            if (freelancer != null)
            {
                if (!string.IsNullOrEmpty(freelancerDto.Name))
                {
                    freelancer.Name = freelancerDto.Name;
                }
                if (!string.IsNullOrEmpty(freelancerDto.Skills))
                {
                    freelancer.Skills = freelancerDto.Skills;
                }
                //password must not updated here, in future send email to user with token etc....
                await _context.SaveChangesAsync();
                return Ok("Updated");
            }
            return NotFound("not found");
        }

    }
}
