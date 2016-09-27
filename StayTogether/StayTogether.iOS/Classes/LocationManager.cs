using System.Collections.Generic;
using CoreLocation;
using Plugin.Geolocator.Abstractions;
using StayTogether.Classes;
using StayTogether.Group;
using UIKit;


namespace StayTogether.iOS.Classes
{
    public class LocationManager
    {

        CLLocationManager _clLocationManager;
        private CLLocation _lastLocation;
        private LocationSender _locationSender;
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
            _clLocationManager = new CLLocationManager
            {
                PausesLocationUpdatesAutomatically = false
            };

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

        private void InitializeLocationSender()
        {
            _locationSender = new LocationSender(UserPhoneNumber);
            _locationSender.InitializeSignalRAsync();
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
                    _lastLocation = e.Locations[e.Locations.Length - 1];
                    //Todo: if more than 2 minutes or 20 meters from last location, send update to server
                    
                };

                _clLocationManager.StartUpdatingLocation();

                InitializeLocationSender();
            }
        }

        public async void StartGroup(List<GroupMemberVm> selectedContacts)
        {
            if (_lastLocation != null)
            {
                var position = new Position
                {
                    Latitude = _lastLocation.Coordinate.Latitude,
                    Longitude = _lastLocation.Coordinate.Longitude
                };

                var groupVm = GroupHelper.InitializeGroupVm(selectedContacts, position, UserPhoneNumber);

                await _locationSender.StartGroup(groupVm);
            }
        }
    }
}