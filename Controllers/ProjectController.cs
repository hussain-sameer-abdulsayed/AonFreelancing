using AonFreelancing.Context;
using AonFreelancing.DTOs;
using AonFreelancing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AonFreelancing.Controllers
{
    

    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly MyContext _context;

        public ProjectController(MyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Projects.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var project = _context.Projects
                .Include(p=>p.Client)
                .FirstOrDefault(p=>p.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreateProjectDto projectDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            Project project = new Project()
            {
                ClientId = projectDto.ClientId,
                Title = projectDto.Title,
            };
            _context.Projects.Add(project);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromBody] string title,int id)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            project.Title = title;
            _context.SaveChanges();
            return Ok(project);

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            _context.Projects.Remove(project);
            _context.SaveChanges();
            return Ok();

        }
    }
}
