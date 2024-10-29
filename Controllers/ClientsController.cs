using AonFreelancing.Context;
using AonFreelancing.DTOs.ClientDTOs;
using AonFreelancing.DTOs.FreelancerDTOs;
using AonFreelancing.DTOs.ProjectDTOs;
using AonFreelancing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AonFreelancing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly MyContext _context;

        public ClientsController(MyContext context)
        {
            _context = context;
        }




        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            var clientDtoList = new List<ViewClientDto>();
            clientDtoList = await _context.Clients
                                     .Select(c => new ViewClientDto
                                     {
                                         Id = c.Id,
                                         Name = c.Name,
                                         CompanyName = c.CompanyName,
                                         Username= c.Username,
                                         Projects = c.Projects.Select(p=>new ViewProjectDto
                                         {
                                             Id=p.Id,
                                             Title = p.Title,
                                             Description = p.Description,
                                             ClientId = p.ClientId,
                                             CreatedAt = p.CreatedAt,
                                         }).ToList()
                                     })
                                    .ToListAsync();

            return Ok(clientDtoList);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            var clientDto = new ViewClientDto();
            clientDto = await _context.Clients
                                    .Select(c=>new ViewClientDto
                                    {
                                        Id = c.Id,
                                        Name = c.Name,
                                        Username= c.Username,
                                        CompanyName = c.CompanyName,
                                        Projects = c.Projects.Select(p=> new ViewProjectDto
                                        {
                                            Id= p.Id,
                                            Title = p.Title,
                                            Description = p.Description,
                                            ClientId = p.ClientId,
                                            CreatedAt = p.CreatedAt,
                                        }).ToList()
                                    })
                                    .FirstOrDefaultAsync(f => f.Id == id);

            if (clientDto == null)
            {
                return NotFound("The resoucre is not found!");
            }
            return Ok(clientDto);
        }


        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] AddClientDto clientDto)
        {
            Client client = new Client()
            {
                Name = clientDto.Name,
                Username = clientDto.UserName,
                Password = clientDto.Password,
                CompanyName = clientDto.CompanyName,
            };
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetClientById", new { id = client.Id }, client);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            Client client = await _context.Clients.FirstOrDefaultAsync(f => f.Id == id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
                return Ok("Deleted");
            }

            return NotFound("not found");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient([FromBody] UpdateClientDto clientDto, int id)
        {
            Client client = _context.Clients.FirstOrDefault(f => f.Id == id);
            if (client != null)
            {
                if (!string.IsNullOrEmpty(clientDto.Name))
                {
                    client.Name = clientDto.Name;
                }
                if (!string.IsNullOrEmpty(clientDto.CompanyName))
                {
                    client.CompanyName = clientDto.CompanyName;
                }
                //password must not updated here, in future send email to user with token etc....
                await _context.SaveChangesAsync();
                return Ok("updated");
            }
            return NotFound("not found");
        }
    }
}
