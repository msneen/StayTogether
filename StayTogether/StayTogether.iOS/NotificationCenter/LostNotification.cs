using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using Plugin.ExternalMaps;
using Plugin.ExternalMaps.Abstractions;
using StayTogether.Classes;
using UIKit;

namespace StayTogether.iOS.NotificationCenter
{
    public class LostNotification : NotificationBase
    {
        //private static Dictionary<string, UILocalNotification> _groupMemberUiLocalNotifications = new Dictionary<string, UILocalNotification>();

        public static void DisplayLostNotification(GroupMemberVm groupMemberVm)
        {
            if (string.IsNullOrWhiteSpace(groupMemberVm.PhoneNumber)) return;

            //RemovePreviousNotification(groupMemberVm);

            var previousNotifications = UIApplication
                                .SharedApplication
                                .ScheduledLocalNotifications
                                .Where(n => n.ApplicationIconBadgeNumber == 10101 &&  n.UserInfo["PhoneNumber"].ToString() == groupMemberVm.PhoneNumber)
                                .ToList();

            var notification = previousNotifications.Count > 0? previousNotifications[0] : CreateNotification("Someone Is lost", "Someone Is lost", 10101);

            var dictionary = GetDictionary(notification);

            AddValue("Name", groupMemberVm.Name, ref dictionary);
            AddValue("PhoneNumber", groupMemberVm.PhoneNumber, ref dictionary);
            AddValue("Latitude", groupMemberVm.Latitude.ToString(), ref dictionary);
            AddValue("Longitude", groupMemberVm.Longitude.ToString(), ref dictionary);

            notification.UserInfo = dictionary;

            //_groupMemberUiLocalNotifications.Add(groupMemberVm.PhoneNumber, notification);
            if (!previousNotifications.Any())
            {
                UIApplication.SharedApplication.ScheduleLocalNotification(notification);
            }
        }

        private static void RemovePreviousNotification(GroupMemberVm groupMemberVm)
        {
//#if (!DEBUG) //Todo:  Turn me on for release.  Supress the error in release mode
//            try
//            {
//#endif
                //var oldNotification = _groupMemberUiLocalNotifications[groupMemberVm.PhoneNumber];
                //if (oldNotification == null) return;

            var previousNotifications = UIApplication
                                            .SharedApplication
                                            .ScheduledLocalNotifications
                                            .Where(n => n.ApplicationIconBadgeNumber == 10101 && n.UserInfo["PhoneNumber"].ToString() == groupMemberVm.PhoneNumber)
                                            .ToList();

            for (var i = 0; i < previousNotifications.Count(); i++)
            {
                UIApplication.SharedApplication.CancelLocalNotification(previousNotifications[i]);
            }


                ////UIApplication.SharedApplication.CancelLocalNotification(oldNotification);
                ////_groupMemberUiLocalNotifications.Remove(groupMemberVm.PhoneNumber);
//#if (!DEBUG)
//            }
//            catch (Exception)
//            {
                
//            }
//#endif
        }


        public static List<UIAlertAction> OnNotify(UILocalNotification notification)
        {
            var actions = new List<UIAlertAction>();
            var dictionary = notification.UserInfo;
            var name = GetValue("Name", ref dictionary);
            var phoneNumber = GetValue("PhoneNumber", ref dictionary);
            var latitude =  Convert.ToDouble(GetValue("Latitude", ref dictionary));
            var longitude = Convert.ToDouble(GetValue("Longitude", ref dictionary));

//#if (DEBUG)
//            latitude = latitude < 1 ? 32.7818399 : latitude;
//            longitude = longitude < 1 ? -117.1112642 : longitude;
//#endif

            var okAction =  UIAlertAction.Create("OK", UIAlertActionStyle.Default, null);
            var mapAction = UIAlertAction.Create("View On Map", UIAlertActionStyle.Default, alertAction =>
            {
                var nameOrPhone = ContactsHelper.NameOrPhone(phoneNumber, name);
                CrossExternalMaps.Current.NavigateTo(nameOrPhone, latitude, longitude, NavigationType.Default);

            });

            actions.Add(okAction);
            actions.Add(mapAction);
            return actions;
        }
    }
}