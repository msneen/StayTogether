package md529fe1e97be2e30d18b46d16226688d6a;


public class LocationSenderBinder
	extends android.os.Binder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("StayTogether.Droid.Services.LocationSenderBinder, StayTogether.Droid, Version=1.0.6126.21558, Culture=neutral, PublicKeyToken=null", LocationSenderBinder.class, __md_methods);
	}


	public LocationSenderBinder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == LocationSenderBinder.class)
			mono.android.TypeManager.Activate ("StayTogether.Droid.Services.LocationSenderBinder, StayTogether.Droid, Version=1.0.6126.21558, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public LocationSenderBinder (md529fe1e97be2e30d18b46d16226688d6a.LocationSenderService p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == LocationSenderBinder.class)
			mono.android.TypeManager.Activate ("StayTogether.Droid.Services.LocationSenderBinder, StayTogether.Droid, Version=1.0.6126.21558, Culture=neutral, PublicKeyToken=null", "StayTogether.Droid.Services.LocationSenderService, StayTogether.Droid, Version=1.0.6126.21558, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
