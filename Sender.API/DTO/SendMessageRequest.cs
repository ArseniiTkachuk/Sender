using Sender.Domain;

namespace Sender.API.DTO
{
    public record SendMessageRequest(
        List<ContactTarget> Contacts,
        MessageContent? Message,
        Guid? messageId
    );
}
