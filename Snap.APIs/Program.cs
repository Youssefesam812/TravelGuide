using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Snap.APIs.Errors;
using Snap.APIs.Extensions;
using Snap.APIs.Middlewares;
using Snap.Core.Entities;
using Snap.Repository.Data;
using Snap.Repository.Seeders;
using System.Text.Json.Serialization;

namespace Snap.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });

            // Configure DbContext
            builder.Services.AddDbContext<SnapDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            // Configure Identity Services
            builder.Services.AddIdentityServices();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.Configure<ApiBehaviorOptions>(
             Options =>{Options
                .InvalidModelStateResponseFactory = (actionContext) =>
            {var errors = actionContext.ModelState
                .Where(p => p.Value.Errors.Count() > 0)
                .SelectMany(p => p.Value.Errors)
                .Select(E => E.ErrorMessage)
                .ToArray();
            var ValidationErrorResponse = new ApiValidationErrorResponse() 
             { Erorrs = errors};
                return new BadRequestObjectResult(ValidationErrorResponse);};});
            var app = builder.Build();

            #region Apply Migrations and Seed Data

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var dbContext = services.GetRequiredService<SnapDbContext>();

                // Apply pending migrations
                await dbContext.Database.MigrateAsync();

                // Seed default users
                var userManager = services.GetRequiredService<UserManager<User>>();
                var logger = loggerFactory.CreateLogger<Program>();

                logger.LogInformation("Seeding default users...");
                await UserSeed.SeedUserAsync(userManager);
                logger.LogInformation("User seeding completed.");
            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(e, "An error occurred while applying migrations and seeding users.");
            }

            #endregion

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseMiddleware<ExceptionMiddleware>();
                app.UseDeveloperExceptionPage();    
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            app.UseAuthentication(); 
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
