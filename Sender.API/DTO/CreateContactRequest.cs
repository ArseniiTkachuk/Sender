namespace Sender.API.DTO
{
    public class CreateContactRequest
    {
        public required string Name { get; init; }
        public required string? Email { get; init; }
        public required string? TelegramUsername { get; init; }
    }
}
