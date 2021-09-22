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
   
        MainImageButton createRouteButton = new MainImageButton
        {
            CornerRadius = 30,
            BackgroundColor = Color.LightGray,
            Source = ImageSource.FromResource("MDMaps.Graphics.route4.png"),
            Opacity = 0.7,       
            Padding = 10,
            BorderColor = Color.LightGray,
            BorderWidth = 2,
            IsEnabled = false,
        };

        MainImageButton moveToMyLocationButton = new MainImageButton
        {
            CornerRadius = 40,
            BackgroundColor = Color.WhiteSmoke,          
            Source = ImageSource.FromResource("MDMaps.Graphics.mylocation.png"),
            Opacity = 1,       
            IsVisible = true,
            Padding = 10,
            BorderColor = Color.LightGray,
            BorderWidth = 2,
        };

        MainImageButton addPinsButton = new MainImageButton
        {
            CornerRadius = 30,
            Opacity = 1,
            BorderWidth = 2,
            BorderColor = Color.LightGray,
            BackgroundColor = Color.WhiteSmoke,
            Active = false,
            Padding = 10,
            Source = ImageSource.FromResource("MDMaps.Graphics.pins.png"),
        };

        List<MapElement> tempMapElements = new List<MapElement>();

        public bool routDrawn = false;

        public MainPage()
        {  
            InitializeComponent();

            createRouteButton.Clicked += CreateRouteButtonClicked;
            addPinsButton.Clicked += AddPinsButtonClicked;
            moveToMyLocationButton.Clicked += MoveToMyLocationButtonClicked;
            map.MapClicked += Map_MapClicked;

            var relativeLayout = new RelativeLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand, 
            };

            var relativeLayout2 = new RelativeLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            relativeLayout.Children.Add(relativeLayout2,
                Constraint.RelativeToView(map, (parent, view) => parent.X + createRouteButton.Width/20),
                Constraint.RelativeToView(map, (parent, view) => parent.Y + createRouteButton.Height/10),
                Constraint.RelativeToView(map, (parent, view) => parent.Width - createRouteButton.Width/10),
                Constraint.RelativeToView(map, (parent, view) => parent.Height/1.3)
            );

            relativeLayout.Children.Add(map,
                Constraint.RelativeToParent(parent => parent.X),
                Constraint.RelativeToParent(parent => parent.Y),
                Constraint.RelativeToParent(parent => parent.Width),
                Constraint.RelativeToParent(parent => parent.Height)
            );

            relativeLayout.Children.Add(createRouteButton,
                Constraint.RelativeToView(map, (parent, view) => 0 + createRouteButton.Width/20),
                Constraint.RelativeToView(map, (parent, view) => parent.Height - createRouteButton.Height - createRouteButton.Height/10),
                Constraint.RelativeToView(map, (parent, view) => parent.Width - createRouteButton.Width/10),
                Constraint.RelativeToView(map, (parent, view) => parent.Height/10)
            );

             relativeLayout.Children.Add(addPinsButton,
                Constraint.RelativeToView(createRouteButton, (parent, view) => view.X),
                Constraint.RelativeToView(createRouteButton, (parent, view) => view.Y - view.Height - addPinsButton.Height/10),
                Constraint.RelativeToView(createRouteButton, (parent, view) => view.Width),
                Constraint.RelativeToView(createRouteButton, (parent, view) => view.Height)
            );

            relativeLayout.Children.Add(moveToMyLocationButton,
                Constraint.RelativeToView(relativeLayout2, (parent, view) => view.Width - moveToMyLocationButton.Width/2),
                Constraint.RelativeToView(relativeLayout2, (parent, view) => view.Height - moveToMyLocationButton.Height),
                Constraint.RelativeToView(relativeLayout2, (parent, view) => parent.Height/20),
                Constraint.RelativeToView(relativeLayout2, (parent, view) => parent.Height/20)
            );

            Content = relativeLayout;
            
            try
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(GetMyLocation().Result,Xamarin.Forms.Maps.Distance.FromMeters(1000)));
            }
            catch
            {
                DisplayAlert("","Turn Location Services and GPS on!", "Ok");
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(51.759445,19.457216),Xamarin.Forms.Maps.Distance.FromKilometers(1000)));
            }            
        }

        public async void CreateRouteButtonClicked(object sender, EventArgs args)
        {     
            if(map.Pins.Count == 2 && !routDrawn)
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

                routDrawn = true;

                addPinsButton.SetEnable(false);

                createRouteButton.SetEnable(false);

                tempMapElements = new List<MapElement>(map.MapElements);

                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    if(tempMapElements.Count == 0)
                    {
                        return false;
                    }

                    if(IsMyLocationInEndPoint())
                    {
                        DisplayAlert ("You did it!", "End point has been reached.", "Ok");
                        routDrawn = false;
                        map.MapElements.Clear();
                        tempMapElements.Clear();
                        map.Pins.Clear();
                        addPinsButton.SetEnable(true);
                        return false;
                    }
                    
                    return true;
                });
            }
        }

        public void AddPinsButtonClicked(object sender, EventArgs args)
        {     
            addPinsButton.SetActive(!addPinsButton.Active);
        }

        public void MoveToMyLocationButtonClicked(object sender, EventArgs args)
        {     
            map.MoveToRegion(MapSpan.FromCenterAndRadius(GetMyLocation().Result,Xamarin.Forms.Maps.Distance.FromMeters(1000)));
        }

        private void Map_MapClicked(object sender, MapClickedEventArgs e)
        {
            if (addPinsButton.Active && map.Pins.Count < 2)
            {   
                PinControler pinControler = new PinControler();
                MainPin pin = pinControler.CreatePin(e.Position);   

                pin.MarkerClicked += (s, args) =>
                {
                    createRouteButton.SetEnable(false);
                    if(routDrawn)
                    {
                        map.MapElements.Clear();
                        tempMapElements.Clear();
                        routDrawn = false;
                    }
                    addPinsButton.SetEnable(true);
                };

                if(routDrawn)
                {
                    map.MapElements.Clear();
                    tempMapElements.Clear();
                    routDrawn = false;
                }

                createRouteButton.SetEnable(map.Pins.Count == 2);
                addPinsButton.SetEnable(map.Pins.Count != 2);
            }
        }

        private async Task<Position> GetMyLocation()
        {
            Location location = await Geolocation.GetLastKnownLocationAsync();

            return new Position(location.Latitude, location.Longitude);
        }

        private bool IsMyLocationInEndPoint()
        {
            Circle circle = (Circle)map.MapElements.FirstOrDefault(x => x.GetType() == typeof(Circle));

            Position myPosition = new Position();

            try
            {
                myPosition = GetMyLocation().Result;
            }
            catch
            {
                return false;
            }

            double distance = Location.CalculateDistance(new Location(myPosition.Latitude,myPosition.Longitude),new Location(circle.Center.Latitude,circle.Center.Longitude),DistanceUnits.Kilometers);

            return distance < circle.Radius.Kilometers;
        }
    }
}
