using CRM.Application.Interfaces;
using CRM.Application.DTOs;
using CRM.Domain.Entities;

namespace CRM.Application.Services
{
    public class LeadService : ILeadService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeadService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<LeadDTO> GetLeadByIdAsync(int id)
        {
            var lead = await _unitOfWork.GetLeadWithDetailsAsync(id);
            if (lead == null) throw new KeyNotFoundException("Lead no encontrado");

            return MapToDTO(lead);
        }

        public async Task<IEnumerable<LeadDTO>> GetAllLeadsAsync()
        {
            var leads = await _unitOfWork.GetLeadsWithDetailsAsync();
            return leads.Select(MapToDTO);
        }

        public async Task<IEnumerable<LeadDTO>> GetLeadsByAssignedUserAsync(int userId)
        {
            var leads = await _unitOfWork.GetLeadsByAssignedUserWithDetailsAsync(userId);
            return leads.Select(MapToDTO);
        }

        public async Task<LeadDTO> CreateLeadAsync(CreateLeadDTO createLeadDto, int createdByUserId)
        {
            var lead = new Lead
            {
                FirstName = createLeadDto.FirstName,
                LastName = createLeadDto.LastName,
                Company = createLeadDto.Company,
                Email = createLeadDto.Email,
                Phone = createLeadDto.Phone,
                Website = createLeadDto.Website,
                LeadSourceId = createLeadDto.LeadSourceId,
                IndustryId = createLeadDto.IndustryId,
                AssignedToUserId = createLeadDto.AssignedToUserId,
                CreatedByUserId = createdByUserId,
                Status = LeadStatus.New,
                Rating = LeadRating.Cold,
                IsActive = true
            };

            await _unitOfWork.Leads.AddAsync(lead);
            await _unitOfWork.CompleteAsync();

            return await GetLeadByIdAsync(lead.Id);
        }

        public async Task<LeadDTO> UpdateLeadAsync(int id, UpdateLeadDTO updateLeadDto)
        {
            var lead = await _unitOfWork.Leads.GetByIdAsync(id);
            if (lead == null) throw new KeyNotFoundException("Lead no encontrado");

            // Actualizar propiedades
            lead.FirstName = updateLeadDto.FirstName;
            lead.LastName = updateLeadDto.LastName;
            lead.Company = updateLeadDto.Company;
            lead.Email = updateLeadDto.Email;
            lead.Phone = updateLeadDto.Phone;
            lead.Website = updateLeadDto.Website;
            lead.LeadSourceId = updateLeadDto.LeadSourceId;
            lead.IndustryId = updateLeadDto.IndustryId;
            lead.AssignedToUserId = updateLeadDto.AssignedToUserId;
            
            // Parsear enums
            if (Enum.TryParse<LeadStatus>(updateLeadDto.Status, out var status))
                lead.Status = status;
            
            if (Enum.TryParse<LeadRating>(updateLeadDto.Rating, out var rating))
                lead.Rating = rating;

            await _unitOfWork.Leads.UpdateAsync(lead);
            await _unitOfWork.CompleteAsync();

            return await GetLeadByIdAsync(lead.Id);
        }

        public async Task<bool> DeleteLeadAsync(int id)
        {
            var lead = await _unitOfWork.Leads.GetByIdAsync(id);
            if (lead == null) throw new KeyNotFoundException("Lead no encontrado");

            lead.IsActive = false;
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<LeadDTO> ConvertLeadToContactAsync(int leadId, int accountId)
        {
            var lead = await _unitOfWork.GetLeadWithDetailsAsync(leadId);
            if (lead == null) throw new KeyNotFoundException("Lead no encontrado");

            var account = await _unitOfWork.Accounts.GetByIdAsync(accountId);
            if (account == null) throw new KeyNotFoundException("Cuenta no encontrada");

            // Crear contacto a partir del lead
            var contact = new Contact
            {
                FirstName = lead.FirstName,
                LastName = lead.LastName,
                Email = lead.Email,
                Phone = lead.Phone,
                AccountId = accountId,
                AssignedToUserId = lead.AssignedToUserId,
                CreatedByUserId = lead.CreatedByUserId,
                IsActive = true
            };

            await _unitOfWork.Contacts.AddAsync(contact);
            
            // Actualizar lead como convertido
            lead.Status = LeadStatus.Converted;
            lead.ConvertedToContactId = contact.Id;
            lead.ConvertedToAccountId = accountId;
            lead.ConvertedAt = DateTime.UtcNow;

            await _unitOfWork.CompleteAsync();

            return await GetLeadByIdAsync(leadId);
        }

        public async Task<IEnumerable<LeadSourceDTO>> GetLeadSourcesAsync()
        {
            var sources = await _unitOfWork.LeadSources.GetAllAsync();
            return sources.Select(s => new LeadSourceDTO
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description
            });
        }

        public async Task<IEnumerable<IndustryDTO>> GetIndustriesAsync()
        {
            var industries = await _unitOfWork.Industries.GetAllAsync();
            return industries.Select(i => new IndustryDTO
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description
            });
        }

        private LeadDTO MapToDTO(Lead lead)
        {
            // AHORA ES SIMPLE - las relaciones ya vienen cargadas
            return new LeadDTO
            {
                Id = lead.Id,
                FirstName = lead.FirstName,
                LastName = lead.LastName,
                Company = lead.Company,
                Email = lead.Email,
                Phone = lead.Phone,
                Website = lead.Website,
                Status = lead.Status.ToString(),
                Rating = lead.Rating.ToString(),
                LeadSource = lead.LeadSource?.Name ?? "No especificado",
                Industry = lead.Industry?.Name ?? "No especificado",
                AssignedToUser = lead.AssignedToUser != null 
                    ? $"{lead.AssignedToUser.FirstName} {lead.AssignedToUser.LastName}" 
                    : "No asignado",
                CreatedByUser = lead.CreatedByUser != null
                    ? $"{lead.CreatedByUser.FirstName} {lead.CreatedByUser.LastName}"
                    : "Sistema",
                CreatedAt = lead.CreatedAt
            };
        }
    }
}