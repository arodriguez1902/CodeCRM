using System.ComponentModel.DataAnnotations;

namespace CRM.Application.DTOs
{
    public class UserUpdateDTO
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        public string LastName { get; set; } = string.Empty;
        
        [EmailAddress]
        public string? Email { get; set; }
        
        public string? Phone { get; set; }
        public string? AvatarUrl { get; set; }
        public string Timezone { get; set; } = "UTC";
        public string Language { get; set; } = "es";
        public int RoleId { get; set; }
    }

    public class ChangePasswordDTO
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;
        
        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;
    }
}