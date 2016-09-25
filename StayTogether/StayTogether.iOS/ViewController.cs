using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreLocation;
using StayTogether.Classes;
using StayTogether.iOS.Classes;
using StayTogether.iOS.Models;
using UIKit;

namespace StayTogether.iOS
{
	public partial class ViewController : UIViewController
	{
        // event for the location changing
        public event EventHandler<LocationUpdatedEventArgs> LocationUpdated = delegate { };

        int _count = 1;
	    private List<GroupMemberVm> _contacts;
	    private CLLocationManager _clLocationManager;

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
                //This is the button click event
                var title = $"{_count++} clicks!";
                Button.SetTitle(title, UIControlState.Normal);
            };

            await LoadContacts();
            InitializeLocationManager();
            StartLocationUpdates();
        }

        public void HandleLocationChanged(object sender, LocationUpdatedEventArgs e)
        {
            // Handle foreground updates
            CLLocation location = e.Location;

            //LblAltitude.Text = location.Altitude + " meters";
            //LblLongitude.Text = location.Coordinate.Longitude.ToString();
            //LblLatitude.Text = location.Coordinate.Latitude.ToString();
            //LblCourse.Text = location.Course.ToString();
            //LblSpeed.Text = location.Speed.ToString();

            Console.WriteLine("foreground updated");
        }

        private void StartLocationUpdates()
	    {
            if (CLLocationManager.LocationServicesEnabled)
            {
                //set the desired accuracy, in meters
                _clLocationManager.DesiredAccuracy = 1;
                _clLocationManager.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
                {
                    // fire our custom Location Updated event
                    LocationUpdated(this, new LocationUpdatedEventArgs(e.Locations[e.Locations.Length - 1]));
                };
                _clLocationManager.StartUpdatingLocation();
            }
        }

	    private void InitializeLocationManager()
	    {
	        _clLocationManager = new CLLocationManager();
            this.LocationUpdated += HandleLocationChanged;
            _clLocationManager.PausesLocationUpdatesAutomatically = false;
	        // iOS 8 has additional permissions requirements
	        if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
	        {
	            _clLocationManager.RequestAlwaysAuthorization(); // works in background
	            //locMgr.RequestWhenInUseAuthorization (); // only in foreground
	        }

	        if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
	        {
	            _clLocationManager.AllowsBackgroundLocationUpdates = true;
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

