using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TK.CustomMap.Overlays
{
    /// <summary>
    /// Displaying a circle on the map
    /// </summary>
    public class TKCircle : TKOverlay
    {
        public const string RadiusPropertyName = "Radius";
        public const string StrokeColorPropertyName = "StrokeColor";
        public const string CenterPropertyName = "Center";
        public const string StrokeWidthPropertyName = "StrokeWidth";

        double radius;
        Color strokeColor;
        Position center;
        float strokeWidth;
        /// <summary>
        /// Gets/Sets the radius of the circle
        /// </summary>
        public double Radius
        {
            get { return radius; }
            set { this.SetField(ref radius, value); }
        }
        /// <summary>
        /// Gets/Sets the stroke color of the circle
        /// </summary>
        public Color StrokeColor
        {
            get { return strokeColor; }
            set { this.SetField(ref strokeColor, value); }
        }
        /// <summary>
        /// Gets/Sets the center position of the circle
        /// </summary>
        public Position Center
        {
            get { return center; }
            set { this.SetField(ref center, value); }
        }
        /// <summary>
        /// Gets/Sets the width of the stroke
        /// </summary>
        public float StrokeWidth
        {
            get { return strokeWidth; }
            set { this.SetField(ref strokeWidth, value); }
        }
    }
}
