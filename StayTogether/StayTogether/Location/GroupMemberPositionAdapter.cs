using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Geolocator.Abstractions;
using StayTogether.Classes;

namespace StayTogether.Location
{
    public class GroupMemberPositionAdapter
    {
        public static GroupMemberVm Adapt(Position position)
        {
            return new GroupMemberVm
            {
                Latitude = position.Latitude,
                Longitude = position.Longitude
            };
        }
    }
}
