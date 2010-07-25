using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace ReportServerMessages
{
    [Serializable]
    public class CommandProcessed : IMessage
    {
        public System.Guid MessageGuid { get; set; }
    }
}
