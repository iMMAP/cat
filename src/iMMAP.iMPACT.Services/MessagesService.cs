using iMMAP.iMPACT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xod;

namespace iMMAP.iMPACT.Services
{
    public interface IMessagesService
    {
        void Notify(Notification note);
        void AknowledgeNotification(int id);
        void SendMessage(Message message);
    }

    public class MessagesService : IMessagesService
    {
        IDataService db = null;

        public MessagesService(IDataService db)
        {
            this.db = db;
        }

        public void Notify(Notification note)
        {
            db.Data.Insert(note);

            //TODO: real time update code
        }

        public void AknowledgeNotification(int id)
        {
            db.Data.Update(
                new Notification() { Id = id, Acknowledged = true },
                new UpdateFilter() { Behavior = UpdateFilterBehavior.Target, Properties = { "Acknowledged" } });
        }

        public void SendMessage(Message message)
        {
            db.Data.Insert(message);
        }
    }
}