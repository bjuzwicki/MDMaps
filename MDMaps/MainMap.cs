using MDMaps.MapElements;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

namespace MDMaps
{   public class MainMap : Xamarin.Forms.GoogleMaps.Map
    {
        public MainMap() : base()
        {
        }

        public async Task<Position> GetMyLocation(bool onStart = false)
        {
            var locator = CrossGeolocator.Current;

            if (locator.IsGeolocationEnabled || !onStart)
            {
                 Location location = await Geolocation.GetLastKnownLocationAsync();
                 return new Position(location.Latitude, location.Longitude);
            }   
            
            return new Position(0,0);
        }
    }
}
