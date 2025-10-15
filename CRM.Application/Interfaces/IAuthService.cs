using CRM.Application.DTOs;

namespace CRM.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> LoginAsync(LoginDTO loginDto);
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDto);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> UserExistsAsync(string username, string email);
    }
}