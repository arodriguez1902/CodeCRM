using Microsoft.Extensions.DependencyInjection;
using CRM.Application.Interfaces;
using CRM.Application.Services;

namespace CRM.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILeadService, LeadService>();
            return services;
        }
    }
}