using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;

namespace CommandServer
{
    public class DB
    {

        public static ISessionFactory GetFactory()
        {
            Configuration configuration = new Configuration();
            configuration.Configure();
            return configuration.BuildSessionFactory();
        }


        static Systementor.Database.NHibernate.NhibContext cont2 = null;

        static public Systementor.Database.Repositories.IDataContext DataContext
        {
            get
            {

                if (cont2 == null)
                    cont2 = new Systementor.Database.NHibernate.NhibContext(Systementor.Database.NHibernate.NhibContext.CreateFactory());
                return cont2;
            }
        }
    }
}
