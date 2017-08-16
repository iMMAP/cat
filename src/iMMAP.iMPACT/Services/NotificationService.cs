using iMMAP.iMPACT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iMMAP.iMPACT.Services
{
    public interface INotificationService
    {
        void Notify(Notification note);
    }

    public class NotificationService: INotificationService
    {
        IDataService db = null;

        public NotificationService(IDataService db)
        {
            this.db = db;
        }

        public void Notify(Notification note)
        {
            db.Data.Insert(note);

            //TODO: real time update code
        }
    }
}