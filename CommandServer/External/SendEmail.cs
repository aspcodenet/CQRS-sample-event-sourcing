using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;
using System.IO;

namespace CommandServer.External
{
    public class SendEmail : IHandleMessages<NerdCommandMessages.External.SendMail>
    {
        public void  Handle(NerdCommandMessages.External.SendMail message)
        {
 	        //Send email but here we just write lo a file
            using(TextWriter w =  System.IO.File.AppendText("sendemails.txt" ))
            {
                w.WriteLine(DateTime.Now.ToString() + " SENDING EMAIL ");
                w.WriteLine("\tTo:" + message.ToName + " " + message.ToEmail );
                w.WriteLine("\tFrom:" + message.FromName + " " + message.FromEmail);
                w.WriteLine("\tHeader:" + message.Header);
                w.WriteLine("\tMessage:" + message.Message);
            }
        }
    }
}
