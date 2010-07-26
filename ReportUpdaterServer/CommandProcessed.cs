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
        ReportUpdaterServer.RepositoryHelper.RepositoryList repList;

        public void Handle(ReportServerMessages.CommandProcessed message)
        {
            //ok we got this message from commandserver saying a command has been processed
            //now we need to apply yet not processed events to the report tables

            log4net.LogManager.GetLogger("CommandHandler").Info("Got CommandProcessed event");

            using (Systementor.Database.Repositories.IUnitOfWork uow = DB.DataContextReportSever.CreateUnitOfWork(true))
            {
                repList = new RepositoryHelper.RepositoryList(uow);
                Systementor.Database.Repositories.IRepository<Classes.ReportParameter> repParam = repList.CreateOrGetRepository<Classes.ReportParameter>();

                //Get from where to start - insert new logrecord if never ran at all
                Classes.ReportParameter param = GetOrCreateLastProcessed(repParam);

                //Register handlers for our events in a dispatcher
                DomainEventsInfrastructure.DomainEventDispatcher disp = new DomainEventsInfrastructure.DomainEventDispatcher();
                disp.RegisterEventHandler<NerdDinnerDomainEvents.DinnerCreatedEvent>(OnDinnerCreatedEvent);
                disp.RegisterEventHandler<NerdDinnerDomainEvents.UserChangedNameEvent>(OnUserChangedNameEvent);
                disp.RegisterEventHandler<NerdDinnerDomainEvents.UserCreatedEvent>(OnUserCreatedEvent);
                disp.RegisterEventHandler<NerdDinnerDomainEvents.DinnerModifiedTimeEvent>(OnDinnerModifiedTimeEvent);
                disp.RegisterEventHandler<NerdDinnerDomainEvents.DinnerModifiedLocationEvent>(OnDinnerModifiedLocationEvent);
                disp.RegisterEventHandler<NerdDinnerDomainEvents.DinnerModifiedDescriptionEvent>(OnDinnerModifiedDescriptionEvent);
                disp.RegisterEventHandler<NerdDinnerDomainEvents.DinnerAddedParticipantEvent>(OnDinnerAddedParticipantEvent);
                disp.RegisterEventHandler<NerdDinnerDomainEvents.DinnerRemovedParticipantEvent>(OnDinnerRemovedParticipantEvent);


                using (Systementor.Database.Repositories.IUnitOfWork uow2 = DB.DataContextEventStore.CreateUnitOfWork(false))
                {

                    //Fetch all events after last processed
                    //Apply one at a time...
                    Systementor.Database.Repositories.IRepository<Classes.EventStore.EventStoreItem> repEventStore = uow2.CreateRepository<Classes.EventStore.EventStoreItem>();
                    foreach (Classes.EventStore.EventStoreItem it in repEventStore.ExecuteNamedQuery("qNotProcessed", Classes.EventStore.EventStoreItem.Criteria_LastProcessIdCriteria(param.intval)))
                    {
                        //Deserialize event
                        DomainEventsInfrastructure.DomainEventBase ev = Deserialize<DomainEventsInfrastructure.DomainEventBase>(Convert.FromBase64String(it.serdata));
                        disp.Publish<DomainEventsInfrastructure.DomainEventBase>(ev);
                        //HandleDomainEvent(ev, repList);
                        param.intval = it.Id;
                    }
                }


                //Update logrecord - now we have processed events up to param.intval
                repParam.Update(param);

            }



        }

        void OnDinnerCreatedEvent(NerdDinnerDomainEvents.DinnerCreatedEvent evDinnerCreated)
        {
            Guid entid = evDinnerCreated.EntityId;
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

        }
        void OnUserChangedNameEvent(NerdDinnerDomainEvents.UserChangedNameEvent evUserChangedName)
        {
            Guid entid = evUserChangedName.EntityId;
            Systementor.Database.Repositories.IRepository<Classes.ReportUser> rep = repList.CreateOrGetRepository<Classes.ReportUser>();
            //Classes.ReportUser user = rep.ExecuteNamedQuery("qUserById", Classes.ReportUser.Criteria_ById(entid)).SingleOrDefault();
            Classes.ReportUser user = rep.GetById(entid);

            user.Forname = evUserChangedName.Forname;
            user.Surname = evUserChangedName.Surname;
            rep.Update(user);

            //Update all the dinners the user is arranging
            //shortcut with defined query instead of looping in repository
            rep.ExecuteNamedQuery("cmdUpdateFullNameOnReportDinners",
                            Classes.ReportDinner.cmdUpdateFullNameOnReportDinners_Parameters(user.Forname + "," + user.Surname, user.User_Id));


        }

        void OnDinnerModifiedTimeEvent(NerdDinnerDomainEvents.DinnerModifiedTimeEvent evDinnerModifiedTime)
        {
            Guid entid = evDinnerModifiedTime.EntityId;
            Systementor.Database.Repositories.IRepository<Classes.ReportDinner> rep = repList.CreateOrGetRepository<Classes.ReportDinner>();
            Classes.ReportDinner oDinner = rep.GetById(evDinnerModifiedTime.EntityId);
            //Classes.ReportUser user = rep.Get(p => p.User_Id == entid);

            oDinner.Date = evDinnerModifiedTime.Date;
            rep.Update(oDinner);
        }
        void OnDinnerModifiedLocationEvent(NerdDinnerDomainEvents.DinnerModifiedLocationEvent evDinnerModifiedTime)
        {
            Guid entid = evDinnerModifiedTime.EntityId;
            Systementor.Database.Repositories.IRepository<Classes.ReportDinner> rep = repList.CreateOrGetRepository<Classes.ReportDinner>();
            Classes.ReportDinner oDinner = rep.GetById(evDinnerModifiedTime.EntityId);
            //Classes.ReportUser user = rep.Get(p => p.User_Id == entid);

            oDinner.Location = evDinnerModifiedTime.Location;
            rep.Update(oDinner);
        }

        void OnDinnerModifiedDescriptionEvent(NerdDinnerDomainEvents.DinnerModifiedDescriptionEvent evDinnerModifiedTime)
        {
            Guid entid = evDinnerModifiedTime.EntityId;
            Systementor.Database.Repositories.IRepository<Classes.ReportDinner> rep = repList.CreateOrGetRepository<Classes.ReportDinner>();
            Classes.ReportDinner oDinner = rep.GetById(evDinnerModifiedTime.EntityId);
            //Classes.ReportUser user = rep.Get(p => p.User_Id == entid);

            oDinner.Description = evDinnerModifiedTime.Description;
            rep.Update(oDinner);
        }

        void OnDinnerRemovedParticipantEvent(NerdDinnerDomainEvents.DinnerRemovedParticipantEvent evDinnerModifiedTime)
        {
            Guid entid = evDinnerModifiedTime.EntityId;
            Systementor.Database.Repositories.IRepository<Classes.ReportDinner> rep = repList.CreateOrGetRepository<Classes.ReportDinner>();
            Systementor.Database.Repositories.IRepository<Classes.ReportUser> repUsers = repList.CreateOrGetRepository<Classes.ReportUser>();
            Classes.ReportDinner oDinner = rep.GetById(evDinnerModifiedTime.EntityId);
            Classes.ReportUser user = repUsers.GetById(evDinnerModifiedTime.User_id);

            oDinner.UsersComing.Remove(user);
            rep.Update(oDinner);
        }
        

        void OnDinnerAddedParticipantEvent(NerdDinnerDomainEvents.DinnerAddedParticipantEvent evDinnerModifiedTime)
        {
            Guid entid = evDinnerModifiedTime.EntityId;
            Systementor.Database.Repositories.IRepository<Classes.ReportDinner> rep = repList.CreateOrGetRepository<Classes.ReportDinner>();
            Systementor.Database.Repositories.IRepository<Classes.ReportUser> repUsers = repList.CreateOrGetRepository<Classes.ReportUser>();
            Classes.ReportDinner oDinner = rep.GetById(evDinnerModifiedTime.EntityId);
            Classes.ReportUser user = repUsers.GetById(evDinnerModifiedTime.User_id);

            oDinner.UsersComing.Add( user );
            rep.Update(oDinner);
        }


        void OnUserCreatedEvent(NerdDinnerDomainEvents.UserCreatedEvent evUserCreated)
        {
            Guid entid = evUserCreated.EntityId;
            Systementor.Database.Repositories.IRepository<Classes.ReportUser> rep = repList.CreateOrGetRepository<Classes.ReportUser>();
            Classes.ReportUser user = new Classes.ReportUser();
            //Classes.ReportUser user = rep.Get(p => p.User_Id == entid);

            user.User_Id = entid;
            user.Forname = evUserCreated.Forname;
            user.Surname = evUserCreated.Surname;
            rep.Insert(user);

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

        private TType Deserialize<TType>(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            {
                return (TType)new BinaryFormatter().Deserialize(memoryStream);
            }
        }



    }
}
