using Xamarin.Forms;

namespace TK.CustomMap.Api
{
    /// <summary>
    /// Manages instance of <see cref="INativePlacesApi"/>
    /// </summary>
    public static class TKNativePlacesApi
    {
        static INativePlacesApi instance;

        /// <summary>
        /// Gets an instance of <see cref="INativePlacesApi"/>
        /// </summary>
        public static INativePlacesApi Instance
        {
            get
            {
                return instance ?? (instance = DependencyService.Get<INativePlacesApi>());
            }
        }
    }
}
