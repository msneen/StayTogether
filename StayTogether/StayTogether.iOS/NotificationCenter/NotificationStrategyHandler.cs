using System.Collections.Generic;
using UIKit;


namespace StayTogether.iOS.NotificationCenter
{
    public class NotificationStrategyHandler
    {
        public static void ReceiveNotification(UILocalNotification notification, UIWindow window)
        {
            var notificationActions = new List<UIAlertAction>();
            // show an alert
            var okayAlertController = UIAlertController.Create(notification.AlertAction, notification.AlertBody,
                UIAlertControllerStyle.Alert);
            switch (notification.ApplicationIconBadgeNumber)
            {
                case 10101:
                    notificationActions = LostNotification.OnNotify(notification);
                    break;
                case 10102:
                    notificationActions = GroupInvitationNotification.OnNotify(notification);
                    break;
                case 10103:
                    notificationActions = LeftGroupNotification.OnNotify(notification);
                    break;
                case 10104:
                    notificationActions = InAnotherGroupNotification.OnNotify(notification);
                    break;
                default:
                    notificationActions.Add(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                    break;
            }
            foreach (var notificationAction in notificationActions)
            {               
                okayAlertController.AddAction(notificationAction);
            }

            window.RootViewController.PresentViewController(okayAlertController, true, null);

            // reset our badge
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        }


    }
}