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
    public interface GroupJoinedCallback
    {
        void GroupJoined();
    }


    [Service]
    // ReSharper disable once RedundantExplicitArrayCreation
    [IntentFilter(new string[] {"com.StayTogether.Droid.LocationSenderService"})]
    public class LocationSenderService : Service
    {
        private GroupJoinedCallback _groupJoinedCallback;

        private LocationSenderBinder _binder;
        private LocationSender _locationSender;

        public static LocationSenderService Instance;

        public void SetGroupJoinedCallback(GroupJoinedCallback groupJoinedCallback)
        {
            _groupJoinedCallback = groupJoinedCallback;
        }

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
            _locationSender = new LocationSender();
            _locationSender.InitializeSignalRAsync();
            _locationSender.OnSomeoneIsLost += (sender, args) =>
            {
                OnNotifySomeoneIsLost(args.GroupMember);
            };
            _locationSender.OnGroupJoined += (sender, args) =>
            {
                //Todo: Test: send this through to the activity and hide the contact list
                _groupJoinedCallback.GroupJoined();
            };
        }

        private void OnNotifySomeoneIsLost(GroupMemberVm groupMember)
        {
            Helpers.NotificationUtils.DisplayLostNotification(this, groupMember);
        }

        public static string GetPhoneNumber()
        {
            var existingNumber = CrossSettings.Current.GetValueOrDefault<string>("phonenumber");
            if (!string.IsNullOrWhiteSpace(existingNumber))
            {
                return existingNumber;
            }

            var info = (TelephonyManager)Application.Context.GetSystemService(TelephonyService);
            var phoneNumber = info.Line1Number;
            CrossSettings.Current.AddOrUpdateValue("phonenumber", phoneNumber);
            return phoneNumber;
        }

        public override IBinder OnBind(Intent intent)
        {
            _binder = new LocationSenderBinder(this);
            return _binder;
        }
    }
}