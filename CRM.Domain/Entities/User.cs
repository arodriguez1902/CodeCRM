using CRM.Domain.Common;

namespace CRM.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? AvatarUrl { get; set; }
        public string Timezone { get; set; } = "UTC";
        public string Language { get; set; } = "es";
        public bool EmailVerified { get; set; } = false;
        public DateTime? LastLoginAt { get; set; }
        
        // Foreign Keys
        public int RoleId { get; set; }
        
        // Navigation Properties
        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Lead> Leads { get; set; } = new List<Lead>();
        public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}