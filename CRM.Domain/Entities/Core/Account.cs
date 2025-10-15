using CRM.Domain.Common;

namespace CRM.Domain.Entities
{
    public class Account : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Website { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int? EmployeeCount { get; set; }
        public decimal? AnnualRevenue { get; set; }
        public string? Description { get; set; }
        public AccountType AccountType { get; set; } = AccountType.Customer;
        public AccountRating AccountRating { get; set; } = AccountRating.Cold;
        
        // Foreign Keys
        public int? IndustryId { get; set; }
        public int? AssignedToUserId { get; set; }
        public int CreatedByUserId { get; set; }
        
        // Navigation Properties
        public virtual Industry? Industry { get; set; }
        public virtual User? AssignedToUser { get; set; }
        public virtual User CreatedByUser { get; set; } = null!;
        public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
        public virtual ICollection<Opportunity> Opportunities { get; set; } = new List<Opportunity>();
    }
    
    public enum AccountType
    {
        Customer,
        Partner,
        Competitor,
        Vendor,
        Other
    }
    
    public enum AccountRating
    {
        Hot,
        Warm,
        Cold
    }
}