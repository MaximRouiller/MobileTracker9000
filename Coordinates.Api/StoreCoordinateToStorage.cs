using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;

namespace MyAwesomeApp
{
    public static class StoreCoordinateToStorage
    {
        [FunctionName("StoreCoordinateToStorage")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")]HttpRequestMessage req, [Table("GPSCoordinate", Connection = "StorageConnectionString")]ICollector<TraceCoordinate> outTable, TraceWriter log)
        {
            dynamic data = await req.Content.ReadAsAsync<object>();
            double longitude = data?.Longitude;
            double latitude = data?.Latitude;
            DateTime recordDate = data?.RecordDate;
            string userKey = data?.UserKey;

            if (string.IsNullOrWhiteSpace(userKey))
            {
                throw new ArgumentNullException(nameof(userKey), "Missing user key");
            }

            outTable.Add(new TraceCoordinate()
            {
                PartitionKey = userKey,
                RowKey = Guid.NewGuid().ToString(),
                Longitude = longitude,
                Latitude = latitude,
                Altitude = data?.Altitude,
                Speed = data?.Speed,
                Accuracy = data?.Accuracy,
                RecordDateAsUtc = recordDate
            });
            return req.CreateResponse(HttpStatusCode.Created);
        }

        public class TraceCoordinate : TableEntity
        {
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public double Altitude { get; set; }
            public double Speed { get; set; }
            public double Accuracy { get; set; }
            public DateTime RecordDateAsUtc { get; set; }
        }
    }
}
