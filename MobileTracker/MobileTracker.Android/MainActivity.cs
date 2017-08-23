using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using static MobileTracker.Services.LocationServiceConnection;
using Android.Util;
using Android.Locations;
using Android;

namespace MobileTracker.Droid
{
	[Activity (Label = "MobileTracker", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        readonly string logTag = "MainActivity";
        protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar; 

			base.OnCreate (bundle);


            App.Current.LocationServiceConnected += (object sender, ServiceConnectedEventArgs e) => {
                Log.Debug(logTag, "ServiceConnected Event Raised");
                // notifies us of location changes from the system
                App.Current.LocationService.LocationChanged += HandleLocationChanged;
                //notifies us of user changes to the location provider (ie the user disables or enables GPS)
                App.Current.LocationService.ProviderDisabled += HandleProviderDisabled;
                App.Current.LocationService.ProviderEnabled += HandleProviderEnabled;
                // notifies us of the changing status of a provider (ie GPS no longer available)
                App.Current.LocationService.StatusChanged += HandleStatusChanged;
            };

            if(Application.Context.CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
            {
                this.RequestPermissions(new[] { Manifest.Permission.AccessFineLocation }, 0);                
            }
            else
            {
                App.StartLocationService();
            }
            

            global::Xamarin.Forms.Forms.Init (this, bundle);
			LoadApplication (new MobileTracker.App ());
		}
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnDestroy()
        {
            Log.Debug(logTag, "OnDestroy: Location app is becoming inactive");
            base.OnDestroy();

            // Stop the location service:
            //App.StopLocationService();
        }
        public void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            Android.Locations.Location location = e.Location;
            Log.Debug(logTag, "Foreground updating");

            // these events are on a background thread, need to update on the UI thread
            RunOnUiThread(() => {
                //latText.Text = String.Format("Latitude: {0}", location.Latitude);
                //longText.Text = String.Format("Longitude: {0}", location.Longitude);
                //altText.Text = String.Format("Altitude: {0}", location.Altitude);
                //speedText.Text = String.Format("Speed: {0}", location.Speed);
                //accText.Text = String.Format("Accuracy: {0}", location.Accuracy);
                //bearText.Text = String.Format("Bearing: {0}", location.Bearing);
            });

        }

        public void HandleProviderDisabled(object sender, ProviderDisabledEventArgs e)
        {
            Log.Debug(logTag, "Location provider disabled event raised");
        }

        public void HandleProviderEnabled(object sender, ProviderEnabledEventArgs e)
        {
            Log.Debug(logTag, "Location provider enabled event raised");
        }

        public void HandleStatusChanged(object sender, StatusChangedEventArgs e)
        {
            Log.Debug(logTag, "Location status changed, event raised");
        }
    }
}

