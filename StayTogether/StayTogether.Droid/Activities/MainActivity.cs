using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;
using Android.Views;
using Android.Widget;
using NLog;
using NLog.Config;
using NLog.Targets;
using Plugin.ExternalMaps;
using Plugin.ExternalMaps.Abstractions;
using StayTogether.Classes;
using StayTogether.Droid.Admob;
using StayTogether.Droid.Classes;
using StayTogether.Droid.Helpers;
using StayTogether.Droid.NotificationCenter;
using StayTogether.Droid.Services;
using StayTogether.Droid.Settings;

namespace StayTogether.Droid.Activities
{
	[Activity (Label = "StayTogether", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, Icon = "@drawable/icon")]
	public class MainActivity : Activity, AdapterView.IOnItemClickListener, GroupJoinedCallback
	{
	    public LocationSenderBinder Binder;
	    public bool IsBound;
	    private LocationSenderServiceConnection _locationSenderServiceConnection;
	    readonly List<GroupMemberVm> _selectedContactSynopses = new List<GroupMemberVm>();
        private ListView _listView;
	    private Logger _logger;
	    private IMenuItem _leaveGroupMenuItem;

	    public void GroupJoined()
        {
            DisableStartGroupButton("Group Joined");
            HideContactList();
        }

        public void GroupDisbanded()
        {
            //Todo:  kill the app for now.  Later we'll just show the start button and contact list.
            Finish();//Todo: Eventually keep running and reshow the Start Group Button and Contacts List
        }

	    protected override void OnNewIntent(Intent intent)
	    {
	        base.OnNewIntent(intent);
            NotificationStrategyController.GetNotificationHandler(intent)?.OnNotify(intent);
        }

	    protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            try
            {
                _logger = SetUpNLog();
                // Set our view from the "main" layout resource
                SetContentView(Resource.Layout.Main);

                NotificationStrategyController.GetNotificationHandler(Intent)?.OnNotify(Intent);

                await LoadContacts();

                StartService(new Intent(this, typeof(LocationSenderService)));

                InitializeStartButton();

                ShowAd();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

	    private void InitializeStartButton()
	    {
	        Button startGroupButton = FindViewById<Button>(Resource.Id.myButton);

	        startGroupButton.Click += delegate
	        {
	            if (_selectedContactSynopses.Count > 0)
	            {
	                LocationSenderService.Instance.StartGroup(_selectedContactSynopses);
	                DisableStartGroupButton();
	                HideContactList();
                    _leaveGroupMenuItem.SetVisible(true);
                }
	        };
	    }

	    private void DisableStartGroupButton(string buttonText = "Restart Group")
	    {
	        RunOnUiThread(() =>
	        {
	            Button startGroupButton = FindViewById<Button>(Resource.Id.myButton);
	            startGroupButton.Text = buttonText;
	            startGroupButton.Enabled = false;
	        });
	    }

	    private void HideContactList()
	    {
            RunOnUiThread(() => {
	            var contactList = FindViewById<ListView>(Resource.Id.List);
	            contactList.Visibility = ViewStates.Invisible;
            });
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
            var inAGroup = false;
            BindToService();
            var locationSenderService = Binder?.GetLocationSenderService();
            if (locationSenderService != null)
            {
                var locationSender = locationSenderService.LocationSender;
                if (locationSender != null)
                {
                    inAGroup = locationSender.InAGroup;
                }
            }

            if (inAGroup == true)
            {
                GroupJoined();
            }
        }

	    protected override void OnDestroy()
	    {
	        base.OnDestroy();
            Binder?.GetLocationSenderService()?.SetGroupJoinedCallback(null);
            Binder?.GetLocationSenderService()?.StopSelf();
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            System.Environment.Exit(0);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            menu.Clear();
            MenuInflater.Inflate(Resource.Menu.SettingsMenu, menu);

            SetEndGroupMenuItemVisibility(menu);
            SetLeaveGroupMenuItemVisibility(menu);

            return base.OnPrepareOptionsMenu(menu);
        }

	    private void SetEndGroupMenuItemVisibility(IMenu menu)
	    {
	        _leaveGroupMenuItem = menu.FindItem(Resource.Id.endGroup);
	        var inAGroup = LocationSenderService.Instance?.LocationSender?.InAGroup ?? false;
	        var isGroupLeader = LocationSenderService.Instance?.LocationSender?.GroupLeader ?? false;
	        var visible = inAGroup && isGroupLeader;
	        _leaveGroupMenuItem.SetVisible(visible);
	    }

        private void SetLeaveGroupMenuItemVisibility(IMenu menu)
        {
            _leaveGroupMenuItem = menu.FindItem(Resource.Id.leaveGroup);
            var inAGroup = LocationSenderService.Instance?.LocationSender?.InAGroup ?? false;
            var isGroupLeader = LocationSenderService.Instance?.LocationSender?.GroupLeader ?? true;//if null, default to true since that keeps the menu item Invisible
            var visible = inAGroup && !isGroupLeader;
            _leaveGroupMenuItem.SetVisible(visible);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.settings:
                    LaunchMenu();
                    break;
                case Resource.Id.endGroup:
                    EndGroup();
                    break;
                case Resource.Id.leaveGroup:
                    LeaveGroup();
                    break;
                case Resource.Id.exit:
                    Finish();                   
                    break;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

	    private void LeaveGroup()
	    {
            LocationSenderService.Instance.LeaveGroup();
            Finish();//Todo: Eventually keep running and reshow the Start Group Button and Contacts List
        }

	    private void EndGroup()
	    {
	        LocationSenderService.Instance.EndGroup();
            Finish();//Todo: Eventually keep running and reshow the Start Group Button and Contacts List
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

        public void ShowAd()
        {
            MobileAds.Initialize(this);
            var adView = (AdView)FindViewById<AdView>(Resource.Id.adView);

            AdRequest adRequest = new AdRequest.Builder()
                .AddTestDevice(AdRequest.DeviceIdEmulator)
                .Build();
            adView.LoadAd(adRequest);
            adView.AdListener = new StayTogetherAdListener();
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


