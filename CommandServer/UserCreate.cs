using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace CommandServer
{
    public class UserCreate : IHandleMessages<NerdCommandMessages.UserCreate>
    {
        public static IBus Bus { get; set; }

        public void Handle(NerdCommandMessages.UserCreate message)
        {
            log4net.LogManager.GetLogger("CommandHandler").Info("NerdCommandMessages.UserCreate " );

            //Update with our data = we have received it
            //message.SetServerReceivedInfo( CommandInfrastructure.MessageLogInfo.CreateNew() );
            using (EventStoreNhib.UnitOfWork uow = new EventStoreNhib.UnitOfWork(DB.GetFactory().OpenSession(), true))
            {
                NerdDinnerDomain.User oUser = NerdDinnerDomain.User.CreateNew(message.UserId, message.Forname, message.Surname);
                uow.CreateRepository<NerdDinnerDomain.User>().Append(oUser, false);
                Bus.Send(new ReportServerMessages.CommandProcessed());
            }


            //message.SetServerDoneInfo(CommandInfrastructure.MessageLogInfo.CreateNew());

        }
    }
}
