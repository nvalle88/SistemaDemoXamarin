using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TK.CustomMap
{
    /// <summary>
    /// A custom map pin
    /// </summary>
    public class TKCustomMapPin : TKBase
    {
        bool isVisible;
        string id;
        string title;
        string subtitle;
        bool showCallout;
        Position position;
        ImageSource image;
        bool isDraggable;
        Color defaultPinColor;
        Point anchor;
        double rotation;
        bool isCalloutClickable;

        public const string IDPropertyName = "ID";
        public const string TitlePropertyName = "Title";
        public const string SubititlePropertyName = "Subtitle";
        public const string PositionPropertyName = "Position";
        public const string ImagePropertyName = "Image";
        public const string IsVisiblePropertyName = "IsVisible";
        public const string IsDraggablePropertyName = "IsDraggable";
        public const string ShowCalloutPropertyName = "ShowCallout";
        public const string DefaultPinColorPropertyName = "DefaultPinColor";
        public const string AnchorPropertyName = "Anchor";
        public const string RotationPropertyName = "Rotation";
        public const string IsCalloutClickablePropertyName = "IsCalloutClickable";

        /// <summary>
        /// Gets/Sets visibility of a pin
        /// </summary>
        public bool IsVisible
        {
            get { return isVisible; }
            set { this.SetField(ref isVisible, value); }
        }
        /// <summary>
        /// Gets/Sets ID of the pin, used for client app reference (optional)
        /// </summary>
        public string ID
        {
            get { return id; }
            set { this.SetField(ref id, value); }
        }
        /// <summary>
        /// Gets/Sets title of the pin displayed in the callout
        /// </summary>
        public string Title
        {
            get { return title; }
            set { this.SetField(ref title, value); }
        }
        /// <summary>
        /// Gets/Sets the subtitle of the pin displayed in the callout
        /// </summary>
        public string Subtitle
        {
            get { return subtitle; }
            set { this.SetField(ref subtitle, value); }
        }
     
        /// <summary>
        /// Gets/Sets if the callout should be displayed when a pin gets selected
        /// </summary>

        public bool ShowCallout
        {
            get { return showCallout; }
            set { this.SetField(ref showCallout, value); }
        }
        /// <summary>
        /// Gets/Sets the position of the pin
        /// </summary>
        public Position Position
        {
            get { return position; }
            set { this.SetField(ref position, value); }
        }
        /// <summary>
        /// Gets/Sets the image of the pin. If null the default is used
        /// </summary>
        public ImageSource Image
        {
            get { return image; }
            set { this.SetField(ref image, value); }
        }
        /// <summary>
        /// Gets/Sets if the pin is draggable
        /// </summary>
        public bool IsDraggable
        {
            get { return isDraggable; }
            set { this.SetField(ref isDraggable, value); }
        }
        /// <summary>
        /// Gets/Sets the color of the default pin. Only applies when no <see cref="Image"/> is set
        /// </summary>
        public Color DefaultPinColor
        {
            get { return defaultPinColor; }
            set { this.SetField(ref defaultPinColor, value); }
        }
        /// <summary>
        /// Gets/Sets the anchor point of the pin when using a custom pin image
        /// </summary>
        public Point Anchor
        {
            get { return anchor; }
            set { this.SetField(ref anchor, value); }
        }
        /// <summary>
        /// Gets/Sets the rotation angle of the pin in degrees
        /// </summary>
        public double Rotation
        {
            get { return rotation; }
            set { this.SetField(ref rotation, value); }
        }
        /// <summary>
        /// Gets/Sets whether the callout is clickable or not. This adds/removes the accessory control on iOS
        /// </summary>
        public bool IsCalloutClickable
        {
            get { return isCalloutClickable; }
            set { this.SetField(ref isCalloutClickable, value); }
        }
        /// <summary>
        /// Creates a new instance of <see cref="TKCustomMapPin" />
        /// </summary>
        public TKCustomMapPin()
        {
            IsVisible = true;
        }
    }
}