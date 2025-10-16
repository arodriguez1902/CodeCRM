using CRM.Application.DTOs;

namespace CRM.Application.Interfaces
{
    public interface IContactService
    {
        Task<ContactDTO> GetContactByIdAsync(int id);
        Task<IEnumerable<ContactDTO>> GetAllContactsAsync();
        Task<IEnumerable<ContactDTO>> GetContactsByAccountAsync(int accountId);
        Task<IEnumerable<ContactDTO>> GetContactsByAssignedUserAsync(int userId);
        Task<ContactDTO> CreateContactAsync(CreateContactDTO createContactDto, int createdByUserId);
        Task<ContactDTO> UpdateContactAsync(int id, UpdateContactDTO updateContactDto);
        Task<bool> DeleteContactAsync(int id);
        Task<ContactDTO> SetAsPrimaryContactAsync(int contactId, int accountId);
    }
}