using System.Runtime.Remoting.Channels;
using Android.OS;
using Android.Preferences;
using Java.Lang;
using Plugin.Settings;
using StayTogether.Droid.Settings;

namespace StayTogether.Droid.Resources.layout
{
    public class SettingsFragment : PreferenceFragment
    {
        

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            AddPreferencesFromResource(Resource.Xml.Preferences);

            Preference preference = FindPreference("nickname");
            preference.PreferenceChange += (sender, e) =>
            {
                var nickname = e.NewValue.ToString();
                CrossSettings.Current.AddOrUpdateValue("nickname", nickname);
            };

        }

       

 
    }
}