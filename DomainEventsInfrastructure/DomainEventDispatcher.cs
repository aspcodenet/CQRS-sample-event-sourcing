using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainEventsInfrastructure
{
    public class DomainEventDispatcher
    {
        private readonly Dictionary<Type, List<Action<DomainEventsInfrastructure.DomainEventBase>>>  _registeredEventsAndTypes = new Dictionary<Type,List<Action<DomainEventBase>>>();

        private readonly List<Action<DomainEventsInfrastructure.DomainEventBase>> _registeredEvents = new List<Action<DomainEventBase>>();


        public void RegisterEventHandler<T>(Action<T> callback) where T : DomainEventsInfrastructure.DomainEventBase
        {
            //_registeredEvents.Add(theEvent => callback(theEvent as T));

            List<Action<DomainEventsInfrastructure.DomainEventBase>> list;
            //Exists?
            if ( _registeredEventsAndTypes.TryGetValue(typeof(T), out list) == false )
            {
                list = new List<Action<DomainEventBase>>();
                _registeredEventsAndTypes.Add(typeof(T), list);
            }
            list.Add(theEvent => callback(theEvent as T));
            
        }



        public void Publish<TEvent>(TEvent domainEvent) where TEvent : DomainEventsInfrastructure.DomainEventBase
        {
            List<Action<DomainEventsInfrastructure.DomainEventBase>> list;

            if (_registeredEventsAndTypes.TryGetValue(typeof(TEvent), out list))
            {
                foreach (Action<TEvent> action in list)
                    action.Invoke(domainEvent);
            }

/*            foreach (Action<TEvent> action in _registeredEvents)
            {
                action.Invoke(domainEvent);
            }
 * */
        }

        public void PublishAsynch<TEvent>(TEvent domainEvent) where TEvent : DomainEventsInfrastructure.DomainEventBase
        {
            List<Action<DomainEventsInfrastructure.DomainEventBase>> list;
            if (_registeredEventsAndTypes.TryGetValue(typeof(TEvent), out list))
            {
                foreach (Action<TEvent> action in list)
                    System.Threading.Tasks.Parallel.Invoke(() => action(domainEvent));
            }
/*
            foreach (Action<TEvent> action in _registeredEvents)
            {
                System.Threading.Tasks.Parallel.Invoke(() => action(domainEvent));
            }
            */
        }


        //public static readonly DomainEvent<DinnerUserEventArgs> UserOptedInForDinnerEvent = 

        //                                     new DomainEvent<DinnerUserEventArgs>();

    }
}
