using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIClasses
{
    public class Report_User
    {
        public Report_User()
        {
        }
        public virtual Guid User_Id { get; set; }
        public virtual string Forname { get;set;}
        public virtual string Surname { get;set;}
//        public virtual ICollection<Report_Dinner> Dinners { get; set; }

        public override string ToString()
        {
            return Forname + " " + Surname;
        }

    }
}
