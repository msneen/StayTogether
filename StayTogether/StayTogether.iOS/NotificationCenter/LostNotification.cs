using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using StayTogether.Classes;
using UIKit;

namespace StayTogether.iOS.NotificationCenter
{
    public class LostNotification : NotificationBase
    {
        public static void DisplayLostNotification(GroupMemberVm groupMemberVm)
        {
            if (string.IsNullOrWhiteSpace(groupMemberVm.PhoneNumber)) return;

            var notification = new UILocalNotification
            {
                FireDate = NSDate.Now,
                AlertAction = "View Alert",
                AlertBody = "Someone Is lost",
                AlertTitle = "Someone Is lost",
                ApplicationIconBadgeNumber = 10101,
                SoundName = UILocalNotification.DefaultSoundName
            };
            var dictionary = new NSMutableDictionary();

            AddValue("PhoneNumber", groupMemberVm.PhoneNumber, ref dictionary);
            AddValue("Latitude", groupMemberVm.Latitude.ToString(), ref dictionary);
            AddValue("Longitude", groupMemberVm.Longitude.ToString(), ref dictionary);

            notification.UserInfo = dictionary;

            UIApplication.SharedApplication.ScheduleLocalNotification(notification);
        }
    }
}