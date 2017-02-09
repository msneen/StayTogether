package mono;

import java.io.*;
import java.lang.String;
import java.util.Locale;
import java.util.HashSet;
import java.util.zip.*;
import android.content.Context;
import android.content.Intent;
import android.content.pm.ApplicationInfo;
import android.content.res.AssetManager;
import android.util.Log;
import mono.android.Runtime;

public class MonoPackageManager {

	static Object lock = new Object ();
	static boolean initialized;

	static android.content.Context Context;

	public static void LoadApplication (Context context, ApplicationInfo runtimePackage, String[] apks)
	{
		synchronized (lock) {
			if (context instanceof android.app.Application) {
				Context = context;
			}
			if (!initialized) {
				android.content.IntentFilter timezoneChangedFilter  = new android.content.IntentFilter (
						android.content.Intent.ACTION_TIMEZONE_CHANGED
				);
				context.registerReceiver (new mono.android.app.NotifyTimeZoneChanges (), timezoneChangedFilter);
				
				System.loadLibrary("monodroid");
				Locale locale       = Locale.getDefault ();
				String language     = locale.getLanguage () + "-" + locale.getCountry ();
				String filesDir     = context.getFilesDir ().getAbsolutePath ();
				String cacheDir     = context.getCacheDir ().getAbsolutePath ();
				String dataDir      = getNativeLibraryPath (context);
				ClassLoader loader  = context.getClassLoader ();

				Runtime.init (
						language,
						apks,
						getNativeLibraryPath (runtimePackage),
						new String[]{
							filesDir,
							cacheDir,
							dataDir,
						},
						loader,
						new java.io.File (
							android.os.Environment.getExternalStorageDirectory (),
							"Android/data/" + context.getPackageName () + "/files/.__override__").getAbsolutePath (),
						MonoPackageManager_Resources.Assemblies,
						context.getPackageName ());
				
				mono.android.app.ApplicationRegistration.registerApplications ();
				
				initialized = true;
			}
		}
	}

	public static void setContext (Context context)
	{
		// Ignore; vestigial
	}

	static String getNativeLibraryPath (Context context)
	{
	    return getNativeLibraryPath (context.getApplicationInfo ());
	}

	static String getNativeLibraryPath (ApplicationInfo ainfo)
	{
		if (android.os.Build.VERSION.SDK_INT >= 9)
			return ainfo.nativeLibraryDir;
		return ainfo.dataDir + "/lib";
	}

	public static String[] getAssemblies ()
	{
		return MonoPackageManager_Resources.Assemblies;
	}

	public static String[] getDependencies ()
	{
		return MonoPackageManager_Resources.Dependencies;
	}

	public static String getApiPackageName ()
	{
		return MonoPackageManager_Resources.ApiPackageName;
	}
}

class MonoPackageManager_Resources {
	public static final String[] Assemblies = new String[]{
		/* We need to ensure that "StayTogether.Droid.dll" comes first in this list. */
		"StayTogether.Droid.dll",
		"Microsoft.AspNet.SignalR.Client.dll",
		"Microsoft.Azure.Mobile.Analytics.Android.Bindings.dll",
		"Microsoft.Azure.Mobile.Analytics.dll",
		"Microsoft.Azure.Mobile.Android.Bindings.dll",
		"Microsoft.Azure.Mobile.Crashes.Android.Bindings.dll",
		"Microsoft.Azure.Mobile.Crashes.dll",
		"Microsoft.Azure.Mobile.dll",
		"NLog.dll",
		"Plugin.Contacts.Abstractions.dll",
		"Plugin.Contacts.dll",
		"Plugin.CurrentActivity.dll",
		"Plugin.ExternalMaps.Abstractions.dll",
		"Plugin.ExternalMaps.dll",
		"Plugin.Geolocator.Abstractions.dll",
		"Plugin.Geolocator.dll",
		"Plugin.LocalNotifications.Abstractions.dll",
		"Plugin.LocalNotifications.dll",
		"Plugin.Permissions.Abstractions.dll",
		"Plugin.Permissions.dll",
		"Plugin.Settings.Abstractions.dll",
		"Plugin.Settings.dll",
		"Xamarin.Android.Support.v4.dll",
		"Xamarin.GooglePlayServices.Ads.dll",
		"Xamarin.GooglePlayServices.Basement.dll",
		"System.Threading.dll",
		"System.Runtime.dll",
		"System.Collections.dll",
		"System.Collections.Concurrent.dll",
		"System.Diagnostics.Debug.dll",
		"System.Reflection.dll",
		"System.Linq.dll",
		"System.Runtime.InteropServices.dll",
		"System.Runtime.Extensions.dll",
		"System.Reflection.Extensions.dll",
		"Newtonsoft.Json.dll",
		"System.Dynamic.Runtime.dll",
		"System.ObjectModel.dll",
		"System.Linq.Expressions.dll",
		"System.Text.Encoding.dll",
		"System.IO.dll",
		"System.Globalization.dll",
		"System.Text.RegularExpressions.dll",
		"System.Xml.ReaderWriter.dll",
		"System.Xml.XDocument.dll",
		"System.Threading.Tasks.dll",
		"System.Runtime.Serialization.Primitives.dll",
		"System.Text.Encoding.Extensions.dll",
		"System.Diagnostics.Tools.dll",
		"System.Net.Http.Extensions.dll",
		"System.Resources.ResourceManager.dll",
	};
	public static final String[] Dependencies = new String[]{
	};
	public static final String ApiPackageName = "Mono.Android.Platform.ApiLevel_19";
}
