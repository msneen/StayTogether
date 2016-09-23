using System;
using System.Collections.Generic;
using StayTogether.Classes;

namespace StayTogether
{
    public class GroupVm
    {
        public DateTime GroupCreatedDateTime { get; set; }
        public DateTime GroupDisbandDateTime { get; set; }

        public List<GroupMemberVm> ContactList { get; set; }
    }
}
