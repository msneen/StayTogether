namespace StayTogether.Classes
{
    public class ContactSynopsis
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        public override string ToString()
        {
            return $"{Name}-{PhoneNumber}";
        }
    }
}