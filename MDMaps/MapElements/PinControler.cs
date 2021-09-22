using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;
using System.Linq;

namespace MDMaps.MapElements
{
    class PinControler
    {
        public MainPin CreatePin(Position position)
        {
            MainPin pin = new MainPin()
            {
                Type = PinType.Place,
                Position = position,
                Label = "1",
            };
            
            var existPin = (MainPin)MainPage.map.Pins.FirstOrDefault();

            pin.Destination = existPin == null ? PinDestination.Start : PinDestination.End; 

            pin.MarkerClicked += (s, args) =>
            {
                args.HideInfoWindow = true;

                MainPage.map.Pins.Remove(pin);

                var existPin2 = (MainPin)MainPage.map.Pins.FirstOrDefault();
                
                if(existPin2 != null)
                {
                   var updatePin =  (MainPin)MainPage.map.Pins[0];
                   updatePin.Destination = PinDestination.Start;
                }
            };

            MainPage.map.Pins.Add(pin);

            return pin;
        }
    }
}
