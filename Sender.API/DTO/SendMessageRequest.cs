namespace Sender.API.DTO
{
    public record SendMessageRequest(
        List<ContactTarget> Contacts,
        MessageContent Message
    );

    public record ContactTarget(
        Guid ContactId,
        List<string> Channels
    );

    public record MessageContent(
        string Subject,
        string Body
    );
}
