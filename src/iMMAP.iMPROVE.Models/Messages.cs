using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMMAP.iMPROVE.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
        public string InboxUserId { get; set; }
        public string[] To { get; set; }
        public string[] CC { get; set; }
        public DateTime Sent { get; set; }
    }

    public class Notification
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public string InboxUserId { get; set; }
        public DateTime Sent { get; set; }
        public bool Acknowledged { get; set; }
    }
}
