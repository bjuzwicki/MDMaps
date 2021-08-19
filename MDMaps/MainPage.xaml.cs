using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Essentials;
using System.Threading;
using MDMaps.Network;
using System.Resources;
using System.IO;
using System.Reflection;
using System.Globalization;

namespace MDMaps
{
    public partial class MainPage : ContentPage
    {
        MainMap map = new MainMap()
        {
            IsShowingUser = true,
            MoveToLastRegionOnLayoutChange = true,  
        };

        Pin pinStart = new Pin()
        {
            Label = "Start",
            Type = PinType.SavedPin,
        };
        Pin pinEnd = new Pin()
        {
            Label = "End",
            Type = PinType.SavedPin,
        };

        public MainPage()
        {  
            var location = Geolocation.GetLastKnownLocationAsync();
            map.MoveToRegion((MapSpan.FromCenterAndRadius(new Position(location.Result.Latitude, location.Result.Longitude), Xamarin.Forms.Maps.Distance.FromMeters(100))));
            
            var button = new Button
            {
                Text = "TEST",
                WidthRequest = 45,
                HeightRequest = 45,
                //CornerRadius = 15,
                //BorderColor=Color.Transparent,
                //BackgroundColor=Color.Transparent,
                Opacity = 1,
            };

            pinStart.Position = new Position(51.12583072392185,16.969331794391568);

            pinEnd.Position = new Position(51.09766773030684,17.048971956165826);

            button.Clicked += ButtonClicked;

            var relativeLayout = new RelativeLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            relativeLayout.Children.Add(map,
                Constraint.RelativeToParent(parent => parent.X),
                Constraint.RelativeToParent(parent => parent.Y),
                Constraint.RelativeToParent(parent => parent.Width),
                Constraint.RelativeToParent(parent => parent.Height)
            );

            relativeLayout.Children.Add(button,
                Constraint.RelativeToView(map, (parent, view) => view.Width - 54),
                Constraint.RelativeToView(map, (parent, view) => view.Y + 60)
            );
           
            map.Pins.Add(pinStart);
            map.Pins.Add(pinEnd);
            Content = relativeLayout;
            InitializeComponent();
        }

        public async void ButtonClicked(object sender, EventArgs args)
        {     
            DirectionAPI directionAPI = new DirectionAPI();

            Xamarin.Forms.Maps.Polyline polyline = new Xamarin.Forms.Maps.Polyline
            {
                StrokeColor = Color.Red,
                StrokeWidth = 40
            };

            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";

            List<Position> positions = await directionAPI.GetPolylinePoints($"https://maps.googleapis.com/maps/api/directions/json?origin={pinStart.Position.Latitude.ToString(nfi)},{pinStart.Position.Longitude.ToString(nfi)}&destination={pinEnd.Position.Latitude.ToString(nfi)},{pinEnd.Position.Longitude.ToString(nfi)}&key=");
            
            foreach(var position in positions)
            {
                polyline.Geopath.Add(position);    
            }

            map.MapElements.Add(polyline);

            //map.MoveToRegion(Camera)

            Console.WriteLine("chyba koniec");           
        }
    }
}
