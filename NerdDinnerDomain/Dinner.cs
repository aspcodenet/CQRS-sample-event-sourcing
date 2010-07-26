using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NerdDinnerDomain
{
    [Serializable]
    public class Dinner : EventStoreInfrastructure.EventStoreBase
    {
        public virtual DateTime Date { get; set; }
        public virtual string Location { get; set; }
        public virtual string Description { get; set; }
        public virtual Guid Organizer_User_id { get; set; }


        public static Dinner CreateNew(Guid id, DateTime dtDate, string sLocation,string sDescription, Guid OrganizerUserId)
        {
            return new Dinner(id, dtDate, sLocation,sDescription, OrganizerUserId);
        }

        private Dinner(Guid id, DateTime dtDate, string sLocation, string sDescription, Guid OrganizerUserId)
        {
            Id = id;

            //Results in a Event
            NerdDinnerDomainEvents.DinnerCreatedEvent oEvent = new NerdDinnerDomainEvents.DinnerCreatedEvent(id, dtDate, sLocation, sDescription, OrganizerUserId);

            //Apply
            ApplyStorageEvent<NerdDinnerDomainEvents.DinnerCreatedEvent>(oEvent);

            //Publish event
            Events.DomainEvents.Instance.Dispatcher.Publish<NerdDinnerDomainEvents.DinnerCreatedEvent>(oEvent);
        }

        public Dinner()
        {
            RegisterStorageHandler();
            Participants = new List<Guid>();
        }

        private void RegisterStorageHandler()
        {
            RegisterStorageEventHandler<NerdDinnerDomainEvents.DinnerCreatedEvent>(onDinnerCreatedEvent);
            RegisterStorageEventHandler<NerdDinnerDomainEvents.DinnerModifiedTimeEvent>(onDinnerModifiedTimeEvent);
            RegisterStorageEventHandler<NerdDinnerDomainEvents.DinnerModifiedLocationEvent>(onDinnerModifiedLocationEvent);
            RegisterStorageEventHandler<NerdDinnerDomainEvents.DinnerModifiedDescriptionEvent>(onDinnerModifiedDescriptionEvent);
            RegisterStorageEventHandler<NerdDinnerDomainEvents.DinnerAddedParticipantEvent>(onDinnerAddedParticipantEvent);
            RegisterStorageEventHandler<NerdDinnerDomainEvents.DinnerRemovedParticipantEvent>(onDinnerRemovedParticipantEvent);
            
            
            
        }
        private void onDinnerAddedParticipantEvent(NerdDinnerDomainEvents.DinnerAddedParticipantEvent ev)
        {
            if (!Participants.Contains(ev.User_id))
                Participants.Add(ev.User_id);
        }
        private void onDinnerRemovedParticipantEvent(NerdDinnerDomainEvents.DinnerRemovedParticipantEvent ev)
        {
            if (Participants.Contains(ev.User_id))
                Participants.Remove(ev.User_id);
        }

        private void onDinnerModifiedDescriptionEvent(NerdDinnerDomainEvents.DinnerModifiedDescriptionEvent ev)
        {
            Description = ev.Description;
        }

        private void onDinnerModifiedLocationEvent(NerdDinnerDomainEvents.DinnerModifiedLocationEvent ev)
        {
            Location = ev.Location;
        }
        private void onDinnerCreatedEvent(NerdDinnerDomainEvents.DinnerCreatedEvent oUserCreatedEvent)
        {
            Id = oUserCreatedEvent.EntityId;
            Date = oUserCreatedEvent.Date;
            Location = oUserCreatedEvent.Location;
            Description = oUserCreatedEvent.Description;
            Organizer_User_id = oUserCreatedEvent.Organizer_User_id;
        }

        private void onDinnerModifiedTimeEvent(NerdDinnerDomainEvents.DinnerModifiedTimeEvent oDinnerModifiedTimeEvent)
        {
            Date = oDinnerModifiedTimeEvent.Date;
        }



        [Serializable]
        public class Snapshot
        {
            public Guid Id { get; set; }
            public string Location { get; set; }
            public string Description { get; set; }
            public DateTime Date { get; set; }
            public Guid Organizer_User_Id { get; set; }
            public List<Guid> Participants { get; set; }
        }
        protected override void Snapshot_LoadFrom(object o)
        {
            Snapshot ser = o as Snapshot;
            Id = ser.Id;
            Location = ser.Location;
            Description = ser.Description;
            Date = ser.Date;
            Organizer_User_id = ser.Organizer_User_Id;
            Participants = ser.Participants;
        }
        protected override object Snapshot_Create()
        {
            Snapshot ser = new Snapshot();
            ser.Id = Id;
            ser.Location = Location;
            ser.Description = Description;
            ser.Date = Date;
            ser.Organizer_User_Id = Organizer_User_id;
            ser.Participants = Participants;
            return ser;
        }




        public virtual void AddParticipant(Guid userid)
        {
            //Behaviour ???
            //TODO
            //Already exists = error!!!
            //Is the user also organizer = error

            //Returns in an event
            NerdDinnerDomainEvents.DinnerAddedParticipantEvent oEvent = new NerdDinnerDomainEvents.DinnerAddedParticipantEvent(Id, userid);

            //Apply
            ApplyStorageEvent<NerdDinnerDomainEvents.DinnerAddedParticipantEvent>(oEvent);

            //Publish event
            Events.DomainEvents.Instance.Dispatcher.Publish<NerdDinnerDomainEvents.DinnerAddedParticipantEvent>(oEvent);


/*            if (!Participants.Contains(oUser))
            {
                Participants.Add(oUser);
                Events.DomainEvents.UserOptedInForDinnerEvent.Raise(new NerdDinnerDomain.Events.DinnerUserEventArgs { DinnerId=Id, Owner_User_id=Organizer_User_id, User_Id_Opted_In=oUser.Id });
            }
            //Raise event
  */          
        }
        public virtual void RemoveParticipant(Guid userid)
        {
            //Behaviour ???
            //TODO
            //Already exists = error!!!
            //Is the user also organizer = error

            //Returns in an event
            NerdDinnerDomainEvents.DinnerRemovedParticipantEvent oEvent = new NerdDinnerDomainEvents.DinnerRemovedParticipantEvent(Id, userid);

            //Apply
            ApplyStorageEvent<NerdDinnerDomainEvents.DinnerRemovedParticipantEvent>(oEvent);

            //Publish event
            Events.DomainEvents.Instance.Dispatcher.Publish<NerdDinnerDomainEvents.DinnerRemovedParticipantEvent>(oEvent);
        }
        public virtual List<Guid> Participants { get; set; }


        public void SetTime(DateTime dateTime)
        {

            //Behaviour ???

            //Returns in an event
            NerdDinnerDomainEvents.DinnerModifiedTimeEvent oEvent = new NerdDinnerDomainEvents.DinnerModifiedTimeEvent(Id, dateTime);

            //Apply
            ApplyStorageEvent<NerdDinnerDomainEvents.DinnerModifiedTimeEvent>(oEvent);

            //Publish event
            Events.DomainEvents.Instance.Dispatcher.Publish<NerdDinnerDomainEvents.DinnerModifiedTimeEvent>(oEvent);
            
        }

        public void SetLocation(string sLocation)
        {
            //Behaviour ???

            //Returns in an event
            NerdDinnerDomainEvents.DinnerModifiedLocationEvent oEvent = new NerdDinnerDomainEvents.DinnerModifiedLocationEvent(Id, sLocation);

            //Apply
            ApplyStorageEvent<NerdDinnerDomainEvents.DinnerModifiedLocationEvent>(oEvent);

            //Publish event
            Events.DomainEvents.Instance.Dispatcher.Publish<NerdDinnerDomainEvents.DinnerModifiedLocationEvent>(oEvent);
        }

        public void SetDescription(string sDescription)
        {
            //Behaviour ???

            //Returns in an event
            NerdDinnerDomainEvents.DinnerModifiedDescriptionEvent oEvent = new NerdDinnerDomainEvents.DinnerModifiedDescriptionEvent(Id, sDescription);

            //Apply
            ApplyStorageEvent<NerdDinnerDomainEvents.DinnerModifiedDescriptionEvent>(oEvent);

            //Publish event
            Events.DomainEvents.Instance.Dispatcher.Publish<NerdDinnerDomainEvents.DinnerModifiedDescriptionEvent>(oEvent);
        }
    }
}
