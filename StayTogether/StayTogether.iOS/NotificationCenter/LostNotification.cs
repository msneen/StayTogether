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
        public static void DisplayLostNotification(GroupMemberVm groupMemberVm)
        {
            if (string.IsNullOrWhiteSpace(groupMemberVm.PhoneNumber)) return;

            var notification = CreateNotification("Someone Is lost", "Someone Is lost", 10101);

            var dictionary = GetDictionary(notification);

            AddValue("Name", groupMemberVm.Name, ref dictionary);
            AddValue("PhoneNumber", groupMemberVm.PhoneNumber, ref dictionary);
            AddValue("Latitude", groupMemberVm.Latitude.ToString(), ref dictionary);
            AddValue("Longitude", groupMemberVm.Longitude.ToString(), ref dictionary);

            notification.UserInfo = dictionary;


            UIApplication.SharedApplication.ScheduleLocalNotification(notification);
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
//            latitude= latitude < 1 ? 32.7818399 : latitude;
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