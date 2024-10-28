using AonFreelancing.Context;
using AonFreelancing.DTOs;
using AonFreelancing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AonFreelancing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FreelancersController : ControllerBase
    {
        private readonly MyContext _context;

        public FreelancersController(MyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            
            return Ok(_context.Freelancers.ToList());
        }

        [HttpPost]
        public IActionResult Create([FromBody] AddFreelancerDto freelancerDto) {
            Freelancer freelancer = new Freelancer()
            {
                Name = freelancerDto.Name,
                Username = freelancerDto.UserName,
                Skills = freelancerDto.Skills,
                Password = freelancerDto.Password,
            };
            _context.Freelancers.Add(freelancer);
            _context.SaveChanges();
            return CreatedAtAction("Create", new { Id = freelancer.Id }, freelancer);
        }

        [HttpGet("{id}")]
        public IActionResult GetFreelancer(int id)
        {
            Freelancer fr = _context.Freelancers.FirstOrDefault(f => f.Id == id);

            if (fr == null)
            {
                return NotFound("The resoucre is not found!");
            }

            return Ok(fr);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Freelancer freelancer = _context.Freelancers.FirstOrDefault(f=>f.Id == id);
            if(freelancer != null)
            {
                _context.Freelancers.Remove(freelancer);
                _context.SaveChanges();
                return Ok("Deleted");

            }

            return NotFound();
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromBody] UpdateFreelancerDto freelancerDto,int id)
        {
            Freelancer freelancer = _context.Freelancers.FirstOrDefault(f => f.Id == id);
            if (freelancer != null)
            {
                if(string.IsNullOrEmpty(freelancerDto.Name))
                {
                    freelancer.Name = freelancerDto.Name;
                }
                if (string.IsNullOrEmpty(freelancerDto.Skills))
                {
                    freelancer.Skills = freelancerDto.Skills;
                }
                _context.SaveChanges();
                return Ok("Deleted");

            }

            return NotFound();
        }

    }
}
