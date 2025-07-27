using Microsoft.AspNetCore.SignalR;
using Sanayii.Core.Entities;
using Sanayii.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Service.Hubs
{
    public class NotificationHub:Hub
    {
        private readonly SanayiiContext _db;
        public NotificationHub(SanayiiContext db) {
            _db = db;
        }
        public async Task sendNotification(Notification notification)
        {
            //store at DB
            _db.Notifications.Add(notification);
            await _db.SaveChangesAsync();

            // Notify a specific user by their userId
            await Clients.User(notification.UserId).SendAsync("ReceiveNotification", notification);
        }
    }
}
