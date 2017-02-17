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

        public static Dictionary<string, GroupMemberVm> LastLocation = new Dictionary<string, GroupMemberVm>();

        public static void DisplayLostNotification(GroupMemberVm groupMemberVm)
        {
            if (string.IsNullOrWhiteSpace(groupMemberVm.PhoneNumber)) return;

            LastLocation[groupMemberVm.PhoneNumber] = groupMemberVm;                    

            var previousNotifications = UIApplication
                                .SharedApplication
                                .ScheduledLocalNotifications
                                .Where(n => n.ApplicationIconBadgeNumber == 10101)
                                .ToList();

            UILocalNotification mostRecentNotification = null;
            foreach (var previousNotification in previousNotifications)
            {
                var userInfo = previousNotification.UserInfo;
                var phoneNumber = GetValue("PhoneNumber", ref userInfo);
                if (phoneNumber == groupMemberVm.PhoneNumber)
                {
                    return; //There is already a notification, we don't need to make another
                }
            }

            var notification = CreateNotification("Someone Is lost", "Someone Is lost", 10101);

            var dictionary = GetDictionary(notification);

            AddValue("Name", groupMemberVm.Name, ref dictionary);
            AddValue("PhoneNumber", groupMemberVm.PhoneNumber, ref dictionary);
            AddValue("Latitude", groupMemberVm.Latitude.ToString(), ref dictionary);
            AddValue("Longitude", groupMemberVm.Longitude.ToString(), ref dictionary);

            notification.UserInfo = dictionary;

            if (!previousNotifications.Any())
            {
                UIApplication.SharedApplication.ScheduleLocalNotification(notification);
            }
        }


        public static List<UIAlertAction> OnNotify(UILocalNotification notification)
        {
            var actions = new List<UIAlertAction>();
            var dictionary = notification.UserInfo;
            var name = GetValue("Name", ref dictionary);
            var phoneNumber = GetValue("PhoneNumber", ref dictionary);

            var okAction =  UIAlertAction.Create("OK", UIAlertActionStyle.Default, null);
            var mapAction = UIAlertAction.Create("View On Map", UIAlertActionStyle.Default, alertAction =>
            {
                var latitude = LastLocation[phoneNumber].Latitude;
                var longitude = LastLocation[phoneNumber].Longitude;

                var nameOrPhone = ContactsHelper.NameOrPhone(phoneNumber, name);
                CrossExternalMaps.Current.NavigateTo(nameOrPhone, latitude, longitude, NavigationType.Default);

            });

            actions.Add(okAction);
            actions.Add(mapAction);
            return actions;
        }
    }
}