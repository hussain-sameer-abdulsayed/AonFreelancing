using AonFreelancing.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AonFreelancing.Context
{
    public class MyContext : IdentityDbContext<User, IdentityRole, string>
    {
        // For TPT design, no need to define each one
        //public DbSet<Freelancer> Freelancers { get; set; }
        //public DbSet<Client> Clients { get; set; }
        //public DbSet<SystemUser> SystemUsers { get; set; }

        // instead, use User only
        public DbSet<User> Users {  get; set; }// Will access Freelancers, Clients, SystemUsers through inheritance and ofType 

        public DbSet<Project> Projects { get; set; }
        public DbSet<OTP> Otps { get; set; }
        public DbSet<UserRefreshTokens> UserRefreshTokens { get; set; }

        public MyContext(DbContextOptions<MyContext> dbContext):base(dbContext)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // For TPT design
            //builder.Entity<Freelancer>().HasBaseType(typeof(User));
            //builder.Entity<Client>().HasBaseType(typeof(User));
            //builder.Entity<SystemUser>().HasBaseType(typeof(User));

            builder.Entity<User>().ToTable("AspNetUsers");
            builder.Entity<Freelancer>().ToTable("Freelancers");
            builder.Entity<Client>().ToTable("Clients");
            builder.Entity<SystemUser>().ToTable("SystemUsers");



            builder.Entity<IdentityRole>()
            .HasData(
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "systemadmin",
                    NormalizedName = "systemadmin".ToUpper()
                },
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "client",
                    NormalizedName = "client".ToUpper()
                },
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "freelancer",
                    NormalizedName = "freelancer".ToUpper()
                }
                );


            base.OnModelCreating(builder);
        }
    }
}
