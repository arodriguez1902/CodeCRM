using CRM.Application.DTOs;

namespace CRM.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByIdAsync(int id);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> UpdateUserAsync(int id, UserUpdateDTO userUpdateDto);
        Task<bool> DeleteUserAsync(int id);
    }
}