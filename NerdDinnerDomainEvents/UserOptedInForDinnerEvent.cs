using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NerdDinnerDomainEvents
{

    [Serializable]
    public class UserOptedInForDinnerEvent : DomainEventsInfrastructure.DomainEventBase
    {
        public virtual Guid iddinner { get; private set; }

        public UserOptedInForDinnerEvent(Guid iduser, Guid giddinner, Guid parentEventId = new Guid(), Guid parentCommandId = new Guid())
            : base(iduser, parentEventId, parentCommandId)
        {
            iddinner = giddinner;
        }
    }


}
