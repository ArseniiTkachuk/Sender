namespace Sender.API.DTO
{
    public class UpdateMessageRequest
    {
        public required Guid Id { get; init; }
        public required string Subject { get; init; }
        public required string Body { get; init; }
    }
}
