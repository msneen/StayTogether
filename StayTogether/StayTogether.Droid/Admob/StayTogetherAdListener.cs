using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace StayTogether.Droid.Admob
{
    public class StayTogetherAdListener:AdListener
    {
        public override void OnAdLoaded()
        {
            base.OnAdLoaded();
        }
        public override void OnAdClosed()
        {
            base.OnAdClosed();
        }
    }
}