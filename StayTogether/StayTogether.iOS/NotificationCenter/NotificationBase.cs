using Foundation;

namespace StayTogether.iOS.NotificationCenter
{
    public class NotificationBase
    {
        protected static void AddValue(string keyName, string propertyValue, ref NSMutableDictionary dictionary)
        {
            var key = new NSString(keyName);
            var value = new NSString(propertyValue);
            dictionary.Add(key, value);
        }
    }
}
