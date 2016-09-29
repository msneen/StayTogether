using System;
using System.Collections.Generic;
using CoreLocation;
using Plugin.Geolocator.Abstractions;
using StayTogether.Classes;
using StayTogether.Group;
using StayTogether.Location;
using UIKit;


namespace StayTogether.iOS.Classes
{
    public class LocationManager
    {

        CLLocationManager _clLocationManager;
        private CLLocation _lastLocation;
        private LocationSender _locationSender;
        private SendMeter _sendMeter;
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
            _sendMeter = new SendMeter(100, TimeSpan.FromMinutes(2));
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
                    _lastLocation = e.Locations[e.Locations.Length - 1];
                    //if more than 2 minutes or 100 feet from last location, send update to server
                    SendPositionUpdate();
                };

                _clLocationManager.StartUpdatingLocation();

                if (UserPhoneNumber.Length > 10)
                {
                    InitializeLocationSender();
                }
            }
        }

        private void SendPositionUpdate()
        {
            var position = new Position
            {
                Latitude = _lastLocation.Coordinate.Latitude,
                Longitude = _lastLocation.Coordinate.Longitude
            };
            if (_sendMeter.CanSend(position))
            {
                //Send position update
                var groupMemberVm = new GroupMemberVm
                {
                    //Todo:  Get Group Member Properties
                    Name = "iPhoneTester",
                    PhoneNumber = UserPhoneNumber,
                    Latitude = _lastLocation.Coordinate.Latitude,
                    Longitude = _lastLocation.Coordinate.Longitude
                };
                 _locationSender.SendUpdatePosition(groupMemberVm);
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

                if (_locationSender == null)
                {
                    InitializeLocationSender();
                }
                await _locationSender.StartGroup(groupVm);
            }

        }
    }
}