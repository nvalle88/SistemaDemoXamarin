using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace TK.CustomMap.Overlays
{
    public class TKPolyline : TKOverlay
    {
        public const string LineCoordinatesPropertyName = "LineCoordinates";
        public const string LineWidthProperty = "LineWidth";

        List<Position> lineCoordinates;
        float lineWidth;

        /// <summary>
        /// Coordinates of the line
        /// </summary>
        public List<Position> LineCoordinates
        {
            get { return lineCoordinates; }
            set { this.SetField(ref lineCoordinates, value); }
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
        /// Creates a new instance of <see cref="TKPolyline"/>
        /// </summary>
        public TKPolyline()
        {
            lineCoordinates = new List<Position>();
        }
    }
}
