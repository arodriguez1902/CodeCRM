using CRM.Domain.Entities;

namespace CRM.Infrastructure.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Roles
            if (!context.Roles.Any())
            {
                var roles = new List<Role>
                {
                    new Role { Name = "Administrador", Description = "Acceso completo al sistema", HierarchyLevel = 1 },
                    new Role { Name = "Gerente", Description = "Puede gestionar equipos y ver reportes", HierarchyLevel = 2 },
                    new Role { Name = "Vendedor", Description = "Puede gestionar leads, contactos y oportunidades", HierarchyLevel = 3 },
                    new Role { Name = "Soporte", Description = "Puede gestionar tickets y casos de soporte", HierarchyLevel = 3 },
                    new Role { Name = "Marketing", Description = "Puede gestionar campañas y formularios", HierarchyLevel = 3 }
                };

                context.Roles.AddRange(roles);
            }

            // Permissions
            if (!context.Permissions.Any())
            {
                var permissions = new List<Permission>
                {
                    new Permission { Module = "Users", Action = "View", Description = "Ver usuarios" },
                    new Permission { Module = "Users", Action = "Create", Description = "Crear usuarios" },
                    new Permission { Module = "Users", Action = "Edit", Description = "Editar usuarios" },
                    new Permission { Module = "Users", Action = "Delete", Description = "Eliminar usuarios" },
                    new Permission { Module = "Leads", Action = "View", Description = "Ver leads" },
                    new Permission { Module = "Leads", Action = "Create", Description = "Crear leads" },
                    new Permission { Module = "Leads", Action = "Edit", Description = "Editar leads" },
                    new Permission { Module = "Leads", Action = "Delete", Description = "Eliminar leads" },
                    new Permission { Module = "Contacts", Action = "View", Description = "Ver contactos" },
                    new Permission { Module = "Contacts", Action = "Create", Description = "Crear contactos" },
                    new Permission { Module = "Contacts", Action = "Edit", Description = "Editar contactos" },
                    new Permission { Module = "Contacts", Action = "Delete", Description = "Eliminar contactos" }
                };

                context.Permissions.AddRange(permissions);
            }

            // Lead Sources
            if (!context.LeadSources.Any())
            {
                var leadSources = new List<LeadSource>
                {
                    new LeadSource { Name = "Web", Description = "Formularios del sitio web" },
                    new LeadSource { Name = "Referido", Description = "Recomendación de clientes" },
                    new LeadSource { Name = "Redes Sociales", Description = "Facebook, LinkedIn, etc." },
                    new LeadSource { Name = "Publicidad", Description = "Google Ads, Facebook Ads" },
                    new LeadSource { Name = "Eventos", Description = "Ferias, conferencias" },
                    new LeadSource { Name = "Email Marketing", Description = "Campañas de email" },
                    new LeadSource { Name = "Llamada en Frío", Description = "Prospección telefónica" }
                };

                context.LeadSources.AddRange(leadSources);
            }

            // Industries
            if (!context.Industries.Any())
            {
                var industries = new List<Industry>
                {
                    new Industry { Name = "Tecnología", Description = "Empresas de tecnología y software" },
                    new Industry { Name = "Salud", Description = "Hospitales, clínicas y servicios médicos" },
                    new Industry { Name = "Educación", Description = "Instituciones educativas" },
                    new Industry { Name = "Finanzas", Description = "Bancos, seguros y servicios financieros" },
                    new Industry { Name = "Retail", Description = "Comercio minorista" },
                    new Industry { Name = "Manufactura", Description = "Fabricación y producción" },
                    new Industry { Name = "Consultoría", Description = "Servicios profesionales" },
                    new Industry { Name = "Construcción", Description = "Construcción e ingeniería" }
                };

                context.Industries.AddRange(industries);
            }

            context.SaveChanges();
        }
    }
}