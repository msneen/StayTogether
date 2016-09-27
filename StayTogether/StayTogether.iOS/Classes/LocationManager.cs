using CoreLocation;
using UIKit;


namespace StayTogether.iOS.Classes
{
    public class LocationManager
    {

        CLLocationManager _clLocationManager;
        public string UserPhoneNumber { get; set; }

        public CLLocationManager ClLocationManager
        {
            get
            {
                return _clLocationManager;               
            }
            private set
            {
                _clLocationManager = value;
            }
        }

        public LocationManager()
        {
            _clLocationManager = new CLLocationManager();

            _clLocationManager.PausesLocationUpdatesAutomatically = false;
            // iOS 8 has additional permissions requirements
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                _clLocationManager.RequestAlwaysAuthorization(); // works in background
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                _clLocationManager.AllowsBackgroundLocationUpdates = true;
            }
        }

        public void StartLocationUpdates()
        {
            if (CLLocationManager.LocationServicesEnabled)
            {
                //set the desired accuracy, in meters
                _clLocationManager.DesiredAccuracy = 1;
                _clLocationManager.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>  
                {
                    // fire our custom Location Updated event
                    //LocationUpdated(this, new LocationUpdatedEventArgs(e.Locations[e.Locations.Length - 1]));
                    //Todo: We get a location here
                    var location = e.Locations[e.Locations.Length - 1];

                };

                _clLocationManager.StartUpdatingLocation();
            }
        }
    }
}