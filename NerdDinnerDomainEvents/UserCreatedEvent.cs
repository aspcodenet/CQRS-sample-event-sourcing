using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NerdDinnerDomainEvents
{
    [Serializable]
    public class UserCreatedEvent : DomainEventsInfrastructure.DomainEventBase
    {
        //public virtual Guid UserId { get; private set; }
        public virtual string Forname { get; private set; }
        public virtual string Surname { get; private set; }
        public virtual DateTime Joined { get; private set; }

        public UserCreatedEvent(Guid id, string forname,string surname,DateTime joined, Guid parentEventId = new Guid(), Guid parentCommandId = new Guid() )
            :base(id,parentEventId,parentCommandId)
        {
            Forname = forname;
            Surname = surname;
            Joined = joined;
        }
    }
}
