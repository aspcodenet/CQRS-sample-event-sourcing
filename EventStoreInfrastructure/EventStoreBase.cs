using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventStoreInfrastructure
{
    [Serializable]
    public abstract class EventStoreBase : IEventStoreBase
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public int EventVersion { get; set; }
        
        public int EventCountSinceLastSnapshot { get; set; }
        public DateTime LastSnapshotTime { get; set; }


        private readonly Dictionary<Type, Action<DomainEventsInfrastructure.DomainEventBase>> _registeredEvents;
        private readonly List<DomainEventsInfrastructure.DomainEventBase> _appliedEvents;




        public EventStoreBase()
        {
            _registeredEvents = new Dictionary<Type, Action<DomainEventsInfrastructure.DomainEventBase>>();
            _appliedEvents = new List<DomainEventsInfrastructure.DomainEventBase>();
            //_childEventProviders = new List<IEntityEventProvider<TDomainEvent>>();
            EventCountSinceLastSnapshot = 0;
            LastSnapshotTime = new DateTime(1900, 1, 1);
        }


        public void RegisterStorageEventHandler<T>(Action<T> callback) where T : DomainEventsInfrastructure.DomainEventBase
        {
            _registeredEvents.Add(typeof(T), theEvent => callback(theEvent as T));
        }

        protected void ApplyStorageEvent<TEvent>(TEvent domainEvent) where TEvent : DomainEventsInfrastructure.DomainEventBase
        {
            domainEvent.EntityId = Id;
            domainEvent.Version = GetNewEventVersion();
            applyStorageEvent(domainEvent.GetType(), domainEvent);
            _appliedEvents.Add(domainEvent);
        }


        public void applyStorageEvent(Type eventType, DomainEventsInfrastructure.DomainEventBase domainEvent)
        {
            Action<DomainEventsInfrastructure.DomainEventBase> handler;

            if (!_registeredEvents.TryGetValue(eventType, out handler))
                return;

            handler(domainEvent);
        }



        private int GetNewEventVersion()
        {
            return ++EventVersion;
        }








        IEnumerable<DomainEventsInfrastructure.DomainEventBase> IEventStoreBase.GetChanges()
        {
            return _appliedEvents;
        }


        public void FromSnapshot(object o)
        {
            Snapshot_LoadFrom(o);
        }

        public object CreateSnapshot()
        {
            return Snapshot_Create();
        }

        protected abstract void Snapshot_LoadFrom(object o);
        protected abstract object Snapshot_Create();


    }
}
