using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainEventsInfrastructure
{
    /// <summary>
    /// All event handlers creates this - which is stored as a log
    /// </summary>
    [Serializable]
    public class DomainHandlerInfo
    {
        /// <summary>
        /// Read only Id of this event. Gets set in constructor
        /// </summary>
        public Guid Id { get; private set; }
        public Guid EventId { get; private set; }

        public DateTime Received { get; private set; }
        public DateTime Done { get; private set; }
        public int Status { get; private set; }

        public string HandlerId { get; private set; }

        public void RegisterReceived()
        {
            Received = DateTime.Now;
        }
        public void RegisterDoneOK()
        {
            Status = 0;
            Done = DateTime.Now;
        }
        public void RegisterDoneWithError(int errcode)
        {
            Status = errcode;
            Done = DateTime.Now;
        }

        public DomainHandlerInfo(string handlerid, Guid eventId)
        {
            Id = Guid.NewGuid();
            EventId = eventId;
            HandlerId = handlerid;
            Status = -1;
        }
    }
}
