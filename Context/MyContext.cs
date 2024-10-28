using AonFreelancing.Models;
using Microsoft.EntityFrameworkCore;

namespace AonFreelancing.Context
{
    public class MyContext : DbContext
    {
        public DbSet<Freelancer> Freelancers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<SystemUser> SystemUsers { get; set; }
        public DbSet<Project> Projects { get; set; }

        public MyContext(DbContextOptions<MyContext> dbContext):base(dbContext)
        {

        }
    }
}
