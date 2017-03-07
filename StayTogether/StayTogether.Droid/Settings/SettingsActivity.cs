using System;
using Android.App;
using Android.OS;
using Android.Views;
using StayTogether.Droid.Resources.layout;
using StayTogether.Droid.Swipe;

namespace StayTogether.Droid.Settings
{
    [Activity(Label = "Settings")]
    
    public class SettingsActivity : Activity
    {
        private SwipeHandler _swipeHandler;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //_swipeHandler = new SwipeHandler(this);
            //_swipeHandler.OnSwipeLeft += _swipeHandler_OnSwipeLeft;
            //_swipeHandler.OnSwipeRight += _swipeHandler_OnSwipeRight;

            SetContentView(Resource.Layout.Settings);

            var settingsFragment  = new SettingsFragment();
            
            var fragmentTransaction = FragmentManager.BeginTransaction();
            fragmentTransaction.Add(Resource.Id.fragment_container, settingsFragment);
            fragmentTransaction.Commit();


        }


        //private void _swipeHandler_OnSwipeRight(object sender, EventArgs e)
        //{
        //    FinishActivity(100);
        //}

        //private void _swipeHandler_OnSwipeLeft(object sender, EventArgs e)
        //{
        //    FinishActivity(100);
        //}
    }
}