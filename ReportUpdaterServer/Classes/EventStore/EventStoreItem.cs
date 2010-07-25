using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportUpdaterServer.Classes.EventStore
{
    public class EventStoreItem
    {
        public EventStoreItem()
        {
        }
        public virtual int Id { get; set; }
        public virtual Guid objectid { get; set; }
        public virtual int typ { get; set; }
        public virtual int version { get; set; }
        public virtual DateTime created { get; set; }
        public virtual string serdata { get; set; }
        public virtual string classtype { get; set; }
        public virtual string artype { get; set; }


        //CriteriaHelper
        public static Dictionary<string, object> Criteria_LastProcessIdCriteria(int idlastprocessed)
        {
            Dictionary<string, object> params1 = new Dictionary<string, object>();
            params1.Add("lastprocessedid", idlastprocessed);
            return params1;
        }

    }
}
