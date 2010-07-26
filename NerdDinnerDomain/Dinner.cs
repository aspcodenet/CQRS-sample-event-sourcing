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
            Participants = new HashSet<User>();
        }

        private void RegisterStorageHandler()
        {
            RegisterStorageEventHandler<NerdDinnerDomainEvents.DinnerCreatedEvent>(onDinnerCreatedEvent);
        }
        private void onDinnerCreatedEvent(NerdDinnerDomainEvents.DinnerCreatedEvent oUserCreatedEvent)
        {
            Id = oUserCreatedEvent.EntityId;
            Date = oUserCreatedEvent.Date;
            Location = oUserCreatedEvent.Location;
            Description = oUserCreatedEvent.Description;
            Organizer_User_id = oUserCreatedEvent.Organizer_User_id;
        }



        [Serializable]
        public class Snapshot
        {
            public Guid Id { get; set; }
            public string Location { get; set; }
            public string Description { get; set; }
            public DateTime Date { get; set; }
            public Guid Organizer_User_Id { get; set; }
        }
        protected override void Snapshot_LoadFrom(object o)
        {
            Snapshot ser = o as Snapshot;
            Id = ser.Id;
            Location = ser.Location;
            Description = ser.Description;
            Date = ser.Date;
            Organizer_User_id = ser.Organizer_User_Id;
        }
        protected override object Snapshot_Create()
        {
            Snapshot ser = new Snapshot();
            ser.Id = Id;
            ser.Location = Location;
            ser.Description = Description;
            ser.Date = Date;
            ser.Organizer_User_Id = Organizer_User_id;
            return ser;
        }




        public virtual void ChangeWhereabouts(DateTime dtDate, string Location)
        {
            
        }
        public virtual void AddParticipant(User oUser)
        {
/*            if (!Participants.Contains(oUser))
            {
                Participants.Add(oUser);
                Events.DomainEvents.UserOptedInForDinnerEvent.Raise(new NerdDinnerDomain.Events.DinnerUserEventArgs { DinnerId=Id, Owner_User_id=Organizer_User_id, User_Id_Opted_In=oUser.Id });
            }
            //Raise event
  */          
        }
        public virtual void RemoveParticipant(User oUser)
        {
            if (Participants.Contains(oUser))
                Participants.Remove(oUser);
        }
        public virtual ICollection<User> Participants { get; set; }

    }
}
