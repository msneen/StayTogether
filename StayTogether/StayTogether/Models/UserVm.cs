using System;
using System.Collections.Generic;
using System.Text;
using StayTogether.Classes;

namespace StayTogether
{
    public class UserVm
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public List<ContactSynopsis> ContactList { get; set; }
    }
}
