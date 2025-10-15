using FluentValidation;
using CRM.Application.DependencyInjection;
using CRM.Infrastructure.DependencyInjection;
using CRM.Shared.Models;
using CRM.Shared.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Swashbuckle.AspNetCore.SwaggerGen;
using CRM.Application.Validators;

namespace CRM.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        // RENOMBRAR este método para evitar conflicto
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Controllers
            services.AddControllers();

            // FluentValidation 12.0.0 + .NET 9
            services.AddValidatorsFromAssemblyContaining<LoginDTOValidator>();

            // Swagger/OpenAPI
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c => ConfigureSwagger(c));

            // JWT Configuration
            var jwtSettings = ConfigureJwt(services, configuration);

            // Authentication
            services.AddAuthenticationServices(jwtSettings);

            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Application and Infrastructure Services
            services.AddApplicationServices(); // Desde CRM.Application
            services.AddInfrastructureServices(configuration); // Desde CRM.Infrastructure

            return services;
        }

        private static void ConfigureSwagger(SwaggerGenOptions c)
        {
            c.SwaggerDoc("v1", new OpenApiInfo { 
                Title = "CRM Profesional API", 
                Version = "v1",
                Description = "API para el sistema CRM Profesional",
                Contact = new OpenApiContact
                {
                    Name = "Soporte CRM",
                    Email = "soporte@crmprofesional.com"
                }
            });
            
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        }

        private static JwtSettings ConfigureJwt(IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.GetSection("JwtSettings").Bind(jwtSettings);
            services.AddSingleton(jwtSettings);
            services.AddSingleton<JwtHelper>();
            return jwtSettings;
        }

        private static void AddAuthenticationServices(this IServiceCollection services, JwtSettings jwtSettings)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                        ClockSkew = TimeSpan.Zero
                    };
                    
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}