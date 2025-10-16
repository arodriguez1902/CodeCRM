using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CRM.Application.Interfaces;
using CRM.Application.DTOs;

namespace CRM.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactDTO>>> GetAllContacts()
        {
            try
            {
                var contacts = await _contactService.GetAllContactsAsync();
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener contactos: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContactDTO>> GetContactById(int id)
        {
            try
            {
                var contact = await _contactService.GetContactByIdAsync(id);
                return Ok(contact);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener el contacto: {ex.Message}" });
            }
        }

        [HttpGet("account/{accountId}")]
        public async Task<ActionResult<IEnumerable<ContactDTO>>> GetContactsByAccount(int accountId)
        {
            try
            {
                var contacts = await _contactService.GetContactsByAccountAsync(accountId);
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener contactos de la cuenta: {ex.Message}" });
            }
        }

        [HttpGet("assigned/{userId}")]
        public async Task<ActionResult<IEnumerable<ContactDTO>>> GetContactsByAssignedUser(int userId)
        {
            try
            {
                var contacts = await _contactService.GetContactsByAssignedUserAsync(userId);
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener contactos del usuario: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ContactDTO>> CreateContact(CreateContactDTO createContactDto)
        {
            try
            {
                // Obtener ID del usuario autenticado
                var createdByUserId = 1; // Temporal - reemplazar con User.Identity.Name
                
                var contact = await _contactService.CreateContactAsync(createContactDto, createdByUserId);
                return CreatedAtAction(nameof(GetContactById), new { id = contact.Id }, contact);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al crear el contacto: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ContactDTO>> UpdateContact(int id, UpdateContactDTO updateContactDto)
        {
            try
            {
                var contact = await _contactService.UpdateContactAsync(id, updateContactDto);
                return Ok(contact);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al actualizar el contacto: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteContact(int id)
        {
            try
            {
                await _contactService.DeleteContactAsync(id);
                return Ok(new { message = "Contacto eliminado exitosamente" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al eliminar el contacto: {ex.Message}" });
            }
        }

        [HttpPost("{contactId}/set-primary/{accountId}")]
        public async Task<ActionResult<ContactDTO>> SetAsPrimaryContact(int contactId, int accountId)
        {
            try
            {
                var contact = await _contactService.SetAsPrimaryContactAsync(contactId, accountId);
                return Ok(contact);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al establecer contacto primario: {ex.Message}" });
            }
        }
    }
}