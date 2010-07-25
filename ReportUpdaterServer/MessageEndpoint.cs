using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace ReportUpdaterServer
{
    public class MessageEndpoint : IConfigureThisEndpoint, AsA_Server, IWantToRunAtStartup
    {
        public static IBus Bus { get; set; }
        public void Run()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }


    }
}
