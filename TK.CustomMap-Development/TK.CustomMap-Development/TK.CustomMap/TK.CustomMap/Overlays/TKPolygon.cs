using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TK.CustomMap.Overlays
{
    /// <summary>
    /// A polygon to display on the map
    /// </summary>
    public class TKPolygon : TKOverlay
    {
        public const string CoordinatesPropertyName = "Coordinates";
        public const string StrokeColorPropertyName = "StrokeColor";
        public const string StrokeWidthPropertyName = "StrokeWidth";

        List<Position> coordinates;
        Color strokeColor;
        float strokeWidth;
        /// <summary>
        /// List of positions of the polygon
        /// </summary>
        public List<Position> Coordinates
        {
            get { return coordinates; }
            set { this.SetField(ref coordinates, value); }
        }
        /// <summary>
        /// Gets/Sets the stroke color of the polygon
        /// </summary>
        public Color StrokeColor
        {
            get { return strokeColor; }
            set { this.SetField(ref strokeColor, value); }
        }
        /// <summary>
        /// Gets/Sets the width of the stroke
        /// </summary>
        public float StrokeWidth
        {
            get { return strokeWidth; }
            set { this.SetField(ref strokeWidth, value); }
        }
        /// <summary>
        /// Creates a new instance of <c>TKPolygon</c>
        /// </summary>
        public TKPolygon()
        {
            coordinates = new List<Position>();
        }
    }
}
