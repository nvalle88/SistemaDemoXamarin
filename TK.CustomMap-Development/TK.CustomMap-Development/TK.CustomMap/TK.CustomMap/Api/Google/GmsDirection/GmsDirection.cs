using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace TK.CustomMap.Api.Google
{
    /// <summary>
    /// Calls the Google Maps Directions API to get a route
    /// </summary>
    public class GmsDirection
    {
        static string apiKey;
        static GmsDirection instance;

        readonly HttpClient httpClient;
        const string BASE_URL = "https://maps.googleapis.com/maps/api/directions/";
        /// <summary>
        /// The <see cref="GmsDirection"/> instance
        /// </summary>
        public static GmsDirection Instance => instance ?? (instance = new GmsDirection());
        /// <summary>
        /// Creates a new instance of <see cref="GmsDirection"/>
        /// </summary>
        private GmsDirection()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(BASE_URL);
        }
        /// <summary>
        /// Set the API key 
        /// </summary>
        /// <param name="apiKey">Google Maps API key</param>
        public static void Init(string apiKey) => GmsDirection.apiKey = apiKey;

        /// <summary>
        /// Calculates a route
        /// </summary>
        /// <param name="origin">The origin</param>
        /// <param name="destination">The destination</param>
        /// <param name="mode">The travelling mode</param>
        /// <param name="language">The language</param>
        /// <returns>A <see cref="GmsDirectionResult"/></returns>
        public async Task<GmsDirectionResult> CalculateRoute(Position origin, Position destination, GmsDirectionTravelMode mode, string language = null)
        {
            var response = await httpClient.GetAsync(BuildQueryString(origin, destination, mode, language));

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<GmsDirectionResult>(await response.Content.ReadAsStringAsync());
            }

            return null;
        }
        /// <summary>
        /// Builds the query string for the Google Maps Directions API call
        /// </summary>
        /// <param name="origin">The origin</param>
        /// <param name="destination">The destination</param>
        /// <param name="mode">The travelling mode</param>
        /// <param name="language">The language</param>
        /// <returns>The query string</returns>
        string BuildQueryString(Position origin, Position destination, GmsDirectionTravelMode mode, string language)
        {
            StringBuilder strBuilder = new StringBuilder(
                string.Format(
                    "json?origin={0}&destination={1}&mode={2}",
                    origin.AsString(),
                    destination.AsString(),
                    mode.ToString().ToLower()));

            if (!string.IsNullOrWhiteSpace(language))
            {
                strBuilder.AppendFormat("&language={0}", language);
            }

            strBuilder.AppendFormat("&key={0}", apiKey);
            return strBuilder.ToString();
        }
    }
}