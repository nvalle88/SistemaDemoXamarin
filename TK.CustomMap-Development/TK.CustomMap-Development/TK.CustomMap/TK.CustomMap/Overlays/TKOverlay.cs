using Xamarin.Forms;

namespace TK.CustomMap.Overlays
{
    /// <summary>
    /// Base overlay class
    /// </summary>
    public abstract class TKOverlay : TKBase
    {
        public const string ColorPropertyName = "Color";

        Color color;

        /// <summary>
        /// Gets/Sets the main color of the overlay.
        /// </summary>
        public Color Color
        {
            get { return color; }
            set { this.SetField(ref color, value); }
        }
    }
}