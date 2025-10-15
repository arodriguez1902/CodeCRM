using CRM.Domain.Common;

namespace CRM.Domain.Entities
{
    public class Industry : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        
        // Navigation Properties
        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
        public virtual ICollection<Lead> Leads { get; set; } = new List<Lead>();
    }
}