using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using NLog;
using NLog.Config;
using NLog.Targets;
using StayTogether.Classes;
using StayTogether.Droid.Classes;
using StayTogether.Droid.Helpers;
using StayTogether.Droid.Services;
using StayTogether.Droid.Settings;

namespace StayTogether.Droid.Activities
{
	[Activity (Label = "StayTogether", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, AdapterView.IOnItemClickListener
	{
	    public LocationSenderBinder Binder;
	    public bool IsBound;
	    private LocationSenderServiceConnection _locationSenderServiceConnection;
	    readonly List<GroupMemberVm> _selectedContactSynopses = new List<GroupMemberVm>();
        private ListView _listView;
	    private Logger _logger;

	    protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            try
            {
                _logger = SetUpNLog();
                // Set our view from the "main" layout resource
                SetContentView(Resource.Layout.Main);

                await LoadContacts();

                StartService(new Intent(this, typeof(LocationSenderService)));

                Button startGroupButton = FindViewById<Button>(Resource.Id.myButton);

                startGroupButton.Click += delegate
                {
                    if (_selectedContactSynopses.Count > 0)
                    {
                        LocationSenderService.Instance.StartGroup(_selectedContactSynopses);
                    }
                };
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

	    private async Task LoadContacts()
	    {

	            await Task.Run(async () =>
	            {
                    try
                    {
                        Process.SetThreadPriority(ThreadPriority.Background);

                        var contactsHelper = new ContactsHelper();
                        var contacts = await contactsHelper.GetContacts();
                        if (contacts != null)
                        {
                            RunOnUiThread(() =>
                            {
                                _listView = FindViewById<ListView>(Resource.Id.List);
                                var listAdapter = new ArrayAdapter<GroupMemberVm>(this,
                                    Android.Resource.Layout.SimpleListItemChecked,
                                    contacts);
                                _listView.Adapter = listAdapter;
                                _listView.ChoiceMode = ChoiceMode.Multiple;
                                _listView.OnItemClickListener = this;
                            });
                        }
                        else
                        {
                            RunOnUiThread(() => Toast.MakeText(this, "Unable To load your contacts", ToastLength.Short).Show());
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError(ex);
                    }
                });
	    }




	    public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
	        try
	        {
	            var checkedView = view as CheckedTextView;
	            if (checkedView == null || _listView.Count < 1)
	            {
	                RunOnUiThread(() => Toast.MakeText(this, "Unable To load your contact", ToastLength.Short).Show());
	                return;
	            }

	            var contact = _listView.GetItemAtPosition(position).Cast<GroupMemberVm>();
	            if (checkedView.Checked)
	            {
	                _selectedContactSynopses.Add(contact);
	            }
	            else
	            {
	                _selectedContactSynopses.Remove(contact);
	            }
	        }
	        catch (Exception ex)
	        {
                LogError(ex);
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
            _locationSenderServiceConnection = new LocationSenderServiceConnection(this);
            BindService(new Intent(this, typeof(LocationSenderService)), _locationSenderServiceConnection, Bind.AutoCreate);
            IsBound = true;
        }

        protected void UnbindFromService()
        {
            if (IsBound)
            {
                UnbindService(_locationSenderServiceConnection);
                IsBound = false;
            }
        }
        public static Logger SetUpNLog()
        {
            var config = new LoggingConfiguration();

            var fileTarget = new FileTarget();
            config.AddTarget("StayTogetherLog", fileTarget);

            fileTarget.FileName = FileHelper.GetDocumentFileName();
            fileTarget.Layout = @"${date:format=HH\:mm\:ss} ${message}";

            var rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);

            LogManager.Configuration = config;

            var logger = LogManager.GetLogger("StayTogetherLog");

            logger.Log(LogLevel.Debug, "StayTogether MainActivity Starting");

            return logger;
        }

        private async void LogError(Exception ex)
        {
            _logger.Log(LogLevel.Debug, ex);

            await LocationSenderService.Instance.SendError(ex.Message + " " + ex.StackTrace);
        }

    }
}


