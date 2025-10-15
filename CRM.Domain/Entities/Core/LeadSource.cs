using CRM.Domain.Common;

namespace CRM.Domain.Entities
{
    public class LeadSource : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        
        // Navigation Properties
        public virtual ICollection<Lead> Leads { get; set; } = new List<Lead>();
    }
}