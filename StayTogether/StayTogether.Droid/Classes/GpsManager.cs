using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Locations;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Plugin.Geolocator.Abstractions;

namespace VideoForwarder
{
    [Service]
    public class GpsService
    {
        private static string _lastLocation;
        public static DateTime _lastLocationCheck;

        public static Position GetLocation()
        {
            try
            {
                Location location = null;

                if (LastLocationUpdateWasMoreThanAnHourAgo())
                {
                    var criteriaForGpsService = new Criteria
                    {
                        //A constant indicating an approximate accuracy  
                        Accuracy = Accuracy.Coarse,
                        PowerRequirement = Power.Low
                    };
                    var locationManager =
                        (LocationManager) Android.App.Application.Context.GetSystemService(Context.LocationService);
                        //context.GetSystemService(
                    var locationProvider = locationManager.GetBestProvider(criteriaForGpsService, true);

                    if (locationManager.IsProviderEnabled(locationProvider))
                    {
                        location = locationManager.GetLastKnownLocation(locationProvider);
                        var position = new Position();
                        position.Longitude = location.Longitude;
                        position.Latitude = location.Latitude;
                        return position;
                    }
                    else
                    {
                        try
                        {
                            TurnGPSOn();
                        }
                        catch (Exception ex)
                        {
                            Log.WriteLine(LogPriority.Info, "GetLocation", "GPS:  Unable to turn on.  " + ex.StackTrace);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Log.WriteLine(LogPriority.Info, "GetLocation", ex.StackTrace);
            
            }
            return null;

        }

        private static bool LastLocationUpdateWasMoreThanAnHourAgo()
        {
            try
            {
                var interval = DateTime.Now - _lastLocationCheck;
                var isMoreThanAnHour = interval > TimeSpan.FromHours(1);
                return isMoreThanAnHour;
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogPriority.Info, "LastLocationUpdateWasMoreThanAnHourAgo", ex.StackTrace);
            }
            return true;
        }

        public static void TurnGPSOn()
        {
            try
            {
                Intent intent = new Intent("android.location.GPS_ENABLED_CHANGE");
                intent.PutExtra("enabled", true);
                Application.Context.SendBroadcast(intent);

                String provider = Android.Provider.Settings.Secure.GetString(Application.Context.ContentResolver, Android.Provider.Settings.Secure.LocationProvidersAllowed);
                if (!provider.Contains("gps"))
                { //if gps is disabled
                    ToggleGps();
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogPriority.Info, "TurnGPSOn", ex.StackTrace);
            }
        }
        // automatic turn off the gps
        public static void TurnGPSOff()
        {
            try
            {
                String provider = Android.Provider.Settings.Secure.GetString(Application.Context.ContentResolver, Android.Provider.Settings.Secure.LocationProvidersAllowed);
                if (provider.Contains("gps"))
                { //if gps is enabled
                    ToggleGps();
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogPriority.Info, "TurnGPSOff", ex.StackTrace);
            }
        }

        private static void ToggleGps()
        {
            try
            {
                Intent poke = new Intent();
                poke.SetClassName("com.android.settings", "com.android.settings.widget.SettingsAppWidgetProvider");
                poke.AddCategory(Intent.CategoryAlternative);
                poke.SetData(Android.Net.Uri.Parse("3"));
                Application.Context.SendBroadcast(poke);
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogPriority.Info, "ToggleGps", ex.StackTrace);
            }

        }
    }
}
