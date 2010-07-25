using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUI.DB
{
    class Context
    {
        static Systementor.Database.NHibernate.NhibContext cont2 = null;

        static public Systementor.Database.Repositories.IDataContext DataContext
        {
            get
            {
                if ( cont2 == null )
                    cont2 = new Systementor.Database.NHibernate.NhibContext(Systementor.Database.NHibernate.NhibContext.CreateFactory());
                return cont2;
            }
        }

    }

}
