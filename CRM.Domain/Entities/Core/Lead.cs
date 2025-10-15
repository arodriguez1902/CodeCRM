using CRM.Domain.Common;

namespace CRM.Domain.Entities
{
    public class Lead : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Company { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public int? NumberOfEmployees { get; set; }
        public decimal? AnnualRevenue { get; set; }
        public LeadStatus Status { get; set; } = LeadStatus.New;
        public LeadRating Rating { get; set; } = LeadRating.Cold;
        public string? Description { get; set; }
        public DateTime? ConvertedAt { get; set; }
        
        // Foreign Keys
        public int? LeadSourceId { get; set; }
        public int? IndustryId { get; set; }
        public int? AssignedToUserId { get; set; }
        public int CreatedByUserId { get; set; }
        public int? ConvertedToAccountId { get; set; }
        public int? ConvertedToContactId { get; set; }
        
        // Navigation Properties
        public virtual LeadSource? LeadSource { get; set; }
        public virtual Industry? Industry { get; set; }
        public virtual User? AssignedToUser { get; set; }
        public virtual User CreatedByUser { get; set; } = null!;
        public virtual Account? ConvertedToAccount { get; set; }
        public virtual Contact? ConvertedToContact { get; set; }
    }
    
    public enum LeadStatus
    {
        New,
        Contacted,
        Qualified,
        Unqualified,
        Converted
    }
    
    public enum LeadRating
    {
        Hot,
        Warm,
        Cold
    }
}