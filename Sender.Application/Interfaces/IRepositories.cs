using Sender.Domain;

namespace Sender.Application.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsAsync(string email);
}

public interface IContactRepository
{
    Task AddAsync(Contact contact);

    Task<Contact?> GetByIdAsync(Guid id);

    Task<Contact?> GetByIdWithUserAsync(Guid id);

    Task<List<Contact>> GetAllByUserIdAsync(Guid userId);

    Task RemoveAsync(Contact contact);

    Task UpdateAsync(Contact contact);
}

public interface IMessageRepository
{
    Task AddAsync(Message message);
    Task<Message?> GetByIdAsync(Guid id);
    Task<List<Message>> GetAllByUserIdAsync(Guid userId);

    Task RemoveAsync(Message contact);

    Task UpdateAsync(Message contact);



}