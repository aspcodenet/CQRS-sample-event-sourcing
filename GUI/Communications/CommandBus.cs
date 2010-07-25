using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace GUI.Communications
{

    public class CommandBus 

    {

        //What is this??? I am not sure if I will need some way of tracjing what happens with the command 
        //or if I need the (maybe generated) aggregate root Id, so I return both back to client
        public class ReturnValue
        {
            public Guid CommandId { get;private set;}
            public Guid ARId { get;private set;}
            public ReturnValue(Guid messid, Guid id)
            {
                CommandId = messid;
                ARId = id;
            }
        }

        public static IBus Bus { get; set; }
        public static void Init()
        {
            Bus = NServiceBus.Configure.With()
                //.CastleWindsorBuilder()
                .DefaultBuilder()
                .XmlSerializer()
                .MsmqTransport()
                    .IsTransactional(true)
                .UnicastBus()
                    .ImpersonateSender(false)
                .CreateBus()
                .Start();

        }


        public static string CurrentUser
        {
            get
            {
                return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            }
        }

        public static ReturnValue UserCreate(string sForname, string sLastName)
        {
            NerdCommandMessages.UserCreate message = new NerdCommandMessages.UserCreate(sForname, sLastName, CommandInfrastructure.MessageLogInfo.CreateNew(CurrentUser));
            Bus.Send(message);
            return new ReturnValue(message.CommandId, message.UserId);
        }




        public static ReturnValue UserChangeName(Guid UserId, string Forname, string LastName)
        {
            NerdCommandMessages.UserChangeName message = new NerdCommandMessages.UserChangeName(UserId, Forname, LastName, CommandInfrastructure.MessageLogInfo.CreateNew(CurrentUser));
            Bus.Send(message);
            return new ReturnValue(message.CommandId, message.UserId);
        }



        public void Stop()
        {
        }

        internal static Guid UserOptInForDinner(int userid, int dinnerid)
        {
            Guid g = Guid.NewGuid(); ;
            NerdCommandMessages.UserOptInForDinner mess = new NerdCommandMessages.UserOptInForDinner { DinnerId = dinnerid, MessageGuid = g, UserId = userid };
            Bus.Send(mess);
            return g;
        }
    }
}
