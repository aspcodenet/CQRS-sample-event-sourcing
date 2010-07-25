using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NerdDinnerDomain
{
    [Serializable]
    public class Dinner
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Location { get; set; }
        public virtual string Description { get; set; }
        public virtual int Organizer_User_id { get; set; }

        public Dinner()
        {
            Participants = new HashSet<User>();
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
