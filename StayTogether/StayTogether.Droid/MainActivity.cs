using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using StayTogether.Classes;
using StayTogether.Droid.Settings;

namespace StayTogether.Droid
{
	[Activity (Label = "StayTogether", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, AdapterView.IOnItemClickListener
	{
		int count = 1;
	    public LocationSenderBinder binder;
	    public bool isBound;
	    private CameraServiceConnection _cameraServiceConnection;
        List<ContactSynopsis> selectedContactSynopses = new List<ContactSynopsis>();
        private ListView listView;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            listView = FindViewById<ListView>(Resource.Id.List);
            var contactsHelper = new ContactsHelper();
            var contacts = await contactsHelper.GetContacts();
            var listAdapter = new ArrayAdapter<ContactSynopsis>(this, Android.Resource.Layout.SimpleListItemChecked, contacts);
            listView.Adapter = listAdapter;
            listView.ChoiceMode = ChoiceMode.Multiple;
            listView.OnItemClickListener = this;

            this.StartService(new Intent(this, typeof(LocationSenderService)));

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += delegate
            {
                //button.Text = $"{contacts.Count} contacts";

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

            var contact = listView.GetItemAtPosition(position).Cast<ContactSynopsis>();
            if (checkedView.Checked)
            {
                //add
                selectedContactSynopses.Add(contact);
            }
            else
            {
                //try to remove
                selectedContactSynopses.Remove(contact);
            }

        }


        protected override void OnPause()
        {
            base.OnPause();
            binder?.GetLocationSenderService()?.StartForeground();
            UnbindFromService();

        }

        protected override void OnResume()
        {
            base.OnResume();
            BindToService();
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


