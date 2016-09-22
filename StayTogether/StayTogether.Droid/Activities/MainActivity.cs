using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using StayTogether.Classes;
using StayTogether.Droid.Services;
using StayTogether.Droid.Settings;

namespace StayTogether.Droid.Activities
{
	[Activity (Label = "StayTogether", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, AdapterView.IOnItemClickListener
	{
	    public LocationSenderBinder Binder;
	    public bool IsBound;
	    private CameraServiceConnection _cameraServiceConnection;
        List<ContactSynopsis> selectedContactSynopses = new List<ContactSynopsis>();
        private ListView _listView;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            _listView = FindViewById<ListView>(Resource.Id.List);
            var contactsHelper = new ContactsHelper();
            var contacts = await contactsHelper.GetContacts();
            var listAdapter = new ArrayAdapter<ContactSynopsis>(this, Android.Resource.Layout.SimpleListItemChecked, contacts);
            _listView.Adapter = listAdapter;
            _listView.ChoiceMode = ChoiceMode.Multiple;
            _listView.OnItemClickListener = this;

            StartService(new Intent(this, typeof(LocationSenderService)));

            Button startGroupButton = FindViewById<Button>(Resource.Id.myButton);

            startGroupButton.Click += delegate
            {
                if (selectedContactSynopses.Count > 0)
                {
                    LocationSenderService.Instance.StartGroup(selectedContactSynopses);
                }
            };
        }

        
        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            var checkedView = view as CheckedTextView;
            if (checkedView == null) return;

            var contact = _listView.GetItemAtPosition(position).Cast<ContactSynopsis>();
            if (checkedView.Checked)
            {
                selectedContactSynopses.Add(contact);
            }
            else
            {
                selectedContactSynopses.Remove(contact);
            }

        }


        protected override void OnPause()
        {
            base.OnPause();
            Binder?.GetLocationSenderService()?.StartForeground();
            UnbindFromService();

        }

        protected override void OnResume()
        {
            base.OnResume();
            BindToService();
        }

	    protected override void OnDestroy()
	    {
	        base.OnDestroy();
	        Binder?.GetLocationSenderService()?.StopSelf();
	    }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            menu.Clear();
            MenuInflater.Inflate(Resource.Menu.SettingsMenu, menu);

            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.settings:
                    LaunchMenu();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void LaunchMenu()
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(SettingsActivity));
            StartActivityForResult(intent, 0);
        }

        protected void BindToService()
        {
            _cameraServiceConnection = new CameraServiceConnection(this);
            BindService(new Intent(this, typeof(LocationSenderService)), _cameraServiceConnection, Bind.AutoCreate);
            IsBound = true;
        }

        protected void UnbindFromService()
        {
            if (IsBound)
            {
                UnbindService(_cameraServiceConnection);
                IsBound = false;
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
                _activity.Binder = locationSenderBinder;
                _activity.IsBound = true;
            }
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            _activity.IsBound = false;
        }
    }
}


