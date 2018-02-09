using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TK.CustomMap.Api.OSM
{
    /// <summary>
    /// Handles calls the the OSM Nominatim API
    /// </summary>
    public class OsmNominatim
    {
        const string BASE_URL = "http://nominatim.openstreetmap.org/search/";

        static OsmNominatim instance;
        readonly HttpClient httpClient;
        /// <summary>
        /// Gets the API Instance
        /// </summary>
        public static OsmNominatim Instance => instance ?? (instance = new OsmNominatim());
        /// <summary>
        /// Gets/Sets the limit of predictions to receive
        /// </summary>
        public int Limit { get; set; }
        /// <summary>
        /// Gets/Sets country codes
        /// </summary>
        public Collection<string> CountryCodes { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="OsmNominatim"/>
        /// </summary>
        private OsmNominatim()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(BASE_URL);

            Limit = 5;
            CountryCodes = new Collection<string>();
        }
        /// <summary>
        /// Calls the OSM Niminatim API to get predictions
        /// </summary>
        /// <param name="searchTerm">Term to search for</param>
        /// <returns>Predictions</returns>
        public async Task<IEnumerable<OsmNominatimResult>> GetPredictions(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return null;

            var result = await httpClient.GetAsync(BuildQueryString(searchTerm));

            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<IEnumerable<OsmNominatimResult>>(await result.Content.ReadAsStringAsync());
            }

            return null;
        }
        /// <summary>
        /// Build the API query string
        /// </summary>
        /// <param name="searchTerm">Term to search for</param>
        /// <returns>Query string</returns>
        string BuildQueryString(string searchTerm)
        {
            StringBuilder str = new StringBuilder();

            if (CountryCodes.Any())
            {
                str.AppendFormat("{0}/", string.Join(",", CountryCodes));
            }

            str.AppendFormat("{0}?", searchTerm);

            if (Limit > 0)
            {
                str.AppendFormat("{0}={1}&", "limit", Limit);
            }

            str.AppendFormat("{0}={1}", "format", "json");

            return str.ToString();
        }
    }
}