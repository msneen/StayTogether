using Android.App;
using Android.OS;
using StayTogether.Droid.Resources.layout;

namespace StayTogether.Droid.Settings
{
    [Activity(Label = "Settings")]
    
    public class SettingsActivity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Settings);

            var settingsFragment  = new SettingsFragment();
          
            var fragmentTransaction = FragmentManager.BeginTransaction();
            fragmentTransaction.Add(Resource.Id.fragment_container, settingsFragment);
            fragmentTransaction.Commit();
        }
    }
}