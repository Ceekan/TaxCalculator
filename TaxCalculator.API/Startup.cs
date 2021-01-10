using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TaxCalculator.API.Data;
using TaxCalculator.API.Logic;

namespace TaxCalculator.API
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
            services.AddDbContext<TaxCalculatorDBContext>(options =>
                           options.UseSqlServer(Configuration.GetConnectionString("TaxCalculatorDBContext")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                        builder =>
                        {
                            builder.WithOrigins("https://localhost:5002", "http://localhost:5003");
                            builder.AllowAnyMethod(); builder.AllowAnyHeader(); builder.AllowCredentials();
                        });
            });

            services.AddSwaggerGen(configuration =>
            {
                configuration.SwaggerDoc("v1", new OpenApiInfo { Title = $"{Assembly.GetExecutingAssembly().GetName().Name}", Version = "v1" });
            });

            services.AddScoped<ITaxManager, TaxManager>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(configuration => configuration.SwaggerEndpoint("/swagger/v1/swagger.json", "TaxCalculator.API"));
            }

            app.UseHttpsRedirection();            

            app.UseRouting();

            app.UseCors("AllowSpecificOrigin");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
