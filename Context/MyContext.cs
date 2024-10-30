using AonFreelancing.Models;
using Microsoft.EntityFrameworkCore;

namespace AonFreelancing.Context
{
    public class MyContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Freelancer> Freelancers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<SystemUser> SystemUsers { get; set; }
        public DbSet<Project> Projects { get; set; }

        public MyContext(DbContextOptions<MyContext> dbContext):base(dbContext)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map each derived type to its own table
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Freelancer>().ToTable("Freelancers");
            modelBuilder.Entity<SystemUser>().ToTable("SystemUsers");
            modelBuilder.Entity<Client>().ToTable("Clients");
            modelBuilder.Entity<Project>().ToTable("Projects");

            base.OnModelCreating(modelBuilder);
        }
    }
}
