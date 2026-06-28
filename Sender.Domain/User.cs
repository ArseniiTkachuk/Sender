using System;
using System.Collections.Generic;
using System.Text;

namespace Sender.Domain
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public List<Contact> Contacts { get; set; } = new();
    }
}
