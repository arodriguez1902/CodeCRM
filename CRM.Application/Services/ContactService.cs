using CRM.Application.Interfaces;
using CRM.Application.DTOs;
using CRM.Domain.Entities;

namespace CRM.Application.Services
{
    public class ContactService : IContactService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContactService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ContactDTO> GetContactByIdAsync(int id)
        {
            var contact = await _unitOfWork.GetContactWithDetailsAsync(id);
            if (contact == null) throw new KeyNotFoundException("Contacto no encontrado");

            return MapToDTO(contact);
        }

        public async Task<IEnumerable<ContactDTO>> GetAllContactsAsync()
        {
            var contacts = await _unitOfWork.GetContactsWithDetailsAsync();
            return contacts.Select(MapToDTO);
        }

        public async Task<IEnumerable<ContactDTO>> GetContactsByAccountAsync(int accountId)
        {
            var contacts = await _unitOfWork.GetContactsByAccountWithDetailsAsync(accountId);
            return contacts.Select(MapToDTO);
        }

        public async Task<IEnumerable<ContactDTO>> GetContactsByAssignedUserAsync(int userId)
        {
            var contacts = await _unitOfWork.GetContactsByAssignedUserWithDetailsAsync(userId);
            return contacts.Select(MapToDTO);
        }

        public async Task<ContactDTO> CreateContactAsync(CreateContactDTO createContactDto, int createdByUserId)
        {
            var contact = new Contact
            {
                FirstName = createContactDto.FirstName,
                LastName = createContactDto.LastName,
                Position = createContactDto.Position,
                Department = createContactDto.Department,
                Email = createContactDto.Email,
                Phone = createContactDto.Phone,
                Mobile = createContactDto.Mobile,
                DateOfBirth = createContactDto.DateOfBirth,
                AccountId = createContactDto.AccountId,
                AssignedToUserId = createContactDto.AssignedToUserId,
                IsPrimaryContact = createContactDto.IsPrimaryContact,
                CreatedByUserId = createdByUserId,
                IsActive = true
            };

            // Si es contacto primario, desmarcar otros contactos primarios de la misma cuenta
            if (createContactDto.IsPrimaryContact && createContactDto.AccountId.HasValue)
            {
                await ResetPrimaryContactsAsync(createContactDto.AccountId.Value);
            }

            await _unitOfWork.Contacts.AddAsync(contact);
            await _unitOfWork.CompleteAsync();

            return await GetContactByIdAsync(contact.Id);
        }

        public async Task<ContactDTO> UpdateContactAsync(int id, UpdateContactDTO updateContactDto)
        {
            var contact = await _unitOfWork.Contacts.GetByIdAsync(id);
            if (contact == null) throw new KeyNotFoundException("Contacto no encontrado");

            // Actualizar propiedades
            contact.FirstName = updateContactDto.FirstName;
            contact.LastName = updateContactDto.LastName;
            contact.Position = updateContactDto.Position;
            contact.Department = updateContactDto.Department;
            contact.Email = updateContactDto.Email;
            contact.Phone = updateContactDto.Phone;
            contact.Mobile = updateContactDto.Mobile;
            contact.DateOfBirth = updateContactDto.DateOfBirth;
            contact.AccountId = updateContactDto.AccountId;
            contact.AssignedToUserId = updateContactDto.AssignedToUserId;
            contact.IsPrimaryContact = updateContactDto.IsPrimaryContact;

            // Si se marca como primario, desmarcar otros
            if (updateContactDto.IsPrimaryContact && updateContactDto.AccountId.HasValue)
            {
                await ResetPrimaryContactsAsync(updateContactDto.AccountId.Value, id);
            }

            await _unitOfWork.Contacts.UpdateAsync(contact);
            await _unitOfWork.CompleteAsync();

            return await GetContactByIdAsync(contact.Id);
        }

        public async Task<bool> DeleteContactAsync(int id)
        {
            var contact = await _unitOfWork.Contacts.GetByIdAsync(id);
            if (contact == null) throw new KeyNotFoundException("Contacto no encontrado");

            contact.IsActive = false;
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<ContactDTO> SetAsPrimaryContactAsync(int contactId, int accountId)
        {
            var contact = await _unitOfWork.Contacts.GetByIdAsync(contactId);
            if (contact == null) throw new KeyNotFoundException("Contacto no encontrado");

            // Verificar que el contacto pertenece a la cuenta
            if (contact.AccountId != accountId)
            {
                throw new InvalidOperationException("El contacto no pertenece a la cuenta especificada");
            }

            // Desmarcar otros contactos primarios
            await ResetPrimaryContactsAsync(accountId, contactId);

            // Marcar este contacto como primario
            contact.IsPrimaryContact = true;
            await _unitOfWork.CompleteAsync();

            return await GetContactByIdAsync(contactId);
        }

        private async Task ResetPrimaryContactsAsync(int accountId, int? excludeContactId = null)
        {
            var primaryContacts = await _unitOfWork.Contacts.FindAsync(c => 
                c.AccountId == accountId && 
                c.IsPrimaryContact && 
                c.Id != excludeContactId &&
                c.IsActive);

            foreach (var primaryContact in primaryContacts)
            {
                primaryContact.IsPrimaryContact = false;
            }
        }

        private ContactDTO MapToDTO(Contact contact)
        {
            return new ContactDTO
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Position = contact.Position,
                Department = contact.Department,
                Email = contact.Email,
                Phone = contact.Phone,
                Mobile = contact.Mobile,
                DateOfBirth = contact.DateOfBirth,
                IsPrimaryContact = contact.IsPrimaryContact,
                Account = contact.Account?.Name ?? "Sin cuenta",
                AssignedToUser = contact.AssignedToUser != null 
                    ? $"{contact.AssignedToUser.FirstName} {contact.AssignedToUser.LastName}" 
                    : "No asignado",
                CreatedByUser = contact.CreatedByUser != null
                    ? $"{contact.CreatedByUser.FirstName} {contact.CreatedByUser.LastName}"
                    : "Sistema",
                CreatedAt = contact.CreatedAt
            };
        }
    }
}