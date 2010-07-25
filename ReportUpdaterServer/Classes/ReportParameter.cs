using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportUpdaterServer.Classes
{
    public class ReportParameter
    {
        public ReportParameter()
        {
            Paramname = "";
            intval = -1;
            stringval = "";
        }
        public virtual int Id { get; set; }
        public virtual string Paramname { get; set; }
        public virtual int intval { get; set; }
        public virtual string stringval { get; set; }
    }
}
