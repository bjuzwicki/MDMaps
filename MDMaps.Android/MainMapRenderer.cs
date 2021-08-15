using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MDMaps;
using MDMaps.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(MainMap), typeof(MainMapRenderer))]

namespace MDMaps.Droid
{
    public class MainMapRenderer : MapRenderer, IOnMapReadyCallback
    {
        public MainMapRenderer(Context context) : base(context)
        {
            
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);
            map.UiSettings.ZoomGesturesEnabled = true;
            map.UiSettings.RotateGesturesEnabled = true;
            map.UiSettings.ScrollGesturesEnabled = true;
            map.UiSettings.MyLocationButtonEnabled = false;
            map.UiSettings.CompassEnabled = true;
            map.UiSettings.ZoomControlsEnabled = false;
        }
    }
}