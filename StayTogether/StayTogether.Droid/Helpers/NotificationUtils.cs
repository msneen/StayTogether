using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Plugin.ExternalMaps;
using Plugin.ExternalMaps.Abstractions;
using StayTogether.Classes;
using StayTogether.Droid.Services;
using String = System.String;

namespace StayTogether.Droid.Helpers
{

    [Activity(Label = "Lost Person Notification", MainLauncher = false, Icon = "@drawable/icon")]
    public class LostPersonNotificationActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)//Intent intent)
        {
            base.OnCreate(bundle);

            var action = Intent.Action;
            if (LocationSenderService.ShowLostMemberOnMap.Equals(action))
            {
                //Launch map here.
                var name = Intent.GetStringExtra("name");
                var phoneNumber = Intent.GetStringExtra("phonenumber");
                var latitude = Intent.GetDoubleExtra("latitude", 0);
                var longitude = Intent.GetDoubleExtra("longitude", 0);
                var nameOrPhone = string.IsNullOrEmpty(name) ? phoneNumber : name;
                CrossExternalMaps.Current.NavigateTo(nameOrPhone, latitude, longitude, NavigationType.Default);
            }
        }
    }
}