using System.ComponentModel.DataAnnotations;

namespace CRM.Application.DTOs
{
    public class LeadDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Company { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Rating { get; set; } = string.Empty;
        public string? LeadSource { get; set; }
        public string? Industry { get; set; }
        public string? AssignedToUser { get; set; }
        public string CreatedByUser { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateLeadDTO
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        public string LastName { get; set; } = string.Empty;
        
        public string? Company { get; set; }
        
        [EmailAddress]
        public string? Email { get; set; }
        
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public int? LeadSourceId { get; set; }
        public int? IndustryId { get; set; }
        public int? AssignedToUserId { get; set; }
    }

    public class UpdateLeadDTO
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        public string LastName { get; set; } = string.Empty;
        
        public string? Company { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public int? LeadSourceId { get; set; }
        public int? IndustryId { get; set; }
        public int? AssignedToUserId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Rating { get; set; } = string.Empty;
    }

    public class LeadSourceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class IndustryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}