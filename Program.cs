
using AonFreelancing.Context;
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
            // ignore relations cycles
            builder.Services.AddControllers()
                            .AddJsonOptions(x =>
                                    x.JsonSerializerOptions.ReferenceHandler =                                              ReferenceHandler.IgnoreCycles);


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // configure the database context with sql server database
            var connectionString = builder.Configuration.GetConnectionString("ContextConnection") ?? throw new InvalidOperationException("Connection string ContextConnection not found.");
            builder.Services.AddDbContext<MyContext>(options => options.UseSqlServer(connectionString));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
