using System;
using System.Collections.Generic;
//Big thanks to Udi Dahan for providing code for this 

namespace DomainEventsInfrastructure
{
    public class DomainEvent<E>
    {
        /*
         * If wanting to use it to feed back error codes or something
         * within a handler then use threadstatic - and the IDisposable
         * 
         * */
//        [ThreadStatic]
        private static List<Action<E>> _actions;

        protected List<Action<E>> actions
        {
            get
            {
                if (_actions == null)
                    _actions = new List<Action<E>>();

                return _actions;
            }
        }

        public void Register(Action<E> callback)
        {
            actions.Add(callback);
        }


/*        public IDisposable Register(Action<E> callback)
        {
            actions.Add(callback);
            return new DomainEventRegistrationRemover(delegate
            {
                actions.Remove(callback);
            }
            );
        }
        */
        public void Raise(E args)
        {
            //Must change it to be asynch - how...Maybe not use this but servicebus for this as well???
            //System.Threading.Tasks.Parallel.Invoke(

            foreach (Action<E> action in actions)
            {
                System.Threading.Tasks.Parallel.Invoke( () => action.Invoke(args)  );
                //action.Invoke(args);
            }
        }
    }
}

