using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

namespace StayTogether.iOS.Classes
{
    public class MobileCenterManager
    {
        public static void RegisterMobileCenter()
        {
            MobileCenter.Start("8fb14343-2648-42ad-acdc-1acf2e6d8c0f",
                typeof(Analytics), typeof(Crashes));
        }
    }
}