using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using EventStoreInfrastructure;

namespace EventStoreNhib
{
    public class UnitOfWork : EventStoreInfrastructure.IUnitOfWork
    {
        ISession m_sessspec;
        bool m_fSaveChangesAtEndOfScope = false;
        bool m_fIsDisposed = false;

        public UnitOfWork(ISession cnt, bool fSaveChangesAtEndOfScope = false)
        {
            m_sessspec = cnt;
            m_fSaveChangesAtEndOfScope = fSaveChangesAtEndOfScope;
        }


        public EventStoreInfrastructure.IRepository<T> CreateRepository<T>() where T : class,IEventStoreBase, new()
        {
            return new Repository<T>(m_sessspec);
        }

        public void SaveChanges()
        {
            m_sessspec.Flush(); 
        }

        public void Dispose()
        {
            if (!m_fIsDisposed)
            {
                if (m_fSaveChangesAtEndOfScope)
                    SaveChanges();
                m_fIsDisposed = true;
                m_sessspec.Dispose();
                m_sessspec = null;
                GC.SuppressFinalize(this);
            }

            
        }
    }
}
