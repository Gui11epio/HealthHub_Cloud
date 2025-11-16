using Health_Hub.Application.Mapping;
using Health_Hub.Application.Services;
using Health_Hub.Domain.IRepositories;
using System.Text.Json.Serialization;
using Health_Hub.Extensions;
using Health_Hub.Infrastructure.Context;
using Health_Hub.Infrastructure.Repositories;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Health_Hub.Infrastructure.HealthChecks;
using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Health_Hub.Domain.Interfaces;

namespace Health_Hub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Health-Hub.API",
                    Version = "v1",
                    Description = "Documentação da versão 1 da API."
                });

                
                c.EnableAnnotations();


            });


            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                
                var connectionString = Environment.GetEnvironmentVariable("CUSTOMCONNSTR_DEFAULT_CONNECTION");

                
                if (string.IsNullOrWhiteSpace(connectionString))
                    connectionString = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION");

                
                if (string.IsNullOrWhiteSpace(connectionString))
                    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

                
                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new Exception("❌ Nenhuma connection string foi encontrada. Verifique as variáveis de ambiente ou o appsettings.json.");

                
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<UsuarioService>();

            builder.Services.AddScoped<IQuestionarioRepository, QuestionarioRepository>();
            builder.Services.AddScoped<QuestionarioService>();

            builder.Services.AddAuthorization();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });


            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });


            builder.Services.AddHealthChecks()
                .AddCheck<ApplicationHealthCheck>(
                    "Application",
                    failureStatus: HealthStatus.Degraded,
                    tags: new[] { "application", "internal" }
                );
                

            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(ui =>
                {
                    ui.SwaggerEndpoint("/swagger/v1/swagger.json", "Health-Hub.API v1");
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();


            app.MapHealthChecks("/health", new HealthCheckOptions()
            {
                ResponseWriter = HealthCheckExtensions.WriteResponse,
                Predicate = check => check.Tags.Contains("application") ||
                                     check.Tags.Contains("external")
            });

            
            app.MapHealthChecks("/health/live", new HealthCheckOptions()
            {
                ResponseWriter = HealthCheckExtensions.WriteResponse,
                Predicate = check => check.Tags.Contains("application")
            });

            app.Run();
        }
    }
}
