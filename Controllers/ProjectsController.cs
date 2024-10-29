using AonFreelancing.Context;
using AonFreelancing.DTOs.ProjectDTOs;
using AonFreelancing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AonFreelancing.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly MyContext _context;

        public ProjectsController(MyContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projectDtoList = new List<ViewProjectDto>();
            projectDtoList = await _context.Projects
                                .Select(p => new ViewProjectDto
                                {
                                    Id = p.Id,
                                    Title = p.Title,
                                    Description = p.Description,
                                    ClientId = p.ClientId,
                                    FreelancerId = p.FreelancerId,
                                    CreatedAt = p.CreatedAt
                                })
                                .ToListAsync();
            return Ok(projectDtoList);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var projectDto = new ViewProjectDto();
            projectDto = await _context.Projects
                            .Select(p=>new ViewProjectDto
                            {
                                Id=p.Id,
                                Title = p.Title,
                                Description = p.Description,
                                ClientId = p.ClientId,
                                FreelancerId=p.FreelancerId,
                                CreatedAt = p.CreatedAt
                            })
                            .FirstOrDefaultAsync(p=>p.Id == id);
            if (projectDto == null)
            {
                return NotFound("not found");
            }
            return Ok(projectDto);
        }


        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto projectDto)
        {
            Project project = new Project()
            {
                ClientId = projectDto.ClientId,
                Title = projectDto.Title,
                Description = projectDto.Description,
                CreatedAt= DateTime.UtcNow
            };
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetProjectById",new {id = project.Id }, project);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject([FromBody] UpdateProjectDto projectDto,int id)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            if(!string.IsNullOrEmpty(project.Title))
            {
                project.Title = projectDto.Title;
            }
            if (!string.IsNullOrEmpty(project.Description))
            {
                project.Description = projectDto.Description;
            }
            await _context.SaveChangesAsync();
            return Ok(project);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
            {
                return NotFound("not found");
            }
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return Ok("deleted successfully");
        }
    }
}