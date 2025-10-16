using CRM.Domain.Entities;

namespace CRM.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Repositories básicos
        IGenericRepository<User> Users { get; }
        IGenericRepository<Role> Roles { get; }
        IGenericRepository<Permission> Permissions { get; }
        IGenericRepository<Lead> Leads { get; }
        IGenericRepository<Contact> Contacts { get; }
        IGenericRepository<Account> Accounts { get; }
        IGenericRepository<Industry> Industries { get; }
        IGenericRepository<LeadSource> LeadSources { get; }
        IGenericRepository<Opportunity> Opportunities { get; }

        // Métodos específicos con relaciones
        Task<Lead?> GetLeadWithDetailsAsync(int id);
        Task<IEnumerable<Lead>> GetLeadsWithDetailsAsync();
        Task<IEnumerable<Lead>> GetLeadsByAssignedUserWithDetailsAsync(int userId);
        Task<User?> GetUserWithRoleAsync(int id);
        Task<IEnumerable<User>> GetUsersWithRolesAsync();
        Task<Account?> GetAccountWithDetailsAsync(int id);
        Task<Contact?> GetContactWithDetailsAsync(int id);
        Task<IEnumerable<Contact>> GetContactsWithDetailsAsync();
        Task<IEnumerable<Contact>> GetContactsByAccountWithDetailsAsync(int accountId);
        Task<IEnumerable<Contact>> GetContactsByAssignedUserWithDetailsAsync(int userId);

        Task<int> CompleteAsync();
    }
}