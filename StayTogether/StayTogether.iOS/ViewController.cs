using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Settings;
using StayTogether.Classes;
using StayTogether.iOS.Classes;
using StayTogether.iOS.Models;
using UIKit;

namespace StayTogether.iOS
{
    //article on backgrounding location
    //https://developer.xamarin.com/guides/ios/application_fundamentals/backgrounding/part_4_ios_backgrounding_walkthroughs/location_walkthrough/
    public partial class ViewController : UIViewController
	{


        int _count = 1;
	    private List<GroupMemberVm> _contacts;
        public static LocationManager Manager;

        public ViewController (IntPtr handle) : base (handle)
        {
            Manager = new LocationManager();
        }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            StartGroup.AccessibilityIdentifier = "startGroup";
            StartGroup.TouchUpInside += delegate
            {
                //This is the button click event
                var title = $"{_count++} clicks!";
                StartGroup.SetTitle(title, UIControlState.Normal);
            };

            GetUserPhoneNumber();

            await LoadContacts();
        }

        private void GetUserPhoneNumber()
        {
            var userPhoneNumber = CrossSettings.Current.GetValueOrDefault<string>("phonenumber");
            if (userPhoneNumber.Length == 10)
            {
                UIPhoneNumberTextField.Hidden = true;
                Manager.UserPhoneNumber = userPhoneNumber;
                Manager.StartLocationUpdates();
            }
            else
            {
                UIPhoneNumberTextField.SizeToFit();
                UIPhoneNumberTextField.EditingDidEnd += (sender, args) =>
                {
                    var cleanPhoneNumber = ContactsHelper.CleanPhoneNumber(UIPhoneNumberTextField.Text);
                    if (cleanPhoneNumber.Length == 10)
                    {
                        InvokeOnMainThread(() =>
                        {
                            UIPhoneNumberTextField.BackgroundColor = UIColor.Yellow;
                            UIPhoneNumberTextField.Layer.BorderColor = UIColor.Red.CGColor;
                            UIPhoneNumberTextField.Layer.BorderWidth = 3;
                            UIPhoneNumberTextField.Layer.CornerRadius = 5;
                        });
                    }
                    else
                    {
                        CrossSettings.Current.AddOrUpdateValue<string>("phonenumber", cleanPhoneNumber);
                        Manager.UserPhoneNumber = cleanPhoneNumber;
                        InvokeOnMainThread(() => { UIPhoneNumberTextField.Hidden = true; });
                        Manager.StartLocationUpdates();
                    }
                };
            }
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

