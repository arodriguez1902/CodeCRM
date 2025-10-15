using CRM.Application.Interfaces;
using CRM.Application.DTOs;
using CRM.Domain.Entities;

namespace CRM.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.GetUserWithRoleAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado");
            }

            return MapToDTO(user);
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.GetUsersWithRolesAsync();
            return users.Select(MapToDTO);
        }

        public async Task<UserDTO> UpdateUserAsync(int id, UserUpdateDTO userUpdateDto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado");
            }

            // Actualizar propiedades
            user.FirstName = userUpdateDto.FirstName;
            user.LastName = userUpdateDto.LastName;
            user.Phone = userUpdateDto.Phone;
            user.AvatarUrl = userUpdateDto.AvatarUrl;
            user.Timezone = userUpdateDto.Timezone;
            user.Language = userUpdateDto.Language;
            user.RoleId = userUpdateDto.RoleId;

            // Solo actualizar email si es diferente y no está en uso
            if (!string.IsNullOrEmpty(userUpdateDto.Email) && userUpdateDto.Email != user.Email)
            {
                var emailExists = await _unitOfWork.Users.FindAsync(u => u.Email == userUpdateDto.Email && u.Id != id);
                if (emailExists.Any())
                {
                    throw new InvalidOperationException("El email ya está en uso");
                }
                user.Email = userUpdateDto.Email;
            }

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();

            // Devolver usuario actualizado con Role
            var updatedUser = await _unitOfWork.GetUserWithRoleAsync(id);
            return MapToDTO(updatedUser!);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado");
            }

            // Soft delete
            user.IsActive = false;
            await _unitOfWork.CompleteAsync();

            return true;
        }

        private UserDTO MapToDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                AvatarUrl = user.AvatarUrl,
                Role = user.Role?.Name ?? "Sin rol",
                LastLoginAt = user.LastLoginAt
            };
        }
    }
}