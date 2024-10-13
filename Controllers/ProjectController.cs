using AonFreelancing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AonFreelancing.Controllers
{
    

    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private static List<Project> projects = new List<Project>();
        static int createId = 1;
        

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var project = projects.FirstOrDefault(p=>p.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
            
        }
        [HttpPost]
        public IActionResult Create([FromBody] string projectTitle)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            Project project = new Project()
            {
                Id = createId++,
                Title = projectTitle
            };
            projects.Add(project);
            return Ok(projects);
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromBody] string title,int id)
        {
            var project = projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            project.Title = title;
            return Ok(project);

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var project = projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            projects.Remove(project);
            return Ok(projects);

        }
    }
}
