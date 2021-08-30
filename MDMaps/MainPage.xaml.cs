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
using MDMaps.MapElements;

namespace MDMaps
{
    public partial class MainPage : ContentPage
    {
        public static MainMap map = new MainMap()
        {
            IsShowingUser = true,
            MoveToLastRegionOnLayoutChange = true,  
        };

        NumberFormatInfo nfi = new NumberFormatInfo()
        {
            NumberDecimalSeparator = "."
        };
   
        MainImageButton addStartPinButton = new MainImageButton
        {
            WidthRequest = 45,
            HeightRequest = 45,
            Opacity = 1,
            BorderWidth = 5,
            BackgroundColor = Color.Transparent,
            Active = false,
            ActivateImage = ImageSource.FromResource("MDMaps.Graphics.active green pin.png"),
            DeactivateImage = ImageSource.FromResource("MDMaps.Graphics.green pin.png"),
            Source = ImageSource.FromResource("MDMaps.Graphics.green pin.png"),
        };
   
        MainImageButton addEndPinButton = new MainImageButton
        {
            WidthRequest = 45,
            HeightRequest = 45,
            Opacity = 1,
            BorderWidth = 5,
            BackgroundColor = Color.Transparent,
            Active = false,
            ActivateImage = ImageSource.FromResource("MDMaps.Graphics.active red pin.png"),
            DeactivateImage = ImageSource.FromResource("MDMaps.Graphics.red pin.png"),
            Source = ImageSource.FromResource("MDMaps.Graphics.red pin.png")
        };

        List<MapElement> tempMapElements = new List<MapElement>();

        public MainPage()
        {  
            InitializeComponent();

            Button createDirectionButton = new Button
            {
                Text = "TEST",
                WidthRequest = 45,
                HeightRequest = 45,
                //CornerRadius = 15,
                //BorderColor=Color.Transparent,
                //BackgroundColor=Color.Transparent,
                Opacity = 1,       
            };
            
            createDirectionButton.Clicked += CreateDirectionButtonClicked;
            addStartPinButton.Clicked += AddStartPinButtonClicked;
            addEndPinButton.Clicked += AddEndPinButtonClicked;
            map.MapClicked += Map_MapClicked;

            var relativeLayout = new RelativeLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,            
            };

            relativeLayout.Children.Add(map,
                Constraint.RelativeToParent(parent => parent.X),
                Constraint.RelativeToParent(parent => parent.Y),
                Constraint.RelativeToParent(parent => parent.Width),
                Constraint.RelativeToParent(parent => parent.Height)
            );

            relativeLayout.Children.Add(createDirectionButton,
                Constraint.RelativeToView(map, (parent, view) => view.Width - 54),
                Constraint.RelativeToView(map, (parent, view) => view.Y + 60)
            );

            relativeLayout.Children.Add(addStartPinButton,
                Constraint.RelativeToView(map, (parent, view) => view.Width - 54),
                Constraint.RelativeToView(map, (parent, view) => view.Y + 140)
            );

            relativeLayout.Children.Add(addEndPinButton,
                Constraint.RelativeToView(map, (parent, view) => view.Width - 54),
                Constraint.RelativeToView(map, (parent, view) => view.Y + 220)
            );

            Content = relativeLayout;

            
            map.MoveToRegion((MapSpan.FromCenterAndRadius(GetMyLocation().Result,Xamarin.Forms.Maps.Distance.FromMeters(1000))));
        }

        public async void CreateDirectionButtonClicked(object sender, EventArgs args)
        {     
            if(tempMapElements.Count > 0)
            {
                if(IsMyLocationInEndPoint())
                {
                    await DisplayAlert ("Alert", "LETS GO", "OK");
                }
                
                map.MapElements.Clear();
                tempMapElements.Clear();
            }
            else if(map.Pins.Count == 2)
            {
                DirectionAPI directionAPI = new DirectionAPI();

                Xamarin.Forms.Maps.Polyline polyline = new Xamarin.Forms.Maps.Polyline
                {
                    StrokeColor = new Color(255,0,0,0.5),
                    StrokeWidth = 8,     
                };

                List<Position> positions = await directionAPI.GetPolylinePoints($"https://maps.googleapis.com/maps/api/directions/json?origin={map.Pins[0].Position.Latitude.ToString(nfi)},{map.Pins[0].Position.Longitude.ToString(nfi)}&destination={map.Pins[1].Position.Latitude.ToString(nfi)},{map.Pins[1].Position.Longitude.ToString(nfi)}&mode=walking&language=en-EN&sensor=false&key=");
            
                foreach(var position in positions)
                {
                    polyline.Geopath.Add(position);    
                }

                Circle circle = new Circle
                {
                    Center = map.Pins[1].Position,
                    Radius = new Xamarin.Forms.Maps.Distance(20),
                    FillColor = new Color(255,0,0,0.5),
                    StrokeWidth = 10,
                    StrokeColor = Color.Red,
                };

                map.MapElements.Add(polyline); 

                map.MapElements.Add(circle);

                tempMapElements = new List<MapElement>(map.MapElements);
            }
        }

        public void AddStartPinButtonClicked(object sender, EventArgs args)
        {     
            if(addEndPinButton.Active)
            {
                addEndPinButton.SetActive(false);
            }  
            
            addStartPinButton.SetActive(!addStartPinButton.Active);
        }

        public void AddEndPinButtonClicked(object sender, EventArgs args)
        {    
            if(addStartPinButton.Active)
            {
                addStartPinButton.SetActive(false);
            }  
            
            addEndPinButton.SetActive(!addEndPinButton.Active);
        }

        private void Map_MapClicked(object sender, MapClickedEventArgs e)
        {   
            PinControler pinControler = new PinControler();  
            
            if(addStartPinButton.Active)
            {
                 pinControler.CreatePin(PinDestination.Start,e.Position);
            }
            else if(addEndPinButton.Active)
            {
                 pinControler.CreatePin(PinDestination.End,e.Position);
            }
        }

        private async Task<Position> GetMyLocation()
        {
            //var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
           // CancellationTokenSource cts = new CancellationTokenSource();
        //var location = await Geolocation.GetLocationAsync(request, cts.Token);
       // if (location != null)
       // {
            //await DisplayAlert ("Alert", "You have been alerted", "OK");
        //}
            var location = await Geolocation.GetLastKnownLocationAsync();
            //var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
           // var location = await Geolocation.GetLocationAsync(request);
            return new Position(location.Latitude, location.Longitude);
        }

        private bool IsMyLocationInEndPoint()
        {
            Circle circle = (Circle)map.MapElements.FirstOrDefault(x => x.GetType() == typeof(Circle));

            Position myPosition = GetMyLocation().Result;

            double distance = Location.CalculateDistance(new Location(myPosition.Latitude,myPosition.Longitude),new Location(circle.Center.Latitude,circle.Center.Longitude),DistanceUnits.Kilometers);

            return distance < circle.Radius.Kilometers;     

            //double latitude = Math.Pow(myPosition.Latitude-circle.Center.Latitude,2);

            //double longitude = Math.Pow(myPosition.Longitude-circle.Center.Longitude,2);

            //double radius = circle.Radius.Meters;

            //if(Math.Pow(myPosition.Latitude-circle.Center.Latitude,2) - Math.Pow(myPosition.Longitude-circle.Center.Longitude,2) < Math.Pow(circle.Radius.Meters,2))
            //{
            //    DisplayAlert ("Alert", "latitude = " + latitude + ", longitude = " + longitude + ", radius = " + circle.Radius.Meters, "OK");
            //    return true;
            //}

            //return false;
        }
    }
}
