using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using iMMAP.iMPROVE.Models;
using Microsoft.AspNet.SignalR.Hubs;
using System;

namespace iMMAP.iMPROVE.Hubs
{
    [HubName("messagesHub")]
    public class MessagesHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<MessagesHub>();
        private static Dictionary<string, string> connectedUsers = new Dictionary<string, string>();

        public override Task OnConnected()
        {
            if (!connectedUsers.ContainsKey(this.Context.User.Identity.Name))
            {
                connectedUsers.Add(this.Context.User.Identity.Name, this.Context.ConnectionId);
            }
            else
            {
                connectedUsers[this.Context.User.Identity.Name] = this.Context.ConnectionId;
            }

            Clients.All.usersConnected(connectedUsers.Count);
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            if(!connectedUsers.ContainsKey(this.Context.User.Identity.Name))
            {
                connectedUsers.Add(this.Context.User.Identity.Name, this.Context.ConnectionId);
            }
            else
            {
                connectedUsers[this.Context.User.Identity.Name] = this.Context.ConnectionId;
            }

            Clients.All.usersConnected(connectedUsers.Count);
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            connectedUsers.Remove(this.Context.User.Identity.Name);
            Clients.All.usersConnected(connectedUsers.Count);
            return base.OnDisconnected(stopCalled);
        }

        public static void Broadcast(string message)
        {
            hubContext.Clients.All.broadcast(message);
        }

        public static void Notify(string[] users, string notification, DateTime time)
        {
            if (connectedUsers.Any())
            {
                List<string> clients = connectedUsers.Where(s => users.Contains(s.Key)).Select(s => s.Value).ToList();
                hubContext.Clients.Clients(clients).notify(notification, time);
            }
        }

        public static void Message(string[] users, string message, DateTime time)
        {
            if (connectedUsers.Any())
            {
                List<string> clients = connectedUsers.Where(s => users.Contains(s.Key)).Select(s => s.Value).ToList();
                hubContext.Clients.Clients(clients).message(message, time);
            }
        }
    }
}