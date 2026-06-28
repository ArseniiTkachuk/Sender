namespace Sender.Domain
{
    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Subject { get; set; }
        public string Body { get; set; }
        public Guid UserId { get; set; }
    }

}