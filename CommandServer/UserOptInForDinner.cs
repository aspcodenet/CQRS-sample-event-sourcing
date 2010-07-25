using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace CommandServer
{
    public class UserOptInForDinner : IHandleMessages<NerdCommandMessages.UserOptInForDinner>
    {
        public static IBus Bus { get; set; }

        public void Handle(NerdCommandMessages.UserOptInForDinner message)
        {
/*            Console.WriteLine( message.UserId.ToString()  +" opting in for dinner " + message.DinnerId.ToString());
            using (Systementor.Database.Repositories.IUnitOfWork uow = DB.DataContext.CreateUnitOfWork(true))
            {

                Systementor.Database.Repositories.IRepository<NerdDinnerDomain.Dinner> repdinner = uow.CreateRepository<NerdDinnerDomain.Dinner>();
                NerdDinnerDomain.Dinner oDinner = repdinner.Get(p => p.Id == message.DinnerId);
                if (oDinner == null) //Not much to do, dinner has disappeared
                    return;

                Systementor.Database.Repositories.IRepository<NerdDinnerDomain.User> rep = uow.CreateRepository<NerdDinnerDomain.User>();
                NerdDinnerDomain.User oUser = rep.Get(p => p.Id == message.UserId);
                if (oUser == null) //Not much to do, user has disappeared
                    return;

                    //First - ask yourself does this *really* belong to domain model???
                    //Yes in this case lets fake and say it does... 
                    //in reality this is areally simple op - but we wanna have something there for the sake of this example
                    //SO WHAT WILL HAPPEN
                    //a - added to the collection
                    oDinner.AddParticipant(oUser);
                    //b - or actually z - collection will be saved when out of scope here

                    //c - domainevent UserOptedInForDinnerEvent will be called - see code below



                //end it all by posting a message to the updater process...
                //string from = message.
                    Bus.Send(new ReportServerMessages.DinnerReservationUpdate { CommandMessageId = message.MessageGuid, DinnerId = oDinner.Id, MessageId = Guid.NewGuid(), OriginalSenderEndpoint = "", UserId = oUser.Id });

                //Saving is automatically when going out of scope since we're using unitofwork(true)
            }
 */
        }
 
    }
}
