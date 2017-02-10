using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Plugin.Settings;
using StayTogether.Classes;
using StayTogether.iOS.Classes;
using StayTogether.iOS.Models;
using StayTogether.iOS.NotificationCenter;
using UIKit;

namespace StayTogether.iOS
{
    //article on backgrounding location
    //https://developer.xamarin.com/guides/ios/application_fundamentals/backgrounding/part_4_ios_backgrounding_walkthroughs/location_walkthrough/
    public partial class ViewController : UIViewController
	{


        int _count = 1;
	    private List<GroupMemberVm> _contacts;
        private bool _eventsInitialized;
        public static LocationManager Manager;

        public ViewController (IntPtr handle) : base (handle)
        {
            Manager = new LocationManager();
        }

        private void InitializeEvents(LocationManager manager)
        {
#if (DEBUG)
//            //Testing only
//            LostNotification.DisplayLostNotification(new GroupMemberVm {PhoneNumber = "6199224340"});
//            GroupInvitationNotification.DisplayGroupInvitationNotification("6199224340", "");
//            LeftGroupNotification.DisplayGroupInvitationNotification("6199224340", "");
//              InAnotherGroupNotification.DisplayInAnotherGroupNotification("6199224340", "");
#endif
            if (manager?.LocationSender == null || _eventsInitialized) return;

            manager.LocationSender.OnSomeoneIsLost += (sender, args) =>
            {
                LostNotification.DisplayLostNotification(args.GroupMember);
            };

            manager.LocationSender.OnGroupInvitationReceived += (sender, args) =>
            {
                GroupInvitationNotification.DisplayGroupInvitationNotification(args.GroupId, args.Name);
            };
            manager.LocationSender.OnGroupJoined += (sender, args) =>
            {

            };
            manager.LocationSender.OnSomeoneLeft += (sender, args) =>
            {
                LeftGroupNotification.DisplayGroupInvitationNotification(args.PhoneNumber, args.Name);
            };
            manager.LocationSender.OnSomeoneAlreadyInAnotherGroup += (sender, args) =>
            {
                InAnotherGroupNotification.DisplayInAnotherGroupNotification(args.PhoneNumber, args.Name);
            };
            manager.LocationSender.OnGroupDisbanded += (sender, args) =>
            {

            };
            _eventsInitialized = true;
        }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            StartGroup.AccessibilityIdentifier = "startGroup";
            StartGroup.TouchUpInside += delegate
            {              
                //This is the button click event
                var title = $"Update Group";
                StartGroup.SetTitle(title, UIControlState.Normal);

                StartGroup1(); //Change name of button, then change method name to "StartGroup"
            };

            GetUserPhoneNumber();
            GetUserName();

            Manager.StartLocationUpdates();
            InitializeEvents(Manager);

            await LoadContacts();
        }

        private void StartGroup1()
        {
            var selectedContacts = _contacts.Where(x => x.Selected).ToList();


            if (selectedContacts.Any() && GetUserPhoneNumber() && TryGetUserName()) //Todo: get nickname
            {

                Manager.StartGroup(selectedContacts);
                InitializeEvents(Manager);
            }
        }

        private bool GetUserPhoneNumber()
        {
            var userPhoneNumber = CrossSettings.Current.GetValueOrDefault<string>("phonenumber");
            if  (userPhoneNumber != null && userPhoneNumber.Length == 10)
            {
                //UIPhoneNumberTextField.Hidden = true;
                UIPhoneNumberTextField.Text = userPhoneNumber;
                UIPhoneNumberTextField.Enabled = true;
                Manager.UserPhoneNumber = userPhoneNumber;
                return true;
            }
            else
            {
                UIPhoneNumberTextField.SizeToFit();
                TryGetUserPhoneNumber();
                UIPhoneNumberTextField.EditingDidEnd += (sender, args) =>
                {
                    TryGetUserPhoneNumber();
                };
                UIPhoneNumberTextField.ValueChanged += (object sender,EventArgs e) =>
                {
                    if (UIPhoneNumberTextField.Text.Length == 10)
                    {
                        TryGetUserPhoneNumber();
                    }
                };
                
            }
            return false;
        }

        private bool TryGetUserPhoneNumber()
        {
            var cleanPhoneNumber = ContactsHelper.CleanPhoneNumber(UIPhoneNumberTextField.Text);
            if (cleanPhoneNumber.Length != 10)
            {
                InvokeOnMainThread(() =>
                {
                    UIPhoneNumberTextField.Hidden = false;
                    UIPhoneNumberTextField.BackgroundColor = UIColor.Yellow;
                    UIPhoneNumberTextField.Layer.BorderColor = UIColor.Red.CGColor;
                    UIPhoneNumberTextField.Layer.BorderWidth = 3;
                    UIPhoneNumberTextField.Layer.CornerRadius = 5;
                });
                return false;
            }
            else
            {
                CrossSettings.Current.AddOrUpdateValue<string>("phonenumber", cleanPhoneNumber);
                Manager.UserPhoneNumber = cleanPhoneNumber;
                InvokeOnMainThread(() => { UIPhoneNumberTextField.Hidden = true; });
            }
            return true;
        }

        private void GetUserName()
        {
            var userName = CrossSettings.Current.GetValueOrDefault<string>("name");
            if (!string.IsNullOrWhiteSpace( userName ))
            {
                UINameTextField.Text = userName;
                UINameTextField.Enabled = true;
                return;
            }
            else
            {
                UINameTextField.SizeToFit();
                TryGetUserName();
                UINameTextField.EditingDidEnd += (sender, args) =>
                {
                    TryGetUserName();
                };
                UINameTextField.ValueChanged += (object sender, EventArgs e) =>
                {
                    if (string.IsNullOrWhiteSpace( UINameTextField.Text))
                    {
                        TryGetUserName();
                    }
                };

            }

            return;
        }

        private bool TryGetUserName()
        {
            var userName = UINameTextField.Text;
            if (string.IsNullOrWhiteSpace( userName))
            {
                InvokeOnMainThread(() =>
                {
                    UINameTextField.Enabled = true;
                    //UINameTextField.BackgroundColor = UIColor.Yellow;
                    //UINameTextField.Layer.BorderColor = UIColor.Red.CGColor;
                    //UINameTextField.Layer.BorderWidth = 3;
                    //UINameTextField.Layer.CornerRadius = 5;
                });
                return false;
            }
            else
            {
                CrossSettings.Current.AddOrUpdateValue<string>("name", userName);

                InvokeOnMainThread(() => { UINameTextField.Enabled = true; });
            }
            return true;
        }

        private async Task LoadContacts()
	    {
            var contactsHelper = new ContactsHelper();
            _contacts = await contactsHelper.GetContacts();
            if (_contacts == null) return;

            ContactsUITableView.Source = new UITableViewContactsViewSource(_contacts);
            ContactsUITableView.AllowsMultipleSelection = true;
            ContactsUITableView.AllowsMultipleSelectionDuringEditing = true;
            ContactsUITableView.SizeToFit();
            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
                ContactsUITableView.CellLayoutMarginsFollowReadableWidth = false;
        }

	    public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

    }
}

