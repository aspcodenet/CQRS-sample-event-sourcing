using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NerdDinnerDomain.DomainEventHandlers
{
    public class GenericHandler
    {
        public GenericHandler()
        {
        }
        public void Setup()
        {
            NerdDinnerDomain.Events.DomainEvents.Instance.Dispatcher.RegisterEventHandler<NerdDinnerDomainEvents.UserCreatedEvent>(OnUserCreatedEvent);
            NerdDinnerDomain.Events.DomainEvents.Instance.Dispatcher.RegisterEventHandler<NerdDinnerDomainEvents.UserChangedNameEvent>(OnUserChangedNameEvent);
        }
        public void OnUserCreatedEvent(NerdDinnerDomainEvents.UserCreatedEvent ev)
        {
            //When a new user has been created THIS should happen
            string s1 = "USER " + ev.EntityId.ToString() + " was created " + ev.Forname + " " + ev.Surname;

            using (TextWriter w = System.IO.File.AppendText("UserCreatedEvent-domainevents.txt"))
            {
                w.WriteLine(DateTime.Now.ToString() + " " + s1);
            }

        }
        public void OnUserChangedNameEvent(NerdDinnerDomainEvents.UserChangedNameEvent ev)
        {
            //When a new user has been created THIS should happen
            string s1 = "USER " + ev.EntityId.ToString() + " changed name to " + ev.Forname + " " + ev.Surname;

            using (TextWriter w = System.IO.File.AppendText("UserChangedNameEvent-domainevents.txt"))
            {
                w.WriteLine(DateTime.Now.ToString() + " " + s1);
            }

        }


    }
}
