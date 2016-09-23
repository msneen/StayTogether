namespace StayTogether.Classes
{
    public class GroupMemberVm
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsAdmin { get; set; }
        public string GroupId { get; set; }
        public string ConnectionId { get; set; }

        public override string ToString()
        {
            return $"{Name}-{PhoneNumber}";
        }
    }
}