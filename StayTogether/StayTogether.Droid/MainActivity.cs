using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace StayTogether.Droid
{
	[Activity (Label = "StayTogether.Droid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 1;
	    public LocationSenderBinder binder;
	    public bool isBound;
	    private CameraServiceConnection _cameraServiceConnection;

	    protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

            this.StartService(new Intent(this, typeof(LocationSenderService)));

            // Get our button from the layout resource,
            // and attach an event to it
            //////Button button = FindViewById<Button> (Resource.Id.myButton);

            //////button.Click += delegate {
            //////	button.Text = string.Format ("{0} clicks!", count++);
            //////};
        }

        protected override void OnPause()
        {
            base.OnPause();

            binder.GetLocationSenderService().StartForeground();
            UnbindFromService();

        }

        protected override void OnResume()
        {
            base.OnResume();
            BindToService();
        }

        protected void BindToService()
        {
            _cameraServiceConnection = new CameraServiceConnection(this);
            BindService(new Intent(this, typeof(LocationSenderService)), _cameraServiceConnection, Bind.AutoCreate);
            isBound = true;
        }

        protected void UnbindFromService()
        {
            if (isBound)
            {
                UnbindService(_cameraServiceConnection);
                isBound = false;
            }
        }
    }

    public class CameraServiceConnection : Java.Lang.Object, IServiceConnection
    {
        MainActivity _activity;

        public CameraServiceConnection(MainActivity activity)
        {
            _activity = activity;
        }


        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            var locationSenderBinder = service as LocationSenderBinder;
            if (locationSenderBinder != null)
            {
                _activity.binder = locationSenderBinder;
                _activity.isBound = true;
            }
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            _activity.isBound = false;
        }
    }
}


