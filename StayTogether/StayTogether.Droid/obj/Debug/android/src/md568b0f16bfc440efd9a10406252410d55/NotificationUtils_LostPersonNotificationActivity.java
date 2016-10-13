package md568b0f16bfc440efd9a10406252410d55;


public class NotificationUtils_LostPersonNotificationActivity
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
		mono.android.Runtime.register ("StayTogether.Droid.Helpers.NotificationUtils+LostPersonNotificationActivity, StayTogether.Droid, Version=1.0.6129.19972, Culture=neutral, PublicKeyToken=null", NotificationUtils_LostPersonNotificationActivity.class, __md_methods);
	}


	public NotificationUtils_LostPersonNotificationActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == NotificationUtils_LostPersonNotificationActivity.class)
			mono.android.TypeManager.Activate ("StayTogether.Droid.Helpers.NotificationUtils+LostPersonNotificationActivity, StayTogether.Droid, Version=1.0.6129.19972, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
