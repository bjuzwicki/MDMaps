using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MDMaps;
using MDMaps.Droid;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MainMap), typeof(MainMapRenderer))]

namespace MDMaps.Droid
{
    public class MainMapRenderer : MapRenderer
    {
        public MainMapRenderer(Context context) : base(context)
        {
        }
    }
}