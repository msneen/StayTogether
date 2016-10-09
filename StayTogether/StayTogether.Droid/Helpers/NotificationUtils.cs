using Android.App;
using Android.Content;
using Android.Support.V4.App;
using StayTogether.Classes;
using String = System.String;

namespace StayTogether.Droid.Helpers
{
    public class NotificationUtils
    {
        public static readonly int NOTIFICATION_ID = 1;

        public static readonly String ACTION_1 = "action_1";

        public static void DisplayLostNotification(Context context, GroupMemberVm groupMemberVm) //Todo: genericize me
        {

            var action1Intent = new Intent(context, typeof(NotificationActionService))
                .SetAction(ACTION_1);
            action1Intent.PutExtra("phonenumber", groupMemberVm.PhoneNumber);
            action1Intent.PutExtra("latitude", groupMemberVm.Latitude);
            action1Intent.PutExtra("longitude", groupMemberVm.Longitude);
            action1Intent.PutExtra("name", groupMemberVm.Name);

            var action1PendingIntent = PendingIntent.GetService(context, 0,
                action1Intent, PendingIntentFlags.OneShot);

            var notificationBuilder =
                new NotificationCompat.Builder(context)
                    .SetSmallIcon(Resource.Drawable.Icon)
                    .SetContentTitle("Map Lost Person")
                    .SetContentText("Click to view lost person on map")
                    .AddAction(new NotificationCompat.Action(Resource.Drawable.Icon,
                        "Action 1", action1PendingIntent));

            var notificationManager = NotificationManagerCompat.From(context);
            notificationManager.Notify(NOTIFICATION_ID, notificationBuilder.Build());
        }

        public class NotificationActionService : IntentService
        {

            protected override void OnHandleIntent(Intent intent)
            {
                var action = intent.Action;
                if (ACTION_1.Equals(action))
                {
                    // TODO: handle action 1.
                    var name = intent.GetStringExtra("name");
                    var phoneNumber = intent.GetStringExtra("phonenumber");
                    var latitude = intent.GetDoubleExtra("latitude", 0);
                    var longitude = intent.GetDoubleExtra("longitude", 0);

                    //Launch map here.
                }
            }
        }
    }
}