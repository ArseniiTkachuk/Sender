namespace Sender.API.DTO
{
    public class CreateMessageRequest
    {
        public required string Subject { get; init; }
        public required string Body { get; init; }
    }
}
