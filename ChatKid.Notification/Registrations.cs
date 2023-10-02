using ChatKid.Notification.FcmNotificationConfig;
using ChatKid.Notification.Notification;
using CorePush.Apple;
using CorePush.Google;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.Notify
{
    public static class Registrations
    {
        /*public static IServiceCollection RegisterFcmService
            (this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddTransient<INotificationService, NotificationService>();
            services.AddHttpClient<FcmSender>();
            services.AddHttpClient<ApnSender>();
            

            // Configure strongly typed settings objects
            var appSettingsSection = configuration.GetSection("FcmNotification");
            services.Configure<FcmNotificationSetting>(appSettingsSection);


            return services;   
        }*/
    }
}
