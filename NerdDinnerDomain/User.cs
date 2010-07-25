using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NerdDinnerDomain
{
    [Serializable]
    public class User : EventStoreInfrastructure.EventStoreBase
    {
        //public virtual Guid Id { get; private set; }
        public virtual string Forname { get; private set; }
        public virtual string Surname { get; private set; }
        public virtual DateTime Joined { get; private set; }
        public User()
        {
            RegisterStorageHandler();
        }
        private User(Guid id, string forname, string surname, DateTime joined)
            : this()
        {
            //Behaviour -
            Id = id;


            //Results in a Event
            NerdDinnerDomainEvents.UserCreatedEvent oEvent = new NerdDinnerDomainEvents.UserCreatedEvent(id, forname, surname, DateTime.Now);

            //Apply
            ApplyStorageEvent<NerdDinnerDomainEvents.UserCreatedEvent>(oEvent);

            //Publish event
            Events.DomainEvents.Instance.Dispatcher.Publish<NerdDinnerDomainEvents.UserCreatedEvent>(oEvent);

        }
        //Adding a new user
        public static User CreateNew(Guid id, string forname, string surname)
        {
            return new User(id,forname,surname,DateTime.Now);
        }

        public void ChangeName(string forname, string surname)
        {
            //Behaviour ???

            //Returns in an event
            NerdDinnerDomainEvents.UserChangedNameEvent oEvent = new NerdDinnerDomainEvents.UserChangedNameEvent(Id, forname, surname);

            //Apply
            ApplyStorageEvent<NerdDinnerDomainEvents.UserChangedNameEvent>(oEvent);

            //Publish event
            Events.DomainEvents.Instance.Dispatcher.Publish<NerdDinnerDomainEvents.UserChangedNameEvent>(oEvent);

        }


        [Serializable]
        public class Snapshot
        {
            public Guid Id { get; set; }
            public string Forname { get;set;}
            public string Surname { get;set;}
            public DateTime Joined { get;set;}
        }
        protected override void Snapshot_LoadFrom(object o)
        {
            Snapshot ser = o as Snapshot;
            Id = ser.Id;
            Forname = ser.Forname;
            Surname = ser.Surname;
            Joined = ser.Joined;
        }
        protected override  object Snapshot_Create()
        {
            Snapshot ser = new Snapshot();
            ser.Id = Id;
            ser.Forname = Forname;
            ser.Surname = Surname;
            ser.Joined = Joined;
            return ser;
        }


        private void RegisterStorageHandler()
        {
            RegisterStorageEventHandler<NerdDinnerDomainEvents.UserCreatedEvent>(onUserCreatedEvent);
            RegisterStorageEventHandler<NerdDinnerDomainEvents.UserChangedNameEvent>(onUserChangedNameEvent);
        }


        private void onUserCreatedEvent(NerdDinnerDomainEvents.UserCreatedEvent oUserCreatedEvent)
        {
            Id = oUserCreatedEvent.EntityId;
            Forname = oUserCreatedEvent.Forname;
            Surname = oUserCreatedEvent.Surname;
            Joined = oUserCreatedEvent.Joined;
        }

        private void onUserChangedNameEvent(NerdDinnerDomainEvents.UserChangedNameEvent oUserChangedNameEvent)
        {
            Forname = oUserChangedNameEvent.Forname;
            Surname = oUserChangedNameEvent.Surname;
        }


    }
}
