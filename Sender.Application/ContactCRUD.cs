using Sender.Application.Interfaces;
using Sender.Domain;

namespace Sender.Application
{
    public class ContactCRUD
    {
        private readonly IContactRepository _contactRepository;

        public ContactCRUD(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<Result<Contact>> CreateContactAsync(Guid userId, string name, string? email, string? telegramUsername)
        {
            var contact = new Contact
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                TelegramUsername = telegramUsername,

                UserId = userId,
            };

            await _contactRepository.AddAsync(contact);

            return Result<Contact>.Success(contact, 201);
        }

        public async Task<Result<Contact>> GetOneContactAsync(Guid userId, Guid contactId)
        {
            Contact contact = await _contactRepository.GetByIdAsync(contactId);


            if (contact == null)
            {
                return Result<Contact>.Failure("Не вдалося знайти контакт", 404);
            }

            if (contact.UserId != userId)
            {
                return Result<Contact>.Failure("Цей контакт не ваш", 403);
            }

            return Result<Contact>.Success(contact);
        }

        public async Task<Result<List<Contact>>> GetAllContactAsync(Guid userId)
        {
            List<Contact> contacts = await _contactRepository.GetAllByUserIdAsync(userId);

            return Result<List<Contact>>.Success(contacts);
        }


        public async Task<Result<Contact>> UpdateContactAsync(Guid contactId, Guid userId, string? name, string? email, string? telegramUsername)
        {
            var contact = await _contactRepository.GetByIdAsync(contactId);

            if (contact == null)
            {
                return Result<Contact>.Failure("Не вдалося знайти контакт", 404);
            }

            if (contact.UserId != userId)
            {
                return Result<Contact>.Failure("Цей контакт не ваш", 403);
            }

            contact.Name = name ?? contact.Name;
            contact.Email = email ?? contact.Email;
            contact.TelegramUsername = telegramUsername ?? contact.TelegramUsername;

            await _contactRepository.UpdateAsync(contact);

            return Result<Contact>.Success(contact);
        }

        public async Task<Result<Unit>> DeleteContactAsync(Guid userId, Guid contactId)
        {
            Contact contact = await _contactRepository.GetByIdAsync(contactId);


            if (contact == null)
                return Result<Unit>.Failure("Не вдалося знайти контакт", 404);

            if (contact.UserId != userId)
                return Result<Unit>.Failure("Цей контакт не ваш", 403);

            await _contactRepository.RemoveAsync(contact);

            return Result<Unit>.Success();
        }

    }
}
