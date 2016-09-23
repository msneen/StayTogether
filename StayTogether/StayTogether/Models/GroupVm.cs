using System;
using System.Collections.Generic;
using StayTogether.Classes;

namespace StayTogether
{
    public class GroupVm
    {
        public string PhoneNumber { get; set; }
        public int MaximumDistance { get; set; }
        public DateTime GroupCreatedDateTime { get; set; }
        public DateTime GroupDisbandDateTime { get; set; }
        public DateTime LastContactDateTime { get; set; }

        public List<GroupMemberVm> GroupMembers { get; set; }
    }
}
