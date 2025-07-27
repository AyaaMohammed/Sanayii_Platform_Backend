using Sanayii.Core.Entities;
using Sanayii.Core.Entities.Identity;
using Sanayii.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Repository.Repository
{
    public class NotificationRepository:GenericRepository<Notification>
    {
        public NotificationRepository(SanayiiContext db) : base(db)
        {
        }
        public List<Notification> getCustomerNotification(string customerId)
        {
            var notifications = db.Notifications
                .Where(n => n.UserId == customerId)
                .ToList();
            return notifications;
        }
        public void MarkAsRead(string customerid)
        {
            var notifications = db.Notifications
                .Where(n => n.UserId == customerid)
                .ToList();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                db.Entry(notification).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }

        }
        public void MarkNotification(int id)
        {
            var notification = db.Notifications.Find(id);
            notification.IsRead = true;
            db.Entry(notification).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
        public void DeleteCustomerNotification(string customerid)
        {
            var notifications = db.Notifications
                .Where(n => n.UserId == customerid)
                .ToList();

            foreach (var notification in notifications)
            {
                db.Entry(notification).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            }
        }


    }
}
