using Android.OS;

namespace MobileTracker.Services
{
    public class LocationServiceBinder : Binder
    {
        public MobileTracker9000LocationService Service
        {
            get { return this.service; }
        }
        protected MobileTracker9000LocationService service;

        public bool IsBound { get; set; }

        // constructor
        public LocationServiceBinder(MobileTracker9000LocationService service)
        {
            this.service = service;
        }
    }
}
