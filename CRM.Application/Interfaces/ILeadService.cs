using CRM.Application.DTOs;

namespace CRM.Application.Interfaces
{
    public interface ILeadService
    {
        Task<LeadDTO> GetLeadByIdAsync(int id);
        Task<IEnumerable<LeadDTO>> GetAllLeadsAsync();
        Task<IEnumerable<LeadDTO>> GetLeadsByAssignedUserAsync(int userId);
        Task<LeadDTO> CreateLeadAsync(CreateLeadDTO createLeadDto, int createdByUserId);
        Task<LeadDTO> UpdateLeadAsync(int id, UpdateLeadDTO updateLeadDto);
        Task<bool> DeleteLeadAsync(int id);
        Task<LeadDTO> ConvertLeadToContactAsync(int leadId, int accountId);
        Task<IEnumerable<LeadSourceDTO>> GetLeadSourcesAsync();
        Task<IEnumerable<IndustryDTO>> GetIndustriesAsync();
    }
}