using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;
using System.Linq;

namespace MDMaps.MapElements
{
    class PinControler
    {
        public MainPin CreatePin(PinDestination destination, Position position)
        {
            MainPin pin = new MainPin()
            {
                Destination = destination,
                Label = destination == PinDestination.Start ? "Start" : "End",
                Type = PinType.SavedPin,
                Position = position
            };

            var pinExists = MainPage.map.Pins.FirstOrDefault(x => x.Label == pin.Label);
            
            if(pinExists != null)
            {
                MainPage.map.Pins.Remove(pinExists);
            }

            MainPage.map.Pins.Add(pin);

            pin.MarkerClicked += (s, args) =>
            {
                args.HideInfoWindow = true;
                MainPage.map.Pins.Remove(pin);
            };

            return pin;
        }
    }
}
