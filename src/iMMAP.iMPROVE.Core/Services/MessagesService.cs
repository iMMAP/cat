using iMMAP.iMPROVE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xod;

namespace iMMAP.iMPROVE.Core.Services
{
    public class NotificationEventArgs
    {
        public string Body { get; set; }
        public DateTime Time { get; set; }
        public string[] Recipients { get; set; }
    }

    public interface IMessagesService
    {
        event EventHandler<NotificationEventArgs> NotificationGenerated;
        event EventHandler<NotificationEventArgs> MessageGenerated;

        List<Notification> GetNotifications(string userId);
        List<Message> GetMessages(string userId);
        void Notify(string[] recipients, string notification, DateTime time);
        void AknowledgeNotification(Guid id);
        void SendMessage(string sender, string[] to, string[] cc, string subject, string message, DateTime time);
        void DeleteNotification(Guid id);
        void DeleteMessage(Guid id);
    }

    public class MessagesService : IMessagesService
    {
        IDataService db = null;

        public event EventHandler<NotificationEventArgs> NotificationGenerated;
        public event EventHandler<NotificationEventArgs> MessageGenerated;

        public MessagesService(IDataService db)
        {
            this.db = db;
        }

        public void Notify(string[] recipients, string notification, DateTime time)
        {
            foreach (var recipient in recipients)
            {
                Notification note = new Notification()
                {
                    InboxUserId = recipient,
                    Body = notification,
                    Sent = time
                };

                db.Data.Insert(note);
            }

            if (NotificationGenerated != null)
            {
                NotificationGenerated(this, new NotificationEventArgs()
                {
                    Body = notification,
                    Time = time,
                    Recipients = recipients
                });
            }
        }

        public void AknowledgeNotification(Guid id)
        {
            db.Data.Update(
                new Notification() { Id = id, Acknowledged = true },
                new UpdateFilter() { Behavior = UpdateFilterBehavior.Target, Properties = new string[] { "Acknowledged" } });
        }

        public void SendMessage(string sender, string[] to, string[] cc, string subject, string body, DateTime time)
        {
            foreach (var recipient in to.Concat(cc))
            {
                Message message = new Message()
                {
                    InboxUserId = recipient,
                    Sent = time,
                    From = sender,
                    To = to,
                    CC = cc,
                    Subject = subject,
                    Body = body
                };

                db.Data.Insert(message);
            }

            if (MessageGenerated != null)
            {
                MessageGenerated(this, new NotificationEventArgs()
                {
                    Body = subject + " ... " + string.Join("", body.Take(128)),
                    Time = time,
                    Recipients = to.Concat(cc).ToArray()
                });
            }
        }

        public List<Notification> GetNotifications(string userId)
        {
            return db.Data.Query<Notification>(s => s.InboxUserId == userId).ToList();
        }

        public List<Message> GetMessages(string userId)
        {
            return db.Data.Query<Message>(s => s.InboxUserId == userId).ToList();
        }

        public void DeleteNotification(Guid id)
        {
            db.Data.Delete(new Notification() { Id = id });
        }

        public void DeleteMessage(Guid id)
        {
            db.Data.Delete(new Message() { Id = id });
        }
    }
}