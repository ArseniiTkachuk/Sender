namespace Sender.API.DTO
{
    public class UpdateContactRequest
    {
        public required Guid Id { get; init; }
        public required string? Name { get; init; }
        public required string? Email { get; init; }
        public required string? TelegramUsername { get; init; }
    }
}
