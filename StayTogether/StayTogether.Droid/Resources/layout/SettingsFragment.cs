using System.Security;
using Android.OS;
using Android.Preferences;
using Plugin.Settings;

namespace StayTogether.Droid.Resources.layout
{
    public class SettingsFragment : PreferenceFragment
    {

        [SecurityCritical]
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