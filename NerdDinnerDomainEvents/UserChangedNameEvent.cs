using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NerdDinnerDomainEvents
{
    [Serializable]
    public class UserChangedNameEvent : DomainEventsInfrastructure.DomainEventBase
    {
        //public virtual Guid UserId { get; private set; }
        public virtual string Forname { get; private set; }
        public virtual string Surname { get; private set; }

        public UserChangedNameEvent(Guid id, string forname, string surname, Guid parentEventId = new Guid(), Guid parentCommandId = new Guid())
            : base(id, parentEventId, parentCommandId)
        {
            //UserId = id; UserId is entityid in base
            Forname = forname;
            Surname = surname;
        }

    }
}
