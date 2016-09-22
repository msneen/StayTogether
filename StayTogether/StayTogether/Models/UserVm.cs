using System.Collections.Generic;
using StayTogether.Classes;

namespace StayTogether
{
    public class UserVm
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    
        public List<ContactSynopsis> ContactList { get; set; }
    }
}
