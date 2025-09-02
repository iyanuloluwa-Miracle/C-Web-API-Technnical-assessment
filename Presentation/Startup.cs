using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Application.Profiles;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Presentation.Middleware;

namespace Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container
            services.AddControllers()
                .ConfigureApplicationPartManager(manager => 
                {
                    // Clear all existing ApplicationParts
                    var parts = manager.ApplicationParts.ToList();
                    foreach (var part in parts)
                    {
                        manager.ApplicationParts.Remove(part);
                    }
                    
                    // Add only the Presentation assembly
                    manager.ApplicationParts.Add(new Microsoft.AspNetCore.Mvc.ApplicationParts.AssemblyPart(typeof(Startup).Assembly));
                });
                
            services.AddEndpointsApiExplorer();

            // Add Swagger with version configuration
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "stackbuld API",
                    Version = "v1",
                    Description = "A simple API for managing products and orders"
                });
                
                // Add a custom operation filter to resolve conflicts
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });

            // Configure API versioning
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            // Configure DB context with SQLite
            services.AddDbContext<AppDbContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlite(connectionString);
            });

            // Configure dependency injection
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ProductService>();
            services.AddScoped<OrderService>();

            // Configure AutoMapper
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => 
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "stackbuld API v1");
                    c.RoutePrefix = "swagger";
                });
                
                // Apply migrations and seed data in development
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    dbContext.Database.EnsureCreated();
                }
            }

            app.UseHttpsRedirection();
            
            app.UseRouting();
            
            // Use custom middleware
            app.UseMiddleware<ErrorHandlingMiddleware>();
            
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
