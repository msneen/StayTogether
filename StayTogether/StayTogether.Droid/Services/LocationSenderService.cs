using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Telephony;
using Plugin.Geolocator.Abstractions;
using Plugin.Settings;
using StayTogether.Classes;
using StayTogether.Droid.Activities;
using StayTogether.Droid.Classes;
using StayTogether.Group;
using StayTogether.Location;

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
            var notification = DisplayServiceNotification();

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

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            StartLocationSender();

            return StartCommandResult.Sticky;
        }

        private Notification DisplayServiceNotification()
        {
            var notificationIntent = new Intent(this, typeof(MainActivity));
            var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, 0);
            var notification = new Notification.Builder(this)
                .SetSmallIcon(Resource.Drawable.Icon)
                .SetContentTitle("Stay Together")
                .SetContentText("Tracking...")
                .SetContentIntent(pendingIntent).Build();
            return notification;
        }

        public async void StartGroup(List<GroupMemberVm> contactList)
        {
            var position = GpsService.GetLocation();
            if (_locationSender == null || position == null) return;

            await CreateGroup(contactList, position);
        }
        private async Task CreateGroup(List<GroupMemberVm> contactList, Position position)
        {
            var groupVm = GroupHelper.InitializeGroupVm(contactList, position, GetPhoneNumber());

            await _locationSender.StartGroup(groupVm);
        }

        public async Task SendError(string message)
        {
            await _locationSender.SendError(message);
        }

        private void StartLocationSender()
        {
            var phoneNumber = GetPhoneNumber();
            InitializeLocationSender(phoneNumber);
            SendFirstPositionUpdate(phoneNumber);
        }

        private void SendFirstPositionUpdate(string phoneNumber)
        {
            var position = GpsService.GetLocation();
            if (position == null) return;

            var groupMemberVm = GroupMemberPositionAdapter.Adapt(position);
            groupMemberVm.PhoneNumber = phoneNumber;
            _locationSender.SendUpdatePosition(groupMemberVm);
        }

        private void InitializeLocationSender(string phoneNumber)
        {
            _locationSender = new LocationSender(phoneNumber);
            _locationSender.InitializeSignalRAsync();
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
}