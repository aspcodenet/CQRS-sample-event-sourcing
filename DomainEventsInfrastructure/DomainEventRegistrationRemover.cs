using System;

//Big thanks to Udi Dahan for providing code for this 

namespace DomainEventsInfrastructure
{
    public class DomainEventRegistrationRemover : IDisposable
    {
        private readonly Action CallOnDispose;

        public DomainEventRegistrationRemover(Action ToCall)
        {
            this.CallOnDispose = ToCall;
        }

        public void Dispose()
        {
            this.CallOnDispose.DynamicInvoke();
        }
    }
}
