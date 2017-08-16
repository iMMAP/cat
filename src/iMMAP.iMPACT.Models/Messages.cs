using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMMAP.iMPACT.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
        public List<string> To { get; set; }
        public DateTime Sent { get; set; }
    }

    public class Notification
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public List<string> To { get; set; }
        public DateTime Sent { get; set; }
        public bool Acknowledged { get; set; }
    }
}
