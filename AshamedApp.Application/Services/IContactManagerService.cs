using AshamedApp.Application.DTOs;

namespace AshamedApp.Application.Services;

public interface IContactManagerService
{
    GetAllContactsResponse GetAllContacts(); 
    // Task<ContactDto> GetContactByIdAsync(int id);
    // Task AddContactAsync(ContactDto contact);
    // Task UpdateContactAsync(ContactDto contact);
    // Task DeleteContactAsync(int id);
}