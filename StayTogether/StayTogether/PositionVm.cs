using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Geolocator.Abstractions;

namespace StayTogether
{
    public class PositionVm
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public Position Position { get; set; }
    }
}
