using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace CommandServer
{
    public class MessageEndpoint : IConfigureThisEndpoint, AsA_Server, IWantToRunAtStartup
    {
        public static IBus Bus { get; set; }
        public void Run()
        {
            CommandServer.DomainEvents.GenericHandler.Register();
            log4net.Config.XmlConfigurator.Configure();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
