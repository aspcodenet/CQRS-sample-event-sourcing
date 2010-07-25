using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace NerdCommandMessages
{
    [Serializable]
    public class UserOptInForDinner : IMessage
    {
        public System.Guid MessageGuid { get; set; }
        public int UserId { get; set; }
        public int DinnerId { get; set; }
    
    }
}
