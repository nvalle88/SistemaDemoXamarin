using Xamarin.Forms.Maps;

namespace TK.CustomMap.Overlays
{
    /// <summary>
    /// A route to display on the map
    /// </summary>
    public class TKRoute : TKOverlay, IRouteFunctions
    {
        public const string SourceProperty = "Source";
        public const string DestinationProperty = "Destination";
        public const string LineWidthProperty = "LineWidth";
        public const string SelectableProperty = "Selectable";
        public const string TravelModelProperty = "TravelMode";
        public const string BoundsProperty = "Bounds";
        public const string StepsProperty = "Steps";
        public const string DistanceProperty = "Distance";
        public const string TravelTimeProperty = "TravelTime";
        public const string IsCalculatedProperty = "IsCalculated";

        Position source;
        Position destination;
        float lineWidth;
        bool selectAble;
        TKRouteTravelMode travelMode;
        MapSpan bounds;
        TKRouteStep[] steps;
        double distance;
        double travelTime;
        bool isCalculated;
        /// <summary>
        /// Gets/Sets the source of the route
        /// </summary>
        public Position Source
        {
            get { return source; }
            set { this.SetField(ref source, value); }
        }
        /// <summary>
        /// Gets/Sets the destination of the route
        /// </summary>
        public Position Destination
        {
            get { return destination; }
            set { this.SetField(ref destination, value); }
        }
        /// <summary>
        /// Gets/Sets the width of the line
        /// </summary>
        public float LineWidth
        {
            get { return lineWidth; }
            set { this.SetField(ref lineWidth, value); }
        }
        /// <summary>
        /// Gets/Sets if a route is selectable. If this is <value>false</value> <see cref="TK.CustomMap.TKCustomMap.RouteClickedCommand"/> will not get raised for this route
        /// </summary>
        public bool Selectable
        {
            get { return selectAble; }
            set { this.SetField(ref selectAble, value); }
        }
        /// <summary>
        /// Gets/Sets the travel mode for a route calculation.
        /// </summary>
        public TKRouteTravelMode TravelMode
        {
            get { return travelMode; }
            set { this.SetField(ref travelMode, value); }
        }
        /// <summary>
        /// Gets the bounds of the route. This is set automatically by the renderer during route calculation.
        /// </summary>
        public MapSpan Bounds
        {
            get { return bounds; }
            private set { this.SetField(ref bounds, value); }
        }
        /// <summary>
        /// Gets the steps of the route
        /// </summary>
        public TKRouteStep[] Steps
        {
            get { return steps; }
            private set { this.SetField(ref steps, value); }
        }
        /// <summary>
        /// Gets the distance of the route in meters
        /// </summary>
        public double Distance
        {
            get { return distance; }
            private set { this.SetField(ref distance, value); }
        }
        /// <summary>
        /// Gets the travel time of the route in seconds
        /// </summary>
        public double TravelTime
        {
            get { return travelTime; }
            private set { this.SetField(ref travelTime, value); }
        }
        public bool IsCalculated
        {
            get { return isCalculated; }
            private set { this.SetField(ref isCalculated, value); }
        }
        /// <summary>
        /// Creates a new instance of <see cref="TKRoute"/>
        /// </summary>
        public TKRoute()
        {
            LineWidth = 2.5f;
            Selectable = true;
            TravelMode = TKRouteTravelMode.Driving;
        }
        ///<inheritdoc/>
        void IRouteFunctions.SetBounds(MapSpan bounds) => Bounds = bounds;

        ///<inheritdoc/>
        void IRouteFunctions.SetSteps(TKRouteStep[] steps) => Steps = steps;

        ///<inheritdoc/>
        void IRouteFunctions.SetTravelTime(double travelTime) => TravelTime = travelTime;

        ///<inheritdoc/>
        void IRouteFunctions.SetDistance(double distance) => Distance = distance;

        ///<inheritdoc/>
        void IRouteFunctions.SetIsCalculated(bool calculated) => IsCalculated = calculated;
    }
}