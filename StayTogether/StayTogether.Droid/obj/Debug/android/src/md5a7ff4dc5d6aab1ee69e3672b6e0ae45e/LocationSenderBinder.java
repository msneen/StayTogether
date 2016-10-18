package md5a7ff4dc5d6aab1ee69e3672b6e0ae45e;


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
		mono.android.Runtime.register ("StayTogether.Droid.Services.LocationSenderBinder, StayTogether.Droid, Version=1.0.6134.31039, Culture=neutral, PublicKeyToken=null", LocationSenderBinder.class, __md_methods);
	}


	public LocationSenderBinder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == LocationSenderBinder.class)
			mono.android.TypeManager.Activate ("StayTogether.Droid.Services.LocationSenderBinder, StayTogether.Droid, Version=1.0.6134.31039, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public LocationSenderBinder (md5a7ff4dc5d6aab1ee69e3672b6e0ae45e.LocationSenderService p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == LocationSenderBinder.class)
			mono.android.TypeManager.Activate ("StayTogether.Droid.Services.LocationSenderBinder, StayTogether.Droid, Version=1.0.6134.31039, Culture=neutral, PublicKeyToken=null", "StayTogether.Droid.Services.LocationSenderService, StayTogether.Droid, Version=1.0.6134.31039, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
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
