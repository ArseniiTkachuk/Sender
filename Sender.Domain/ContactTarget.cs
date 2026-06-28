using System;
using System.Collections.Generic;
using System.Text;

namespace Sender.Domain
{
    public class ContactTarget
    {
        public Guid ContactId { get; set; }
       public List<string> Channels { get; set; }
    }
}
