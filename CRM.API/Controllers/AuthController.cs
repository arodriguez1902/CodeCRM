using Microsoft.AspNetCore.Mvc;
using CRM.Application.Interfaces;
using CRM.Application.DTOs;

namespace CRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO loginDto)
        {
            try
            {
                var result = await _authService.LoginAsync(loginDto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error durante el login: " + ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDTO>> Register(RegisterDTO registerDto)
        {
            try
            {
                var result = await _authService.RegisterAsync(registerDto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error durante el registro: " + ex.Message });
            }
        }

        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDto)
        {
            try
            {
                // Obtener el ID del usuario del token (implementaremos esto después)
                var userId = 1; // Temporal - lo reemplazaremos con el usuario autenticado
                
                var result = await _authService.ChangePasswordAsync(
                    userId, 
                    changePasswordDto.CurrentPassword, 
                    changePasswordDto.NewPassword
                );
                
                return Ok(new { message = "Contraseña cambiada exitosamente" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al cambiar la contraseña: " + ex.Message });
            }
        }
    }
}