using System;
using System.Collections.Generic;
using System.Text;

namespace Sender.Domain
{
    public class Token
    {
        public Guid Id {  get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
