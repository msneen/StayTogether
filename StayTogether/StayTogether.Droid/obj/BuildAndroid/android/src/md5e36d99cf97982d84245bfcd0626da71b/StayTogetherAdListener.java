package md5e36d99cf97982d84245bfcd0626da71b;


public class StayTogetherAdListener
	extends com.google.android.gms.ads.AdListener
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onAdLoaded:()V:GetOnAdLoadedHandler\n" +
			"n_onAdClosed:()V:GetOnAdClosedHandler\n" +
			"";
		mono.android.Runtime.register ("StayTogether.Droid.Admob.StayTogetherAdListener, StayTogether.Droid, Version=1.0.6249.6509, Culture=neutral, PublicKeyToken=null", StayTogetherAdListener.class, __md_methods);
	}


	public StayTogetherAdListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == StayTogetherAdListener.class)
			mono.android.TypeManager.Activate ("StayTogether.Droid.Admob.StayTogetherAdListener, StayTogether.Droid, Version=1.0.6249.6509, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onAdLoaded ()
	{
		n_onAdLoaded ();
	}

	private native void n_onAdLoaded ();


	public void onAdClosed ()
	{
		n_onAdClosed ();
	}

	private native void n_onAdClosed ();

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