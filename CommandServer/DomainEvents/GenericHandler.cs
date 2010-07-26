using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandServer.DomainEvents
{
    public class GenericHandler
    {
        public static void Register()
        {
            NerdDinnerDomain.Events.DomainEvents.Instance.Dispatcher.RegisterEventHandler<NerdDinnerDomainEvents.UserOptedInForDinnerEvent>(OnUserOptedInForDinnerEvent);
            NerdDinnerDomain.Events.DomainEvents.Instance.Dispatcher.RegisterEventHandler<NerdDinnerDomainEvents.DinnerModifiedTimeEvent>(OnDinnerModifiedTimeEvent);
            NerdDinnerDomain.Events.DomainEvents.Instance.Dispatcher.RegisterEventHandler<NerdDinnerDomainEvents.DinnerModifiedLocationEvent>(OnDinnerModifiedLocationEvent);
            NerdDinnerDomain.Events.DomainEvents.Instance.Dispatcher.RegisterEventHandler<NerdDinnerDomainEvents.DinnerAddedParticipantEvent>(OnDinnerAddedParticipantEvent);
            NerdDinnerDomain.Events.DomainEvents.Instance.Dispatcher.RegisterEventHandler<NerdDinnerDomainEvents.DinnerRemovedParticipantEvent>(OnDinnerRemovedParticipantEvent);
            //NerdDinnerDomain.Events.DomainEvents.Instance..UserOptedInForDinnerEvent.Register(UserOptedInForDinnerHandler);
        }

        public static void OnDinnerRemovedParticipantEvent(NerdDinnerDomainEvents.DinnerRemovedParticipantEvent ev)
        {
            //Send email to that user - and to organizer
            using (EventStoreNhib.UnitOfWork uow = new EventStoreNhib.UnitOfWork(DB.GetFactory().OpenSession(), false))
            {
                EventStoreInfrastructure.IRepository<NerdDinnerDomain.Dinner> repDinners = uow.CreateRepository<NerdDinnerDomain.Dinner>();
                NerdDinnerDomain.Dinner oDinner = repDinners.GetById(ev.EntityId);
                string fromemail = "info@testabc123.com";
                string fromname = "system info";

                //Send to organizer
                EventStoreInfrastructure.IRepository<NerdDinnerDomain.User> repUsers = uow.CreateRepository<NerdDinnerDomain.User>();
                NerdDinnerDomain.User oUserOrg = repUsers.GetById(oDinner.Organizer_User_id);
                NerdDinnerDomain.User oUser = repUsers.GetById(ev.User_id);

                string toemail = oUserOrg.Forname + "." + oUserOrg.Surname + "@" + oUserOrg.Surname.ToString() + ".com";
                string toname = oUserOrg.Forname + " " + oUserOrg.Surname;
                string message = oUser.Forname + " " + oUser.Surname + " is NOT coming to your dinner at " + oDinner.Location + " " + oDinner.Date.ToString();
                MessageEndpoint.Bus.Send(new NerdCommandMessages.External.SendMail { MessageGuid = Guid.NewGuid(), FromEmail = fromemail, FromName = fromname, ToEmail = toemail, ToName = toname, Header = "Someone is NOT coming to your dinner", Message = message });

                toemail = oUser.Forname + "." + oUser.Surname + "@" + oUser.Surname.ToString() + ".com";
                toname = oUser.Forname + " " + oUser.Surname;
                message = "You have UNregistered for dinner at " + oDinner.Location + " " + oDinner.Date.ToString();
                MessageEndpoint.Bus.Send(new NerdCommandMessages.External.SendMail { MessageGuid = Guid.NewGuid(), FromEmail = fromemail, FromName = fromname, ToEmail = toemail, ToName = toname, Header = "Dinner unregistration", Message = message });


            }


        }


        public static void OnDinnerAddedParticipantEvent(NerdDinnerDomainEvents.DinnerAddedParticipantEvent ev)
        {
            //Send email to that user - and to organizer
            using (EventStoreNhib.UnitOfWork uow = new EventStoreNhib.UnitOfWork(DB.GetFactory().OpenSession(), false))
            {
                EventStoreInfrastructure.IRepository<NerdDinnerDomain.Dinner> repDinners = uow.CreateRepository<NerdDinnerDomain.Dinner>();
                NerdDinnerDomain.Dinner oDinner = repDinners.GetById(ev.EntityId);
                string fromemail = "info@testabc123.com";
                string fromname = "system info";

                //Send to organizer
                EventStoreInfrastructure.IRepository<NerdDinnerDomain.User> repUsers = uow.CreateRepository<NerdDinnerDomain.User>();
                NerdDinnerDomain.User oUserOrg = repUsers.GetById(oDinner.Organizer_User_id);
                NerdDinnerDomain.User oUser = repUsers.GetById(ev.User_id);

                string toemail = oUserOrg.Forname + "." + oUserOrg.Surname + "@" + oUserOrg.Surname.ToString() + ".com";
                string toname = oUserOrg.Forname + " " + oUserOrg.Surname;
                string message =  oUser.Forname + " " + oUser.Surname + " is coming to your dinner at " + oDinner.Location + " " + oDinner.Date.ToString();
                MessageEndpoint.Bus.Send(new NerdCommandMessages.External.SendMail { MessageGuid = Guid.NewGuid(), FromEmail = fromemail, FromName = fromname, ToEmail = toemail, ToName = toname, Header = "Someone is coming to your dinner", Message = message });

                toemail = oUser.Forname + "." + oUser.Surname + "@" + oUser.Surname.ToString() + ".com";
                toname = oUser.Forname + " " + oUser.Surname;
                message = "You have registered for dinner at " + oDinner.Location + " " + oDinner.Date.ToString();
                MessageEndpoint.Bus.Send(new NerdCommandMessages.External.SendMail { MessageGuid = Guid.NewGuid(), FromEmail = fromemail, FromName = fromname, ToEmail = toemail, ToName = toname, Header = "Dinner registration", Message = message });


            }



        }
        public static void OnDinnerModifiedTimeEvent(NerdDinnerDomainEvents.DinnerModifiedTimeEvent ev)
        {
            //Time is modified on this event...We need to generate the emails to send to all users
            //Those email are then sent to the external bus so the real email sender process can send
            using (EventStoreNhib.UnitOfWork uow = new EventStoreNhib.UnitOfWork(DB.GetFactory().OpenSession(), false))
            {
                EventStoreInfrastructure.IRepository<NerdDinnerDomain.Dinner> repDinners = uow.CreateRepository<NerdDinnerDomain.Dinner>();
                NerdDinnerDomain.Dinner oDinner = repDinners.GetById(ev.EntityId);
                string fromemail = "info@testabc123.com";
                string fromname = "system info";

                //Send to organizer
                EventStoreInfrastructure.IRepository<NerdDinnerDomain.User> repUsers = uow.CreateRepository<NerdDinnerDomain.User>();
                NerdDinnerDomain.User oUser = repUsers.GetById( oDinner.Organizer_User_id );

                string toemail = oUser.Forname + "." + oUser.Surname + "@" + oUser.Surname.ToString() + ".com";
                string toname = oUser.Forname + " " + oUser.Surname;
                string message = "You have modified the time for dinner at " + oDinner.Location + " " + oDinner.Date.ToString();
                MessageEndpoint.Bus.Send(new NerdCommandMessages.External.SendMail { MessageGuid = Guid.NewGuid(), FromEmail = fromemail, FromName = fromname, ToEmail = toemail, ToName = toname, Header = "Your dinner has changed", Message = message });

                foreach (Guid userid in oDinner.Participants)
                {
                    oUser = repUsers.GetById(userid);
                    toemail = oUser.Forname + "." + oUser.Surname + "@" + oUser.Surname.ToString() + ".com";
                    toname = oUser.Forname + " " + oUser.Surname;
                    message = "New dinner time at " + oDinner.Location + " " + oDinner.Date.ToString();
                    MessageEndpoint.Bus.Send(new NerdCommandMessages.External.SendMail { MessageGuid = Guid.NewGuid(), FromEmail = fromemail, FromName = fromname, ToEmail = toemail, ToName = toname, Header = "Dinner time has changed", Message = message });
                }
                
            }


        }


        public static void OnDinnerModifiedLocationEvent(NerdDinnerDomainEvents.DinnerModifiedLocationEvent ev)
        {
            //Location is modified on this event...We need to generate the emails to send to all users
            //Those email are then sent to the external bus so the real email sender process can send
            using (EventStoreNhib.UnitOfWork uow = new EventStoreNhib.UnitOfWork(DB.GetFactory().OpenSession(), false))
            {
                EventStoreInfrastructure.IRepository<NerdDinnerDomain.Dinner> repDinners = uow.CreateRepository<NerdDinnerDomain.Dinner>();
                NerdDinnerDomain.Dinner oDinner = repDinners.GetById(ev.EntityId);
                string fromemail = "info@testabc123.com";
                string fromname = "system info";

                //Send to organizer
                EventStoreInfrastructure.IRepository<NerdDinnerDomain.User> repUsers = uow.CreateRepository<NerdDinnerDomain.User>();
                NerdDinnerDomain.User oUser = repUsers.GetById(oDinner.Organizer_User_id);

                string toemail = oUser.Forname + "." + oUser.Surname + "@" + oUser.Surname.ToString() + ".com";
                string toname = oUser.Forname + " " + oUser.Surname;
                string message = "You have modified the location  for dinner at " + oDinner.Location + " " + oDinner.Date.ToString();
                MessageEndpoint.Bus.Send(new NerdCommandMessages.External.SendMail { MessageGuid = Guid.NewGuid(), FromEmail = fromemail, FromName = fromname, ToEmail = toemail, ToName = toname, Header = "Your dinner has changed", Message = message });

                foreach (Guid userid in oDinner.Participants)
                {
                    oUser = repUsers.GetById(userid);
                    toemail = oUser.Forname + "." + oUser.Surname + "@" + oUser.Surname.ToString() + ".com";
                    toname = oUser.Forname + " " + oUser.Surname;
                    message = "New dinner location at " + oDinner.Location + " " + oDinner.Date.ToString();
                    MessageEndpoint.Bus.Send(new NerdCommandMessages.External.SendMail { MessageGuid = Guid.NewGuid(), FromEmail = fromemail, FromName = fromname, ToEmail = toemail, ToName = toname, Header = "Dinner location has changed", Message = message });
                }

            }


        }



        public static void OnUserOptedInForDinnerEvent(NerdDinnerDomainEvents.UserOptedInForDinnerEvent ev)
        {
#if(false)
            string fromemail = "info@testabc123.com";
            string fromname = "system info";
            using (Systementor.Database.Repositories.IUnitOfWork uow = DB.DataContext.CreateUnitOfWork(false))
            {
                Systementor.Database.Repositories.IRepository<NerdDinnerDomain.User> rep = uow.CreateRepository<NerdDinnerDomain.User>();
                Systementor.Database.Repositories.IRepository<NerdDinnerDomain.Dinner> repDinner = uow.CreateRepository<NerdDinnerDomain.Dinner>();
                NerdDinnerDomain.Dinner oDinner = repDinner.Get(p => p.Id == args.DinnerId);

                NerdDinnerDomain.User oUser = rep.Get(p => p.Id == args.User_Id_Opted_In);
                NerdDinnerDomain.User o = rep.Get(p => p.Id == args.User_Id_Opted_In);
                if (args.Owner_User_id != args.User_Id_Opted_In)
                {
                    string toemail = oUser.Forname + "." + oUser.Surname + "@" + args.Owner_User_id.ToString() + ".com";
                    string toname = oUser.Forname + " " + oUser.Surname;
                    string message = "You have registered for dinner at " + oDinner.Location + " " + oDinner.Date.ToString();
                    MessageEndpoint.Bus.Send(new NerdCommandMessages.External.SendMail { MessageGuid = Guid.NewGuid(), FromEmail = fromemail, FromName = fromname, ToEmail = toemail, ToName = toname, Header = "You opted in for a dinner", Message=message });
                }
                //Message to owner
                NerdDinnerDomain.User oUserOwner = rep.Get(p => p.Id == args.Owner_User_id);
                string toemailowner = oUserOwner.Forname + "." + oUserOwner.Surname + "@" + oUserOwner.Id.ToString() + ".com";
                string tonameowner = oUserOwner.Forname + " " + oUserOwner.Surname;
                string messageowner =  oUser.Forname + " " + oUser.Surname + " have registered for the dinner you are arranging at " + oDinner.Location + " " + oDinner.Date.ToString();
                MessageEndpoint.Bus.Send(new NerdCommandMessages.External.SendMail { MessageGuid = Guid.NewGuid(), FromEmail = fromemail, FromName = fromname, ToEmail = toemailowner, ToName = tonameowner, Header = "Someone coming to your dinner", Message = messageowner });


                
            }
#endif



        }

  

    }
}
