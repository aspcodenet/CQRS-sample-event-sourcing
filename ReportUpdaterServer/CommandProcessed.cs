using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ReportUpdaterServer
{
    public class CommandProcessed : IHandleMessages<ReportServerMessages.CommandProcessed>
    {
        public void Handle(ReportServerMessages.CommandProcessed message)
        {
            //ok we got this message from commandserver saying a command has been processed
            //now we need to apply yet not processed events to the report tables

            log4net.LogManager.GetLogger("CommandHandler").Info("Got CommandProcessed event");

            using (Systementor.Database.Repositories.IUnitOfWork uow = DB.DataContextReportSever.CreateUnitOfWork(true))
            {
                ReportUpdaterServer.RepositoryHelper.RepositoryList oRepositories = new RepositoryHelper.RepositoryList(uow);
                Systementor.Database.Repositories.IRepository<Classes.ReportParameter> repParam = oRepositories.CreateOrGetRepository<Classes.ReportParameter>();

                //Get from where to start - insert new logrecord if never ran at all
                Classes.ReportParameter param = GetOrCreateLastProcessed(repParam);

                using (Systementor.Database.Repositories.IUnitOfWork uow2 = DB.DataContextEventStore.CreateUnitOfWork(false))
                {
                    //Fetch all events after last processed
                    //Apply one at a time...
                    Systementor.Database.Repositories.IRepository<Classes.EventStore.EventStoreItem> repEventStore = uow2.CreateRepository<Classes.EventStore.EventStoreItem>();
                    foreach (Classes.EventStore.EventStoreItem it in repEventStore.ExecuteNamedQuery("qNotProcessed", Classes.EventStore.EventStoreItem.Criteria_LastProcessIdCriteria(param.intval)))
                    {
                        //Deserialize event
                        DomainEventsInfrastructure.DomainEventBase ev = Deserialize<DomainEventsInfrastructure.DomainEventBase>(Convert.FromBase64String(it.serdata));                
                        
                        HandleDomainEvent(ev, oRepositories);
                        param.intval = it.Id;
                    }
                }


                //Update logrecord - now we have processed events up to param.intval
                repParam.Update(param);

            }



        }


        public Classes.ReportParameter GetOrCreateLastProcessed(Systementor.Database.Repositories.IRepository<Classes.ReportParameter> repParam)
        {
            Classes.ReportParameter param = repParam.ExecuteNamedQuery("qLastProcessedEvent").SingleOrDefault();
            if (param == null)
            {
                param = new Classes.ReportParameter();
                param.Paramname = "lastprocessed";
                param.intval = -1;
                repParam.Insert(param);
            }
            return param;
        }

        public void HandleDomainEvent(DomainEventsInfrastructure.DomainEventBase ev, ReportUpdaterServer.RepositoryHelper.RepositoryList repList)
        {
            Guid entid = ev.EntityId;

            //Ugly swicth, I'll do something about it later...
            NerdDinnerDomainEvents.UserCreatedEvent evUserCreated = ev as NerdDinnerDomainEvents.UserCreatedEvent;
            if (evUserCreated != null)
            {
                //Create the user...
                Systementor.Database.Repositories.IRepository<Classes.ReportUser> rep = repList.CreateOrGetRepository<Classes.ReportUser>();
                Classes.ReportUser user = new Classes.ReportUser();
                //Classes.ReportUser user = rep.Get(p => p.User_Id == entid);
                
                user.User_Id = entid;
                user.Forname = evUserCreated.Forname;
                user.Surname = evUserCreated.Surname;
                rep.Insert(user);
                return;
            }
            NerdDinnerDomainEvents.UserChangedNameEvent evUserChangedName = ev as NerdDinnerDomainEvents.UserChangedNameEvent;
            if (evUserChangedName != null)
            {
                //Create the user...
                Systementor.Database.Repositories.IRepository<Classes.ReportUser> rep = repList.CreateOrGetRepository<Classes.ReportUser>();
                //Classes.ReportUser user = rep.ExecuteNamedQuery("qUserById", Classes.ReportUser.Criteria_ById(entid)).SingleOrDefault();
                Classes.ReportUser user = rep.GetById(entid);

                user.Forname = evUserChangedName.Forname;
                user.Surname = evUserChangedName.Surname;
                rep.Update(user);
                return;
            }
            NerdDinnerDomainEvents.DinnerCreatedEvent evDinnerCreated = ev as NerdDinnerDomainEvents.DinnerCreatedEvent;
            if (evDinnerCreated != null)
            {
                //Create the user...
                Systementor.Database.Repositories.IRepository<Classes.ReportDinner> repDinner = repList.CreateOrGetRepository<Classes.ReportDinner>();
                Systementor.Database.Repositories.IRepository<Classes.ReportUser> repUser = repList.CreateOrGetRepository<Classes.ReportUser>();
                Classes.ReportDinner oDinner = new Classes.ReportDinner();
                //Classes.ReportUser user = rep.Get(p => p.User_Id == entid);

                oDinner.Dinner_Id = entid;
                oDinner.Location = evDinnerCreated.Location;
                oDinner.Description = evDinnerCreated.Description;
                oDinner.Date = evDinnerCreated.Date;
                oDinner.Organizer_User_id = evDinnerCreated.Organizer_User_id;
                //Get user full name
                Classes.ReportUser user = repUser.GetById(oDinner.Organizer_User_id);
                oDinner.Organizer_Fullname = user.Forname + " " + user.Surname;

                //Add itself as coming
                oDinner.UsersComing.Add(user);

                repDinner.Insert(oDinner);
                return;

            }


        }


        private TType Deserialize<TType>(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            {
                return (TType)new BinaryFormatter().Deserialize(memoryStream);
            }
        }



    }
}
