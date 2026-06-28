using Microsoft.EntityFrameworkCore;
using Sender.Application.Interfaces;
using Sender.Domain;
using Sender.Persistence;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(User user) { await _db.Users.AddAsync(user); await _db.SaveChangesAsync(); }

    public async Task<User?> GetByIdAsync(Guid id) => await _db.Users.FindAsync(id);
    public async Task<User?> GetByEmailAsync(string email) => await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
    public async Task<bool> ExistsAsync(string email) => await _db.Users.AnyAsync(u => u.Email == email);
}

public class ContactRepository : IContactRepository
{
    private readonly AppDbContext _db;
    public ContactRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(Contact contact) { await _db.Contacts.AddAsync(contact); await _db.SaveChangesAsync(); }

    public async Task<Contact?> GetByIdAsync(Guid id)
    {
        return await _db.Contacts.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Contact?> GetByIdWithUserAsync(Guid id)
    {
        return await _db.Contacts
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Contact>> GetAllByUserIdAsync(Guid userId)
        => await _db.Contacts.Where(c => c.UserId == userId).ToListAsync();

    public async Task RemoveAsync(Contact contact)
    {
        _db.Contacts.Remove(contact);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Contact contact)
    {
        _db.Contacts.Update(contact);
        await _db.SaveChangesAsync();
    }

    public async Task<string?> GetEmailAsync(Guid id)
    {
        return await _db.Contacts
            .Where(c => c.Id == id)
            .Select(c => c.Email)
            .FirstOrDefaultAsync();
    }

    public async Task<string?> GetTelegramUsernameAsync(Guid id)
    {
        return await _db.Contacts
            .Where(c => c.Id == id)
            .Select(c => c.TelegramUsername)
            .FirstOrDefaultAsync();
    }
}

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _db;
    public MessageRepository(AppDbContext db) => _db = db;


    public async Task AddAsync(Message message)
    {
        await _db.Messages.AddAsync(message); await _db.SaveChangesAsync();
    }

    public async Task<List<Message>> GetAllByUserIdAsync(Guid userId)
    {
        return await _db.Messages.Where(m => m.UserId == userId).ToListAsync();
    }

    public async Task<Message?> GetByIdAsync(Guid id)
    {
        return await _db.Messages.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task RemoveAsync(Message message)
    {
        _db.Messages.Remove(message);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Message message)
    {
        _db.Messages.Update(message);
        await _db.SaveChangesAsync();
    }
}