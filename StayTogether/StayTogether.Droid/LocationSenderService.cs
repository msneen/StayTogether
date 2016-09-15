using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;

namespace StayTogether.Droid
{
    [Service]
    [IntentFilter(new String[] {"com.StayTogether.LocationSenderService"})]
    public class LocationSenderService : Service
    {
        private LocationSenderBinder _binder;

        public void StartForeground()
        {
            Intent notificationIntent = new Intent(this, typeof(MainActivity));

            PendingIntent pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, 0);

            Notification notification = new Notification.Builder(this)
                .SetSmallIcon(Resource.Drawable.Icon)
                .SetContentTitle("Stay Together")
                .SetContentText("Tracking...")
                .SetContentIntent(pendingIntent).Build();

            StartForeground(1337, notification);
        }

        public void StopForeground()
        {
            StopForeground(true);
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            new Task(async () =>
            {
                var locationSender = new LocationSender();

                await locationSender.InitializeSignalRAsync();

                await locationSender.UpdateGeoLocationAsync(DateTime.Now + TimeSpan.FromHours(4));

            }).Start();
            return StartCommandResult.Sticky;
        }



        public override IBinder OnBind(Intent intent)
        {
            _binder = new LocationSenderBinder(this);
            return _binder;
        }
    }

    public class LocationSenderBinder : Binder
    {
        private readonly LocationSenderService _locationSenderService;

        public LocationSenderBinder(LocationSenderService locationSenderService)
        {
            _locationSenderService = locationSenderService;
        }

        public LocationSenderService GetLocationSenderService()
        {
            return _locationSenderService;
        }
    }
}