using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using NServiceBus;

namespace CommandInfrastructure
{
    [Serializable]
    public class MessageLogInfo : IMessage
    {
        /// <summary>
        /// Time of creation. Will be client time
        /// </summary>
        public DateTime Created { get; private set; }
        
        /// <summary>
        /// some sort of user identification
        /// </summary>
        public string UserId { get; private set; }

        /// <summary>
        /// ipaddress of creator
        /// </summary>
        public string IP { get; private set; }


        /// <summary>
        /// shortcut for creating orginator windows apps = first ip of current machine 
        /// </summary>
        public static MessageLogInfo CreateNew(string sUserId)
        {
            string sIP = "";
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            if ( ipEntry.AddressList.Length > 0 )
                sIP = ipEntry.AddressList[0].ToString();
            return new MessageLogInfo(sUserId,sIP);
        }

        /// <summary>
        /// shortcut for creating orginator windows apps = first ip of current machine USERID = windows user
        /// </summary>
        public static MessageLogInfo CreateNew()
        {
            string sIP = "";
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            if (ipEntry.AddressList.Length > 0)
                sIP = ipEntry.AddressList[0].ToString();
            return new MessageLogInfo(System.Security.Principal.WindowsIdentity.GetCurrent().Name, sIP);
        }


        /// <summary>
        /// alternative - can be used in webscenarios
        /// </summary>
        public static MessageLogInfo CreateNew(string sUserId, string sIP)
        {
            return new MessageLogInfo(sUserId, sIP);
        }




        private MessageLogInfo()
        {
            Created = new DateTime(1900, 1, 1);
            UserId = "";
            IP = "";
        }

        private MessageLogInfo(string sUserId,string sIP)
        {
            Created = DateTime.Now;
            UserId = sUserId;
            IP = sIP;
        }


    }
}
