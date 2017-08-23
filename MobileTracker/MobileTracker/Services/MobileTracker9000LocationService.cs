using Android.App;
using System;
using System.Collections.Generic;
using System.Text;
using Android.Content;
using Android.OS;
using Android.Locations;
using Android.Runtime;
using Android.Util;
using System.Net.Http;
using Newtonsoft.Json;

namespace MobileTracker.Services
{
    //[Service(Name = "com.MobileTracker.MobileTracker9000LocationService", Process = ":mobiletracker9000_process", Exported = true)]
    [Service]
    public class MobileTracker9000LocationService : Service, ILocationListener
    {
        public event EventHandler<LocationChangedEventArgs> LocationChanged = delegate { };
        public event EventHandler<ProviderDisabledEventArgs> ProviderDisabled = delegate { };
        public event EventHandler<ProviderEnabledEventArgs> ProviderEnabled = delegate { };
        public event EventHandler<StatusChangedEventArgs> StatusChanged = delegate { };

        protected LocationManager LocMgr = Application.Context.GetSystemService("location") as LocationManager;
        readonly string logTag = "LocationService";
        IBinder binder;

        public override IBinder OnBind(Intent intent)
        {
            binder = new LocationServiceBinder(this);
            return binder;
        }
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            return StartCommandResult.Sticky;
        }


        public void StartLocationUpdates()
        {
            //we can set different location criteria based on requirements for our app -
            //for example, we might want to preserve power, or get extreme accuracy
            var locationCriteria = new Criteria();

            locationCriteria.Accuracy = Accuracy.Fine;
            locationCriteria.PowerRequirement = Power.NoRequirement;

            // get provider: GPS, Network, etc.
            var locationProvider = LocMgr.GetBestProvider(locationCriteria, true);

            // Get an initial fix on location
            LocMgr.RequestLocationUpdates(locationProvider, 2000, 0, this);

        }
        public override void OnDestroy()
        {
            base.OnDestroy();

            // Stop getting updates from the location manager:
            LocMgr.RemoveUpdates(this);
        }

        public void OnLocationChanged(Location location)
        {
            this.LocationChanged(this, new LocationChangedEventArgs(location));
            var newLocation = new
            {
                location.Latitude,
                location.Longitude,
                location.Altitude,
                location.Speed,
                location.Accuracy,
                location.Bearing,
                UserKey = "testing",
                RecordDate = DateTime.UtcNow
            };
            try
            {
                HttpClient client = new HttpClient { BaseAddress = new Uri("https://cdamobiletracker.azurewebsites.net/") };


                var content = new StringContent(JsonConvert.SerializeObject(newLocation), Encoding.UTF8, "application/json");
                var result = client.PostAsync("api/StoreCoordinateToStorage", content).Result;
            }
            finally
            {
                // catch: do nothing.
            }

            Log.Debug(logTag, String.Format("Latitude is {0}", newLocation.Latitude));
            Log.Debug(logTag, String.Format("Longitude is {0}", newLocation.Longitude));
            Log.Debug(logTag, String.Format("Altitude is {0}", newLocation.Altitude));
            Log.Debug(logTag, String.Format("Speed is {0}", newLocation.Speed));
            Log.Debug(logTag, String.Format("Accuracy is {0}", newLocation.Accuracy));
            Log.Debug(logTag, String.Format("Bearing is {0}", newLocation.Bearing));
        }

        public void OnProviderDisabled(string provider)
        {
            this.ProviderDisabled(this, new ProviderDisabledEventArgs(provider));
        }

        public void OnProviderEnabled(string provider)
        {
            this.ProviderEnabled(this, new ProviderEnabledEventArgs(provider));
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            this.StatusChanged(this, new StatusChangedEventArgs(provider, status, extras));
        }
    }
}
