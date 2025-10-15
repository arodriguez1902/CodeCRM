using CRM.Application.Interfaces;
using CRM.Application.DTOs;
using CRM.Domain.Entities;
using CRM.Shared.Helpers;
using CRM.Shared.Models;

namespace CRM.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtHelper _jwtHelper;

        public AuthService(IUnitOfWork unitOfWork, JwtHelper jwtHelper)
        {
            _unitOfWork = unitOfWork;
            _jwtHelper = jwtHelper;
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO loginDto)
        {
            // Buscar usuario con Role incluido
            var users = await _unitOfWork.Users.FindWithIncludeAsync(
                u => u.Username == loginDto.UsernameOrEmail || u.Email == loginDto.UsernameOrEmail,
                u => u.Role!
            );

            var userEntity = users.FirstOrDefault();
            if (userEntity == null || !PasswordHasher.VerifyPassword(loginDto.Password, userEntity.PasswordHash))
            {
                throw new UnauthorizedAccessException("Credenciales inválidas");
            }

            if (!userEntity.IsActive)
            {
                throw new UnauthorizedAccessException("Usuario inactivo");
            }

            // Actualizar último login
            userEntity.LastLoginAt = DateTime.UtcNow;
            await _unitOfWork.CompleteAsync();

            // Generar token - ahora userEntity.Role está cargado
            var token = _jwtHelper.GenerateToken(userEntity);

            return new AuthResponseDTO
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(60),
                User = MapToUserDTO(userEntity)
            };
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDto)
        {
            // Verificar si el usuario ya existe
            if (await UserExistsAsync(registerDto.Username, registerDto.Email))
            {
                throw new InvalidOperationException("El usuario o email ya existe");
            }

            // Obtener rol por defecto
            var roles = await _unitOfWork.Roles.GetAllAsync();
            var defaultRole = roles.FirstOrDefault(r => r.Name == "Vendedor");

            if (defaultRole == null)
            {
                throw new InvalidOperationException("No se encontró el rol por defecto");
            }

            // Crear nuevo usuario
            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = PasswordHasher.HashPassword(registerDto.Password),
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Phone = registerDto.Phone,
                RoleId = defaultRole.Id,
                IsActive = true,
                EmailVerified = false
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            // Obtener usuario con Role cargado
            var userWithRole = await _unitOfWork.GetUserWithRoleAsync(user.Id);
            if (userWithRole == null)
            {
                throw new InvalidOperationException("Error al recuperar el usuario recién creado");
            }

            // Generar token
            var token = _jwtHelper.GenerateToken(userWithRole);

            return new AuthResponseDTO
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(60),
                User = MapToUserDTO(userWithRole)
            };
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado");
            }

            if (!PasswordHasher.VerifyPassword(currentPassword, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Contraseña actual incorrecta");
            }

            user.PasswordHash = PasswordHasher.HashPassword(newPassword);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> UserExistsAsync(string username, string email)
        {
            var existingUser = await _unitOfWork.Users.FindAsync(u => 
                u.Username == username || u.Email == email);
            return existingUser.Any();
        }

        private UserDTO MapToUserDTO(User user)
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