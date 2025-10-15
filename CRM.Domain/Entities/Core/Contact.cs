using CRM.Domain.Common;

namespace CRM.Domain.Entities
{
    public class Contact : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Position { get; set; }
        public string? Department { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool IsPrimaryContact { get; set; } = false;
        
        // Foreign Keys
        public int? AccountId { get; set; }
        public int? AssignedToUserId { get; set; }
        public int CreatedByUserId { get; set; }
        
        // Navigation Properties
        public virtual Account? Account { get; set; }
        public virtual User? AssignedToUser { get; set; }
        public virtual User CreatedByUser { get; set; } = null!;
    }
}