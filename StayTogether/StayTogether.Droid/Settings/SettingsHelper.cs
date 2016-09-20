using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace VideoForwarder.Settings
{
    public class SettingsHelper
    {
        // Function called from OnDestroy
        public static void Saveset(string name, string value)
        {
            //store
            var prefs = Application.Context.GetSharedPreferences("VideoForwarder", FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.PutString(name, value);
            prefEditor.Commit();

        }

        // Function called from OnCreate
        public static string Retrieveset(string name)
        {
            //retreive 
            var prefs = Application.Context.GetSharedPreferences("VideoForwarder", FileCreationMode.Private);
            
            var pref = prefs.GetString(name, null);

            return pref;

        }
    }
}