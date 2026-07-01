using Sender.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sender.Persistence
{
    public class EmailProvider : IEmailProvider
    {
        public Task SendAsync(string subject, string body, string email)
        {
            throw new NotImplementedException();
        }
    }

    public class TelegramlProvider : ITelegramlProvider
    {
        public Task SendAsync(string subject, string body, string telegramUsername)
        {
            throw new NotImplementedException();
        }
    }
}
