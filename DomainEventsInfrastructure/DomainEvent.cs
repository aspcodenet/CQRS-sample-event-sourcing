using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainEventsInfrastructure
{
    [Serializable]
    public class DomainEventBase
    {
        /// <summary>
        /// Read only Id of this event. Gets set in constructor
        /// </summary>
        public Guid Id { get; private set; }
        public int Version { get; set; }
        /// <summary>
        /// The Aggregateroot it concerns
        /// </summary>
        public Guid EntityId { get; set; }

        public DateTime Created { get; private set; }
        public Guid ParentEventId { get; private set; }
        public Guid ParentCommandId { get; private set; }

        public DomainEventBase()
        {
        }
        public DomainEventBase(Guid entid, Guid parentEventId = new Guid(), Guid parentCommandId = new Guid())
        {
            Id = Guid.NewGuid();
            Created = DateTime.Now;
            EntityId = entid;
            ParentEventId = parentEventId;
            ParentCommandId = parentCommandId;
        }
    }
}
