using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Telephony;
using Plugin.Settings;
using StayTogether.Classes;
using StayTogether.Droid.Activities;
using VideoForwarder;

namespace StayTogether.Droid.Services
{
    [Service]
    // ReSharper disable once RedundantExplicitArrayCreation
    [IntentFilter(new string[] {"com.StayTogether.Droid.LocationSenderService"})]
    public class LocationSenderService : Service
    {
        private LocationSenderBinder _binder;
        private LocationSender _locationSender;

        public static LocationSenderService Instance;

        public void StartForeground()
        {
            var notificationIntent = new Intent(this, typeof(MainActivity));

            var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, 0);

            var notification = new Notification.Builder(this)
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

        public override void OnCreate()
        {
            base.OnCreate();
            Instance = this;
        }

        public async void StartGroup(List<GroupMemberVm> contactList)
        {
            var position = GpsService.GetLocation();
            if (_locationSender != null && position!= null)
            {
                var adminMember = new GroupMemberVm
                {
                    PhoneNumber = GetPhoneNumber(),
                    Name = CrossSettings.Current.GetValueOrDefault<string>("nickname"),
                    Latitude = position.Latitude,
                    Longitude = position.Longitude,
                    IsAdmin = true
                };
                contactList.Insert(0, adminMember);

                var userVm = new GroupVm
                {
                    ContactList = contactList,
                    GroupCreatedDateTime = DateTime.Now,
                    GroupDisbandDateTime = DateTime.Now.AddHours(5)
                };
                await _locationSender.StartGroup(userVm);
            }
        }

        public async Task SendError(string message)
        {
            await _locationSender.SendError(message);
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            var phoneNumber = GetPhoneNumber();
            var position = GpsService.GetLocation();

            _locationSender = new LocationSender(phoneNumber);            

            _locationSender.InitializeSignalRAsync();

            if (position != null)
            {
                var groupMemberVm = new GroupMemberVm()
                {
                    PhoneNumber = phoneNumber,
                    Latitude = position.Latitude,
                    Longitude = position.Longitude
                };
                _locationSender.SendUpdatePosition(groupMemberVm);
            }

            return StartCommandResult.Sticky;
        }

        public static string GetPhoneNumber()
        {
            var info = (TelephonyManager)Application.Context.GetSystemService(TelephonyService);
            return info.Line1Number;
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