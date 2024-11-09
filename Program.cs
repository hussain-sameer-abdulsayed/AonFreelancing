
using AonFreelancing.Context;
using AonFreelancing.Models;
using AonFreelancing.Repositories.IRepos;
using AonFreelancing.Repositories.Repos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace AonFreelancing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                            .AddJsonOptions(x =>
                                    x.JsonSerializerOptions.ReferenceHandler =ReferenceHandler.IgnoreCycles);


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionString = builder.Configuration.GetConnectionString("ContextConnection") ?? throw new InvalidOperationException("Connection string ContextConnection not found.");

            builder.Services.AddDbContext<MyContext>(options => options.UseSqlServer(connectionString));


            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<MyContext>()
            //.AddRoles<IdentityRole>()
            .AddDefaultTokenProviders();



            //repos
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<IJWTMangerRepo, JWTManagerRepo>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
