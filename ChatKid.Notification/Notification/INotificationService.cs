using ChatKid.Notification.Notification.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.Notification.Notification
{
    public interface INotificationService
    {
        public Task<ResponseModel> PushNotification(NotificationModel notificationModel); 
    }
}
