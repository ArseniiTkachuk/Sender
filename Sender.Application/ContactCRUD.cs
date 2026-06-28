using Sender.Application.Interfaces;
using Sender.Domain;

namespace Sender.Application
{
    public class ContactCRUD
    {

        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IContactRepository _contactRepository;
        private readonly ITokenService _tokenService;

        public ContactCRUD(IPasswordHasher passwordHasher, IUserRepository userRepository, IContactRepository contactRepository, ITokenService tokenService)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _contactRepository = contactRepository;
            _tokenService = tokenService;
        }

        public async Task<Result<Contact>> CreateContactAsync(Token decodeToken, string name, string? email, string? telegramUsername)
        {
            var contact = new Contact
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                TelegramUsername = telegramUsername,

                UserId = decodeToken.Id,
            };

            await _contactRepository.AddAsync(contact);

            return Result<Contact>.Success(contact, 201);
        }

        public async Task<Result<Contact>> GetOneContactAsync(Token decodeToken, Guid contactId)
        {
            Contact contact = await _contactRepository.GetByIdAsync(contactId);


            if (contact == null)
            {
                return Result<Contact>.Failure("Не вдалося знайти контакт", 404);
            }

            if (contact.UserId != decodeToken.Id)
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


        public async Task<Result<Contact>> UpdateContactAsync(Guid contactId, Token decodeToken, string? name, string? email, string? telegramUsername)
        {
            var contact = await _contactRepository.GetByIdAsync(contactId);

            if (contact == null)
            {
                return Result<Contact>.Failure("Не вдалося знайти контакт", 404);
            }

            if (contact.UserId != decodeToken.Id)
            {
                return Result<Contact>.Failure("Цей контакт не ваш", 403);
            }

            contact.Name = name ?? contact.Name;
            contact.Email = email ?? contact.Email;
            contact.TelegramUsername = telegramUsername ?? contact.TelegramUsername;

            await _contactRepository.UpdateAsync(contact);

            return Result<Contact>.Success(contact);
        }

        public async Task<Result<Unit>> DeleteContactAsync(Token decodeToken ,Guid contactId)
        {
            Contact contact = await _contactRepository.GetByIdAsync(contactId);


            if (contact == null)
                return Result<Unit>.Failure("Не вдалося знайти контакт", 404);

            if (contact.UserId != decodeToken.Id)
                return Result<Unit>.Failure("Цей контакт не ваш", 403);

            await _contactRepository.RemoveAsync(contact);

            return Result<Unit>.Success();
        }

    }
}
