using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;

namespace EventStoreNhib
{
    public class Repository<T> : EventStoreInfrastructure.IRepository<T> where T : class, EventStoreInfrastructure.IEventStoreBase, new()
    {
        protected ISession m_context;

        public Repository(ISession context)
        {
            if (context == null)
                ;
            m_context = context;
        }




        T EventStoreInfrastructure.IRepository<T>.GetById(Guid id)
        {


            //Create the real aggregate 
            T retObj = new T();

            //Get snapshot if exists
            IQuery query = m_context.GetNamedQuery("qLatestSnapShot") as IQuery;
            query.SetParameter("objectid", id);
            IList<EventStoreItem> ret = query.List<EventStoreItem>();
            int snapshotloaded_id = -1;
            if (ret.Count > 0)
            {
                EventStoreItem oItemSnapshot = ret[0];
                //We do that by deserializing oItem.serdata
                //retObj = DeserializeObject(oItem.serdata) as T2;   
                object o = Deserialize<object>(Convert.FromBase64String(oItemSnapshot.serdata));
                retObj.FromSnapshot(o);
                //retObj = Deserialize<T>(Convert.FromBase64String(oItemSnapshot.serdata));
                snapshotloaded_id = oItemSnapshot.Id;
                retObj.LastSnapshotTime = oItemSnapshot.created;
            }


            //Load history events after that
            query = m_context.GetNamedQuery("qHistoryEvents") as IQuery;
            query.SetParameter("objectid", id);
            query.SetParameter("latestsnapshotid", snapshotloaded_id);
            IList<EventStoreItem> historyevents = query.List<EventStoreItem>();

            //Next apply all history events
            foreach (EventStoreItem it in historyevents)
            {
                DomainEventsInfrastructure.DomainEventBase ev = Deserialize<DomainEventsInfrastructure.DomainEventBase>(Convert.FromBase64String(it.serdata)) as DomainEventsInfrastructure.DomainEventBase;
                retObj.applyStorageEvent( ev.GetType(),ev );
                retObj.EventCountSinceLastSnapshot++;
            }

            return retObj;
        }


/*        public Object DeserializeObject(String pXmlizedString)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return xs.Deserialize(memoryStream);

        }

        public String SerializeObject<TType>(TType pObject)
        {

            try
            {

                String XmlizedString = null;

                MemoryStream memoryStream = new MemoryStream();
                XmlSerializer xs = new XmlSerializer(typeof(TType));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

                xs.Serialize(xmlTextWriter, pObject);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
                return XmlizedString;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return null;
            }
        }

        private String UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);

        }

        private Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;

        }
*/
        private byte[] Serialize(object theObject)
        {
            using (var memoryStream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(memoryStream, theObject);
                return memoryStream.ToArray();
            }
        }

        private TType Deserialize<TType>(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            {
                return (TType)new BinaryFormatter().Deserialize(memoryStream);
            }
        }


        public void Append(EventStoreInfrastructure.IEventStoreBase b, bool fDoSnapShotAsWell)
        {
            foreach (DomainEventsInfrastructure.DomainEventBase changeevent in b.GetChanges())
            {
                //Connvert to item
                EventStoreItem oItem = new EventStoreItem();
                oItem.created = changeevent.Created;
                oItem.objectid = changeevent.EntityId;
                oItem.typ = 0;
                oItem.classtype = changeevent.GetType().ToString();
                oItem.artype = b.GetType().ToString();
                oItem.version = changeevent.Version;
                oItem.serdata = Convert.ToBase64String(Serialize(changeevent));
                //oItem.serdata = SerializeObject<DomainEventsInfrastructure.DomainEventBase>(changeevent);
                m_context.Save(oItem);
            }

            //Snapshot?
            if (fDoSnapShotAsWell)
            {
                EventStoreItem oItem = new EventStoreItem();
                oItem.created = DateTime.Now;
                oItem.objectid = b.Id;
                oItem.typ = 1;
                oItem.version = 0;
                oItem.serdata = Convert.ToBase64String(Serialize(b.CreateSnapshot()));
                oItem.classtype = b.GetType().ToString();
                oItem.artype = b.GetType().ToString();
                //oItem.serdata = SerializeObject<DomainEventsInfrastructure.DomainEventBase>(changeevent);
                m_context.Save(oItem);
            }


        }
    }
}
