using System;
using System.Collections.Generic;
using StayTogether.Classes;
using StayTogether.iOS.Models;
using UIKit;

namespace StayTogether.iOS
{
	public partial class ViewController : UIViewController
	{
		int _count = 1;
	    private List<GroupMemberVm> _contacts;

	    public ViewController (IntPtr handle) : base (handle)
		{
		}

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            Button.AccessibilityIdentifier = "myButton";
            Button.TouchUpInside += delegate
            {
                var title = $"{_count++} clicks!";
                Button.SetTitle(title, UIControlState.Normal);
            };

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

