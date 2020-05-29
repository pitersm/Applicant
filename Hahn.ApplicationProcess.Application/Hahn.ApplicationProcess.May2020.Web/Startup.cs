using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Hahn.ApplicationProcess.May2020.Data.Data;
using Hahn.ApplicationProcess.May2020.Data.Repository;
using Hahn.ApplicationProcess.May2020.Data.Validators;
using Hahn.ApplicationProcess.Web.Middleware;
using Hahn.ApplicatonProcess.May2020.Domain;
using Hahn.ApplicatonProcess.May2020.Domain.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Hahn.ApplicationProcess.Application
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Applicant API",
                        Version = "v1",
                    });

                string appPath =
                    PlatformServices.Default.Application.ApplicationBasePath;
                string appName =
                    PlatformServices.Default.Application.ApplicationName;
                var filePath = Path.Combine(AppContext.BaseDirectory, "Hahn.ApplicationProcess.May2020.Web.xml");

                Assembly.GetExecutingAssembly().GetReferencedAssemblies();

                var currentAssembly = Assembly.GetExecutingAssembly();
                var xmlDocs = currentAssembly.GetReferencedAssemblies()
                                             .Union(new AssemblyName[] { currentAssembly.GetName() })
                                             .Select(a => Path.Combine(Path.GetDirectoryName(currentAssembly.Location), $"{a.Name}.xml"))
                                             .Where(f => File.Exists(f)).ToArray();

                Array.ForEach(xmlDocs, (d) =>
                {
                    c.IncludeXmlComments(d);
                });
            });

            services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("ApplicantDB"));
            services.AddScoped<ApplicantRepository, ApplicantRepository>();
            services.AddScoped<IApplicantService, ApplicantService>();
            services.AddScoped<ApplicantValidator, ApplicantValidator>();
            services.AddControllers().AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "Applicant API");
            });
        }
    }
}
