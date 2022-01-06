using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Android.Widget;

namespace MDMaps.MapElements
{
    class PinControler
    {
        public Pin CreatePin(Position position)
        {
            Pin pin = new Pin()
            {
                Type = PinType.Place,
                Position = position,          
            };
            
            var existPin = MainPage.map.Pins.FirstOrDefault();

            string label = "1";

            if(existPin != null)
            {
                label = existPin.Label == "1" ? "2" : "1";
            }
            pin.Label = label;

            Color color = pin.Label == "1" ? Color.Green : Color.Red;
            pin.Icon = BitmapDescriptorFactory.DefaultMarker(color);

            MainPage.map.PinClicked += (s, args) =>
            {
                MainPage.map.Pins.Remove(args.Pin);
                Console.WriteLine(pin.Label);
                Console.WriteLine(args.Pin.Label);
            };

            MainPage.map.Pins.Add(pin);

            return pin;
        }
    }
}
