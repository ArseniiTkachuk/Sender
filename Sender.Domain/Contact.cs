using System;
using System.Collections.Generic;
using System.Text;

namespace Sender.Domain
{
    public class Contact
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? TelegramUsername { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
