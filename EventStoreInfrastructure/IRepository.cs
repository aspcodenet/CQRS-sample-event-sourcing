using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventStoreInfrastructure
{
    public interface IRepository<T> where T : class, IEventStoreBase, new() 
    {
        T GetById(Guid id);
        void Append(IEventStoreBase b, bool fSnapshotAsWell);
    }
}
