using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace NerdCommandMessages
{
    [Serializable]
    public class UserCreate : IMessage 
    {
        public UserCreate(Guid userid,string sForname, string sSurname, CommandInfrastructure.MessageLogInfo oorginator)
        {
            Forname = sForname;
            Surname = sSurname;
            UserId = userid;
            CommandId = Guid.NewGuid();
            orginator = oorginator;
        }
        public string Forname { get; private set; }
        public string Surname { get; private set; }
        public Guid UserId { get; private set; }
        public Guid CommandId { get; private set; }

        public CommandInfrastructure.MessageLogInfo orginator;

    }
}
