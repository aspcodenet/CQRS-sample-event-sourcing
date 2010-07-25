using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportUpdaterServer.RepositoryHelper
{
    public class RepositoryList
    {
        Systementor.Database.Repositories.IUnitOfWork m_uow;
        public List<object> m_listofrepositories = new List<object>();
        public RepositoryList(Systementor.Database.Repositories.IUnitOfWork uow)
        {
            m_uow = uow;
        }
        public Systementor.Database.Repositories.IRepository<T> CreateOrGetRepository<T>() where T : class
        {
            foreach (object o in m_listofrepositories)
            {
                if (o as Systementor.Database.Repositories.IRepository<T> != null)
                    return o as Systementor.Database.Repositories.IRepository<T>;
            }
            Systementor.Database.Repositories.IRepository<T> ret = m_uow.CreateRepository<T>();
            m_listofrepositories.Add( ret );
            return ret;

        }
    }
}
