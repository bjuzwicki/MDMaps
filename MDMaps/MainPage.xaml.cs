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

namespace MDMaps
{
    public partial class MainPage : ContentPage
    {
        MainMap map = new MainMap()
        {
            IsShowingUser = true,
            MoveToLastRegionOnLayoutChange = true,  
        };

        public MainPage()
        {  
            var location = Geolocation.GetLastKnownLocationAsync();
            map.MoveToRegion((MapSpan.FromCenterAndRadius(new Position(location.Result.Latitude, location.Result.Longitude), Xamarin.Forms.Maps.Distance.FromMeters(100))));

            //Position position1 = new Position(51.12583072392185, 16.969331794391568);
            //Position position2 = new Position(51.130109572828744, 16.96647782321775);
            
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
           
            Content = relativeLayout;
            InitializeComponent();
        }

        //public async void GetJSON(string url)  
        //{   
        //    if (NetworkCheck.IsInternet())  
        //    {  
        //        string key = Key.GetKey();
        //        var client = new System.Net.Http.HttpClient();  
        //        var response = await client.GetAsync(url + key);  
        //        json = await response.Content.ReadAsStringAsync(); 
        //    }  
        //}  

        public async void ButtonClicked(object sender, EventArgs args)
        {     
            DirectionAPI directionAPI = new DirectionAPI();

            Xamarin.Forms.Maps.Polyline polyline = new Xamarin.Forms.Maps.Polyline
            {
                StrokeColor = Color.Red,
                StrokeWidth = 40
            };

            List<Position> positions = await directionAPI.GetPolylinePoints("https://maps.googleapis.com/maps/api/directions/json?origin=51.12583072392185,16.969331794391568&destination=51.09766773030684,17.048971956165826&key=");
            
            foreach(var position in positions)
            {
                polyline.Geopath.Add(position);    
            }

            map.MapElements.Add(polyline);

            //map.MoveToRegion(Camera)

            Console.WriteLine("chyba koniec");           
        }

        //CancellationTokenSource cts;

        //async Task<Location> GetCurrentLocation()
        //{
        //    Console.WriteLine("jestem tu");
        //    try
        //    {
        //        var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
        //        Console.WriteLine("jestem tu3");
        //        cts = new CancellationTokenSource();
        //        var location = await Geolocation.GetLocationAsync(request, cts.Token);
        //        Console.WriteLine("jestem tu2");
        //        if (location != null)
        //        {
        //            Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
        //        }

        //        return location;
        //    }
        //    catch (FeatureNotSupportedException fnsEx)
        //    {
        //        Console.WriteLine(fnsEx);
        //    }
        //    catch (FeatureNotEnabledException fneEx)
        //    {
        //        Console.WriteLine(fneEx);
        //    }
        //    catch (PermissionException pEx)
        //    {
        //        Console.WriteLine(pEx);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //    }

        //    return null;
        //}

        //protected override void OnDisappearing()
        //{
        //    if (cts != null && !cts.IsCancellationRequested)
        //        cts.Cancel();
        //    base.OnDisappearing();
        //}
    }
}
