package md51aedf5c98984e30d08c3ffabbcc5d9d4;


public class SettingsFragment
	extends android.preference.PreferenceFragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("StayTogether.Droid.Resources.layout.SettingsFragment, StayTogether.Droid, Version=1.0.6125.35601, Culture=neutral, PublicKeyToken=null", SettingsFragment.class, __md_methods);
	}


	public SettingsFragment () throws java.lang.Throwable
	{
		super ();
		if (getClass () == SettingsFragment.class)
			mono.android.TypeManager.Activate ("StayTogether.Droid.Resources.layout.SettingsFragment, StayTogether.Droid, Version=1.0.6125.35601, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
