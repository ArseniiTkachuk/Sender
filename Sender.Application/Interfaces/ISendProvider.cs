using System;
using System.Collections.Generic;
using System.Text;

namespace Sender.Application.Interfaces
{
    public interface IEmailProvider
    {
        Task SendAsync(string subject, string body, string email);
    }
    public interface ITelegramlProvider
    {
        Task SendAsync(string subject, string body, string telegramUsername);
    }
}
