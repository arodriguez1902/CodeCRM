using CRM.Application.Interfaces;
using CRM.Infrastructure.Data;
using CRM.Infrastructure.Repositories;
using CRM.Domain.Entities;
using System.Linq.Expressions;

namespace CRM.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        // Repositories (existentes)
        private GenericRepository<User>? _users;
        private GenericRepository<Role>? _roles;
        private GenericRepository<Permission>? _permissions;
        private GenericRepository<Lead>? _leads;
        private GenericRepository<Contact>? _contacts;
        private GenericRepository<Account>? _accounts;
        private GenericRepository<Industry>? _industries;
        private GenericRepository<LeadSource>? _leadSources;
        private GenericRepository<Opportunity>? _opportunities;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        // PROPIEDADES BÁSICAS (existentes)
        public IGenericRepository<User> Users => _users ??= new GenericRepository<User>(_context);
        public IGenericRepository<Role> Roles => _roles ??= new GenericRepository<Role>(_context);
        public IGenericRepository<Permission> Permissions => _permissions ??= new GenericRepository<Permission>(_context);
        public IGenericRepository<Lead> Leads => _leads ??= new GenericRepository<Lead>(_context);
        public IGenericRepository<Contact> Contacts => _contacts ??= new GenericRepository<Contact>(_context);
        public IGenericRepository<Account> Accounts => _accounts ??= new GenericRepository<Account>(_context);
        public IGenericRepository<Industry> Industries => _industries ??= new GenericRepository<Industry>(_context);
        public IGenericRepository<LeadSource> LeadSources => _leadSources ??= new GenericRepository<LeadSource>(_context);
        public IGenericRepository<Opportunity> Opportunities => _opportunities ??= new GenericRepository<Opportunity>(_context);

        // MÉTODOS ESPECÍFICOS CON RELACIONES CARGADAS
        public async Task<Lead?> GetLeadWithDetailsAsync(int id)
        {
            return await _leads.GetByIdWithIncludeAsync(id,
                l => l.LeadSource!,
                l => l.Industry!,
                l => l.AssignedToUser!,
                l => l.CreatedByUser!
            );
        }

        public async Task<IEnumerable<Lead>> GetLeadsWithDetailsAsync()
        {
            return await _leads.GetAllWithIncludeAsync(
                l => l.LeadSource!,
                l => l.Industry!,
                l => l.AssignedToUser!,
                l => l.CreatedByUser!
            );
        }

        public async Task<IEnumerable<Lead>> GetLeadsByAssignedUserWithDetailsAsync(int userId)
        {
            return await _leads.FindWithIncludeAsync(
                l => l.AssignedToUserId == userId,
                l => l.LeadSource!,
                l => l.Industry!,
                l => l.AssignedToUser!,
                l => l.CreatedByUser!
            );
        }

        public async Task<User?> GetUserWithRoleAsync(int id)
        {
            return await _users.GetByIdWithIncludeAsync(id, u => u.Role!);
        }

        public async Task<IEnumerable<User>> GetUsersWithRolesAsync()
        {
            return await _users.GetAllWithIncludeAsync(u => u.Role!);
        }

        public async Task<Account?> GetAccountWithDetailsAsync(int id)
        {
            return await _accounts.GetByIdWithIncludeAsync(id,
                a => a.Industry!,
                a => a.AssignedToUser!,
                a => a.CreatedByUser!
            );
        }

        public async Task<Contact?> GetContactWithDetailsAsync(int id)
        {
            return await _contacts.GetByIdWithIncludeAsync(id,
                c => c.Account!,
                c => c.AssignedToUser!,
                c => c.CreatedByUser!
            );
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}