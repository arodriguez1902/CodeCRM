using System.ComponentModel.DataAnnotations;

namespace CRM.Application.DTOs
{
    public class ContactDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Position { get; set; }
        public string? Department { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool IsPrimaryContact { get; set; }
        public string? Account { get; set; }
        public string? AssignedToUser { get; set; }
        public string CreatedByUser { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateContactDTO
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        public string LastName { get; set; } = string.Empty;
        
        public string? Position { get; set; }
        public string? Department { get; set; }
        
        [EmailAddress]
        public string? Email { get; set; }
        
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? AccountId { get; set; }
        public int? AssignedToUserId { get; set; }
        public bool IsPrimaryContact { get; set; }
    }

    public class UpdateContactDTO
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        public string LastName { get; set; } = string.Empty;
        
        public string? Position { get; set; }
        public string? Department { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? AccountId { get; set; }
        public int? AssignedToUserId { get; set; }
        public bool IsPrimaryContact { get; set; }
    }
}