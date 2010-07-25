using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportUpdaterServer.Classes
{
    public class ReportUser
    {
        public virtual Guid User_Id { get; set; }
        public virtual string Forname { get; set; }
        public virtual string Surname { get; set; }
        public static Dictionary<string, object> Criteria_ById(Guid id)
        {
            Dictionary<string, object> params1 = new Dictionary<string, object>();
            params1.Add("id", id);
            return params1;
        }

    }
}
