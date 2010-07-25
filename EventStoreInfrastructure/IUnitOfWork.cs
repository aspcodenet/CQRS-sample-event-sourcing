using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventStoreInfrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> CreateRepository<T>() where T : class,IEventStoreBase, new();
        void SaveChanges();
    }
}
