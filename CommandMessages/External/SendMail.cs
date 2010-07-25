using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;


namespace NerdCommandMessages.External
{
    [Serializable]
    public class SendMail : IMessage
    {
        public System.Guid MessageGuid { get; set; }
        public string FromName;
        public string FromEmail;
        public string ToName;
        public string ToEmail;
        public string Header;
        public string Message;
    }
}
