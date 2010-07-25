using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIClasses
{
    public class DinnersNotGoingTo
    {
        public virtual Guid Dinner_Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Location{ get; set; }
        public virtual string Organizer_Fullname { get; set; }
    }
}
