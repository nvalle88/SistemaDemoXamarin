using Android.Gms.Common.Apis;
using Android.Gms.Location.Places;
using Android.Gms.Maps.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TK.CustomMap.Api;
using TK.CustomMap.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

[assembly: Dependency(typeof(NativePlacesApi))]

namespace TK.CustomMap.Droid
{
    /// <inheritdoc />
    public class NativePlacesApi : INativePlacesApi
    {
        GoogleApiClient apiClient;
        AutocompletePredictionBuffer buffer;

        ///<inheritdoc/>
        public async Task<IEnumerable<IPlaceResult>> GetPredictions(string query, MapSpan bounds)
        {
            if (apiClient == null || !apiClient.IsConnected) Connect();

            List<IPlaceResult> result = new List<IPlaceResult>();

            double mDistanceInMeters = bounds.Radius.Meters;

            double latRadian = bounds.LatitudeDegrees;

            double degLatKm = 110.574235;
            double degLongKm = 110.572833 * Math.Cos(latRadian);
            double deltaLat = mDistanceInMeters / 1000.0 / degLatKm;
            double deltaLong = mDistanceInMeters / 1000.0 / degLongKm;

            double minLat = bounds.Center.Latitude - deltaLat;
            double minLong = bounds.Center.Longitude - deltaLong;
            double maxLat = bounds.Center.Latitude + deltaLat;
            double maxLong = bounds.Center.Longitude + deltaLong;

            if (buffer != null)
            {
                buffer.Dispose();
                buffer = null;
            }

            buffer = await PlacesClass.GeoDataApi.GetAutocompletePredictionsAsync(
                apiClient,
                query,
                new LatLngBounds(new LatLng(minLat, minLong), new LatLng(maxLat, maxLong)),
                null);

            if (buffer != null)
            {
                result.AddRange(buffer.Select(i =>
                    new TKNativeAndroidPlaceResult
                    {
                        Description = i.GetPrimaryText(null),
                        Subtitle = i.GetSecondaryText(null),
                        PlaceId = i.PlaceId,
                    }));
            }

            return result;
        }
        ///<inheritdoc/>
        public void Connect()
        {
            if (apiClient == null)
            {
                apiClient = new GoogleApiClient.Builder(Forms.Context)
                    .AddApi(PlacesClass.GEO_DATA_API)
                    .Build();
            }

            if (!apiClient.IsConnected && !apiClient.IsConnecting)
            {
                apiClient.Connect();
            }
        }
        ///<inheritdoc/>
        public void DisconnectAndRelease()
        {
            if (apiClient == null) return;

            if (apiClient.IsConnected)
                apiClient.Disconnect();

            apiClient.Dispose();
            apiClient = null;

            if (buffer != null)
            {
                buffer.Dispose();
                buffer = null;
            }
        }
        /// <inheritdoc/>
        public async Task<TKPlaceDetails> GetDetails(string id)
        {
            if (apiClient == null || !apiClient.IsConnected) Connect();

            var nativeResult = await PlacesClass.GeoDataApi.GetPlaceByIdAsync(apiClient, id);

            if (nativeResult == null || !nativeResult.Any()) return null;

            var nativeDetails = nativeResult.First();

            return new TKPlaceDetails
            {
                Coordinate = nativeDetails.LatLng.ToPosition(),
                FormattedAddress = nativeDetails.AddressFormatted.ToString(),
                InternationalPhoneNumber = nativeDetails.PhoneNumberFormatted?.ToString(),
                Website = nativeDetails.WebsiteUri?.ToString()
            };
        }
    }
}