using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventStoreInfrastructure
{
    public interface IEventStoreBase
    {
        Guid Id { get; set; }
        int Version { get; set; }
        int EventVersion { get; set; }

        void applyStorageEvent(Type eventType, DomainEventsInfrastructure.DomainEventBase domainEvent);
        IEnumerable<DomainEventsInfrastructure.DomainEventBase> GetChanges();


        int EventCountSinceLastSnapshot { get; set; }
        DateTime LastSnapshotTime { get; set; }

        void FromSnapshot(object o);
        object CreateSnapshot();



    }
}
