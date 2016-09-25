using System;
using CoreLocation;

namespace StayTogether.iOS.Classes
{
    public class LocationUpdatedEventArgs : EventArgs
    {
        public LocationUpdatedEventArgs(CLLocation location)
        {
            Location = location;
        }

        public CLLocation Location { get; }
    }
}