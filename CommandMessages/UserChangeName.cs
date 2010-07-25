using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace NerdCommandMessages
{
    [Serializable]
    public class UserChangeName : IMessage
    {
        public UserChangeName(Guid gUserId,string sForname, string sSurname, CommandInfrastructure.MessageLogInfo oorginator)
        {
            Forname = sForname;
            Surname = sSurname;
            UserId = gUserId;
            CommandId = Guid.NewGuid();
            orginator = oorginator;
        }

        public Guid UserId { get; set; }
        public string Forname { get; set; }
        public string Surname { get; set; }
    
        public Guid CommandId { get; private set; }

        public CommandInfrastructure.MessageLogInfo orginator;


    }
}
