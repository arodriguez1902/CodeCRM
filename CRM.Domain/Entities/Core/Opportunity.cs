using CRM.Domain.Common;

namespace CRM.Domain.Entities
{
    public class Opportunity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "PEN";
        public OpportunityStage Stage { get; set; } = OpportunityStage.Prospecting;
        public int Probability { get; set; } = 0; // 0-100%
        public DateTime CloseDate { get; set; }
        public string? LostReason { get; set; }
        
        // Foreign Keys
        public int AccountId { get; set; }
        public int? ContactId { get; set; }
        public int AssignedToUserId { get; set; }
        public int CreatedByUserId { get; set; }
        
        // Navigation Properties
        public virtual Account Account { get; set; } = null!;
        public virtual Contact? Contact { get; set; }
        public virtual User AssignedToUser { get; set; } = null!;
        public virtual User CreatedByUser { get; set; } = null!;
    }
    
    public enum OpportunityStage
    {
        Prospecting,
        Qualification,
        Proposal,
        Negotiation,
        ClosedWon,
        ClosedLost
    }
}