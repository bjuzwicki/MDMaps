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
        public static string json;
        public MainPage()
        {
            
            var location = Geolocation.GetLastKnownLocationAsync();
            //map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(locator.Result.Latitude, locator.Result.Longitude), Distance.FromMiles(1)));
            MainMap map = new MainMap(MapSpan.FromCenterAndRadius(new Position(location.Result.Latitude, location.Result.Longitude), Distance.FromMeters(100)))
            {
                IsShowingUser = true,
                MoveToLastRegionOnLayoutChange = true,  
                MapType = MapType.Hybrid, 
                HasZoomEnabled = false,
            };

            Position position1 = new Position(51.12583072392185, 16.969331794391568);
            Position position2 = new Position(51.130109572828744, 16.96647782321775);
            
            Distance distance = Distance.BetweenPositions(position1,position2);

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
           
            GetJSON("https://maps.googleapis.com/maps/api/directions/json?origin=51.12583072392185,16.969331794391568&destination=51.130109572828744,16.96647782321775&key=");


            Content = relativeLayout;
            InitializeComponent();
        }

        public async void GetJSON(string url)  
        {   
            if (NetworkCheck.IsInternet())  
            {  
                string key = Key.GetKey();
                var client = new System.Net.Http.HttpClient();  
                var response = await client.GetAsync(url + key);  
                json = await response.Content.ReadAsStringAsync(); 
            }  
        }  

        public void ButtonClicked(object sender, EventArgs args)
        {
            Console.WriteLine("json: " + json);
        }


        //public string GetKey()
        //{
        //    Assembly assem = this.GetType().Assembly;
        //    using (Stream stream = assem.GetManifestResourceStream("MDMaps.Network.key.txt"))
        //    {
        //        using (var reader = new StreamReader(stream))
        //        {
        //            return reader.ReadToEnd();
        //        }
        //    }
        //}
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
