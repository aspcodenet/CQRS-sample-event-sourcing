using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;

namespace ReportUpdaterServer
{
    public class DB
    {


        public static ISessionFactory GetFactory(string filename = "hibernate.cfg.xml")
        {
            Configuration configuration = new Configuration();
            configuration.Configure(filename);

            return configuration.BuildSessionFactory();
        }



        static Systementor.Database.NHibernate.NhibContext contReportServer = null;
        static ISessionFactory factReportServer = null;
        static public Systementor.Database.Repositories.IDataContext DataContextReportSever
        {
            get
            {

                if (contReportServer == null)
                {
                    if (factReportServer == null)
                        factReportServer = GetFactory("hibernate-reportserver.cfg.xml");
                    contReportServer = new Systementor.Database.NHibernate.NhibContext(factReportServer);
                }
                return contReportServer;
            }
        }

        static Systementor.Database.NHibernate.NhibContext contEventStore = null;
        static ISessionFactory factEventStore = null;
        static public Systementor.Database.Repositories.IDataContext DataContextEventStore
        {
            get
            {

                if (contEventStore == null)
                {
                    if (factEventStore == null)
                        factEventStore = GetFactory("hibernate-eventstore.cfg.xml");
                    contEventStore = new Systementor.Database.NHibernate.NhibContext(factEventStore);
                }
                return contEventStore;
            }
        }


    }
}
