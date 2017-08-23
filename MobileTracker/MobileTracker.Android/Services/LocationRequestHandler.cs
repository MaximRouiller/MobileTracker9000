using Android.OS;

namespace MobileTracker.Services
{
    public class LocationRequestHandler : Android.OS.Handler
    {
        // other code omitted for clarity

        public override void HandleMessage(Message msg)
        {
            int messageType = msg.What;

            switch (messageType)
            {
                default:
                    base.HandleMessage(msg);
                    break;
            }
        }
    }
}
