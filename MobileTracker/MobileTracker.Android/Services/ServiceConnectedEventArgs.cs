using System;
using Android.OS;

namespace MobileTracker.Services
{
    public partial class LocationServiceConnection
    {
        public class ServiceConnectedEventArgs : EventArgs
        {
            public IBinder Binder { get; set; }
        }
    }
}
