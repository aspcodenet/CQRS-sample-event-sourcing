using System;
using DomainEventsInfrastructure;
using System.Collections.Generic;


namespace NerdDinnerDomain.Events
{
    public class DomainEvents
    {
        [ThreadStatic()]
        private static DomainEvents m_Events = null;

        [ThreadStatic()]
        private static NerdDinnerDomain.DomainEventHandlers.GenericHandler m_InDomainHandler = null;

        public static DomainEvents Instance
        {
            get
            {
                if (m_Events == null)
                {
                    m_Events = new DomainEvents();
                    m_Events.Setup();
                }
                return m_Events;
            }
        }

        protected DomainEventsInfrastructure.DomainEventDispatcher dispatcher = null;
        public DomainEventsInfrastructure.DomainEventDispatcher Dispatcher
        {
            get
            {
                return dispatcher;
            }
        }
        private void Setup()
        {
            dispatcher = new DomainEventDispatcher();
            m_InDomainHandler = new DomainEventHandlers.GenericHandler();
            m_InDomainHandler.Setup();

        }


    }
}
