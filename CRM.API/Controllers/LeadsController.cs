using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CRM.Application.Interfaces;
using CRM.Application.DTOs;

namespace CRM.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LeadsController : ControllerBase
    {
        private readonly ILeadService _leadService;

        public LeadsController(ILeadService leadService)
        {
            _leadService = leadService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeadDTO>>> GetAllLeads()
        {
            try
            {
                var leads = await _leadService.GetAllLeadsAsync();
                return Ok(leads);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener leads: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeadDTO>> GetLeadById(int id)
        {
            try
            {
                var lead = await _leadService.GetLeadByIdAsync(id);
                return Ok(lead);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener el lead: {ex.Message}" });
            }
        }

        [HttpGet("assigned/{userId}")]
        public async Task<ActionResult<IEnumerable<LeadDTO>>> GetLeadsByAssignedUser(int userId)
        {
            try
            {
                var leads = await _leadService.GetLeadsByAssignedUserAsync(userId);
                return Ok(leads);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener leads del usuario: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<LeadDTO>> CreateLead(CreateLeadDTO createLeadDto)
        {
            try
            {
                // Obtener ID del usuario autenticado (temporalmente hardcodeado)
                var createdByUserId = 1; // Reemplazar con User.Identity.Name cuando tengamos auth
                
                var lead = await _leadService.CreateLeadAsync(createLeadDto, createdByUserId);
                return CreatedAtAction(nameof(GetLeadById), new { id = lead.Id }, lead);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al crear el lead: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LeadDTO>> UpdateLead(int id, UpdateLeadDTO updateLeadDto)
        {
            try
            {
                var lead = await _leadService.UpdateLeadAsync(id, updateLeadDto);
                return Ok(lead);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al actualizar el lead: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLead(int id)
        {
            try
            {
                await _leadService.DeleteLeadAsync(id);
                return Ok(new { message = "Lead eliminado exitosamente" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al eliminar el lead: {ex.Message}" });
            }
        }

        [HttpPost("{leadId}/convert/{accountId}")]
        public async Task<ActionResult<LeadDTO>> ConvertLeadToContact(int leadId, int accountId)
        {
            try
            {
                var lead = await _leadService.ConvertLeadToContactAsync(leadId, accountId);
                return Ok(lead);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al convertir el lead: {ex.Message}" });
            }
        }

        [HttpGet("sources")]
        public async Task<ActionResult<IEnumerable<LeadSourceDTO>>> GetLeadSources()
        {
            try
            {
                var sources = await _leadService.GetLeadSourcesAsync();
                return Ok(sources);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener fuentes de leads: {ex.Message}" });
            }
        }

        [HttpGet("industries")]
        public async Task<ActionResult<IEnumerable<IndustryDTO>>> GetIndustries()
        {
            try
            {
                var industries = await _leadService.GetIndustriesAsync();
                return Ok(industries);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener industrias: {ex.Message}" });
            }
        }
    }
}