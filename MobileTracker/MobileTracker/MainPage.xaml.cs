using Android.Content;
using Android.Locations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileTracker
{
    public partial class MainPage : ContentPage    {

        public MainPage()
        {
            InitializeComponent();

        }
        
        private void StopTracking(object sender, EventArgs e)
        {
            StartTrackingButton.IsEnabled = true;
            StopTrackingButton.IsEnabled = false;
            Droid.App.StopLocationService();
        }

        private void StartTracking(object sender, EventArgs e)
        {
            StartTrackingButton.IsEnabled = false;
            StopTrackingButton.IsEnabled = true;
            Droid.App.StartLocationService();
        }

    }
}
