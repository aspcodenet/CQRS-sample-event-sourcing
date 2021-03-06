﻿using System;
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
            NerdCommandMessages.UserCreate message = new NerdCommandMessages.UserCreate(Guid.NewGuid(),sForname, sLastName, CommandInfrastructure.MessageLogInfo.CreateNew(CurrentUser));
            Bus.Send(message);
            return new ReturnValue(message.CommandId, message.UserId);
        }

        public static ReturnValue DinnerCreate(DateTime dtWhen, string location, string description, Guid organizerUserId)
        {
            NerdCommandMessages.DinnerCreate message = new NerdCommandMessages.DinnerCreate(Guid.NewGuid(), dtWhen, location, description, organizerUserId, CommandInfrastructure.MessageLogInfo.CreateNew(CurrentUser));
            Bus.Send(message);
            return new ReturnValue(message.CommandId, message.DinnerId);
        }




        public static ReturnValue DinnerModifyTime(Guid dinnerid,DateTime dtWhen)
        {
            NerdCommandMessages.DinnerModifyTime message = new NerdCommandMessages.DinnerModifyTime(dinnerid, dtWhen,  CommandInfrastructure.MessageLogInfo.CreateNew(CurrentUser));
            Bus.Send(message);
            return new ReturnValue(message.CommandId, message.DinnerId);
        }
        public static ReturnValue DinnerModifyLocation(Guid dinnerid, string sLocation)
        {
            NerdCommandMessages.DinnerModifyLocation message = new NerdCommandMessages.DinnerModifyLocation(dinnerid, sLocation, CommandInfrastructure.MessageLogInfo.CreateNew(CurrentUser));
            Bus.Send(message);
            return new ReturnValue(message.CommandId, message.DinnerId);
        }
        public static ReturnValue DinnerModifyDescription(Guid dinnerid, string sDescription)
        {
            NerdCommandMessages.DinnerModifyDescription message = new NerdCommandMessages.DinnerModifyDescription(dinnerid, sDescription, CommandInfrastructure.MessageLogInfo.CreateNew(CurrentUser));
            Bus.Send(message);
            return new ReturnValue(message.CommandId, message.DinnerId);
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

        internal static ReturnValue UserOptOutForDinner(Guid userid, Guid dinnerid)
        {
            NerdCommandMessages.DinnerRemoveUser message = new NerdCommandMessages.DinnerRemoveUser(userid, dinnerid, CommandInfrastructure.MessageLogInfo.CreateNew(CurrentUser));
            Bus.Send(message);
            return new ReturnValue(message.CommandId, message.UserId);
        }


        internal static ReturnValue UserOptInForDinner(Guid userid, Guid dinnerid)
        {
            NerdCommandMessages.DinnerAddUser message = new NerdCommandMessages.DinnerAddUser(userid,dinnerid, CommandInfrastructure.MessageLogInfo.CreateNew(CurrentUser));
            Bus.Send(message);
            return new ReturnValue(message.CommandId, message.UserId);
        }
    }
}
