package md5ca64b6aaf4711af49b2edab62eb49cb3;


public class SettingsActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("StayTogether.Droid.Settings.SettingsActivity, StayTogether.Droid, Version=1.0.6129.20547, Culture=neutral, PublicKeyToken=null", SettingsActivity.class, __md_methods);
	}


	public SettingsActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == SettingsActivity.class)
			mono.android.TypeManager.Activate ("StayTogether.Droid.Settings.SettingsActivity, StayTogether.Droid, Version=1.0.6129.20547, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
