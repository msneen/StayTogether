using System;
using Android.Content;
using Android.OS;
using StayTogether.Droid.Activities;

namespace StayTogether.Droid.Services
{
    public class LocationSenderServiceConnection : Java.Lang.Object, IServiceConnection
    {
        private readonly MainActivity _activity;
        private LocationSenderService _locationSenderService;

        public LocationSenderServiceConnection(MainActivity activity)
        {
            _activity = activity;
        }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            var locationSenderBinder = service as LocationSenderBinder;
            if (locationSenderBinder != null)
            {
                _activity.Binder = locationSenderBinder;
                _activity.IsBound = true;
                _locationSenderService = locationSenderBinder.GetLocationSenderService();
                _locationSenderService.SetGroupJoinedCallback(_activity);
            }
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            _activity.IsBound = false;            
        }

    }
}