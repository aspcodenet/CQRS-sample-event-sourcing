using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace ReportServerMessages
{
    [Serializable]
    public class DinnerReservationUpdate    : IMessage
    {
        public Guid MessageId;
        public Guid CommandMessageId;
        public string OriginalSenderEndpoint;
        public int DinnerId;
        public int UserId;
    }
}
