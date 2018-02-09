using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using TK.CustomMap.Interfaces;
using TK.CustomMap.Models;
using TK.CustomMap.Overlays;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TK.CustomMap
{
    /// <summary>
    /// An extensions of the <see cref="Xamarin.Forms.Maps.Map"/>
    /// </summary>
    public class TKCustomMap : Map, IMapFunctions
    {
        /// <summary>
        /// Event raised when a pin gets selected
        /// </summary>
        public event EventHandler<TKGenericEventArgs<TKCustomMapPin>> PinSelected;
        /// <summary>
        /// Event raised when a drag of a pin ended
        /// </summary>
        public event EventHandler<TKGenericEventArgs<TKCustomMapPin>> PinDragEnd;
        /// <summary>
        /// Event raised when an area of the map gets clicked
        /// </summary>
        public event EventHandler<TKGenericEventArgs<Position>> MapClicked;
        /// <summary>
        /// Event raised when an area of the map gets long-pressed
        /// </summary>
        public event EventHandler<TKGenericEventArgs<Position>> MapLongPress;
        /// <summary>
        /// Event raised when the location of the user changes
        /// </summary>
        public event EventHandler<TKGenericEventArgs<Position>> UserLocationChanged;
        /// <summary>
        /// Event raised when a route gets tapped
        /// </summary>
        public event EventHandler<TKGenericEventArgs<TKRoute>> RouteClicked;
        /// <summary>
        /// Event raised when a route calculation finished successfully
        /// </summary>
        public event EventHandler<TKGenericEventArgs<TKRoute>> RouteCalculationFinished;
        /// <summary>
        /// Event raised when a route calculation failed
        /// </summary>
        public event EventHandler<TKGenericEventArgs<TKRouteCalculationError>> RouteCalculationFailed;
        /// <summary>
        /// Event raised when all pins are added to the map initially
        /// </summary>
        public event EventHandler PinsReady;
        /// <summary>
        /// Event raised when a callout got tapped
        /// </summary>
        public event EventHandler<TKGenericEventArgs<TKCustomMapPin>> CalloutClicked;

        /// <summary>
        /// Property Key for the read-only bindable Property <see cref="MapFunctions"/>
        /// </summary>
        static readonly BindablePropertyKey MapFunctionsPropertyKey = BindableProperty.CreateReadOnly(
            nameof(MapFunctions),
            typeof(IRendererFunctions),
            typeof(TKCustomMap),
            null,
            defaultBindingMode: BindingMode.OneWayToSource);
        /// <summary>
        /// Bindable Property of <see cref="MapFunctions"/>
        /// </summary>
        public static readonly BindableProperty MapFunctionsProperty = MapFunctionsPropertyKey.BindableProperty;
        /// <summary>
        /// Bindable Property of <see cref="CustomPins" />
        /// </summary>
        public static readonly BindableProperty CustomPinsProperty = BindableProperty.Create(
            nameof(CustomPins),
            typeof(IEnumerable<TKCustomMapPin>),
            typeof(TKCustomMap));
        /// <summary>
        /// Bindable Property of <see cref="SelectedPin" />
        /// </summary>
        public static readonly BindableProperty SelectedPinProperty = BindableProperty.Create(
            nameof(SelectedPin),
            typeof(TKCustomMapPin),
            typeof(TKCustomMap),
            defaultBindingMode: BindingMode.TwoWay);
        /// <summary>
        /// Bindable Property of <see cref="PinSelectedCommand" />
        /// </summary>
        public static readonly BindableProperty PinSelectedCommandProperty = BindableProperty.Create(
            nameof(PinSelectedCommand),
            typeof(ICommand),
            typeof(TKCustomMap));
        /// <summary>
        /// Bindable Property of <see cref="MapClickedCommand"/>
        /// </summary>
        public static readonly BindableProperty MapClickedCommandProperty = BindableProperty.Create(
            nameof(MapClickedCommand),
            typeof(ICommand),
            typeof(TKCustomMap));
        /// <summary>
        /// Bindable Property of <see cref="MapLongPressCommand"/>
        /// </summary>
        public static readonly BindableProperty MapLongPressCommandProperty = BindableProperty.Create(
            nameof(MapLongPressCommand),
            typeof(ICommand),
            typeof(TKCustomMap));
        /// <summary>
        /// Bindable Property of <see cref="PinDragEndCommand"/>
        /// </summary>
        public static readonly BindableProperty PinDragEndCommandProperty = BindableProperty.Create(
            nameof(PinDragEndCommand),
            typeof(Command<TKCustomMapPin>),
            typeof(TKCustomMap));
        /// <summary>
        /// Bindable Property of <see cref="PinsReadyCommand"/>
        /// </summary>
        public static readonly BindableProperty PinsReadyCommandProperty = BindableProperty.Create(
            nameof(PinsReadyCommand),
            typeof(ICommand),
            typeof(TKCustomMap));
        /// <summary>
        /// Bindable Property of <see cref="MapCenter"/>
        /// </summary>
        public static readonly BindableProperty MapCenterProperty = BindableProperty.Create(
            nameof(MapCenter),
            typeof(Position),
            typeof(TKCustomMap),
            default(Position),
            defaultBindingMode: BindingMode.TwoWay);
        /// <summary>
        /// Bindable Property of <see cref="IsRegionChangeAnimated"/>
        /// </summary>
        public static readonly BindableProperty IsRegionChangeAnimatedProperty = BindableProperty.Create(
            nameof(IsRegionChangeAnimated),
            typeof(bool),
            typeof(TKCustomMap),
            default(bool));
        /// <summary>
        /// Bindable Property of <see cref="ShowTraffic"/>
        /// </summary>
        public static readonly BindableProperty ShowTrafficProperty = BindableProperty.Create(
            nameof(ShowTraffic),
            typeof(bool),
            typeof(TKCustomMap),
            default(bool));
        /// <summary>
        /// Bindable Property of <see cref="Routes"/>
        /// </summary>
        public static readonly BindableProperty PolylinesProperty = BindableProperty.Create(
            nameof(Polylines),
            typeof(IEnumerable<TKPolyline>),
            typeof(TKCustomMap),
            null);
        /// <summary>
        /// Bindable Property of <see cref="Circles"/>
        /// </summary>
        public static readonly BindableProperty CirclesProperty = BindableProperty.Create(
            nameof(Circles),
            typeof(IEnumerable<TKCircle>),
            typeof(TKCustomMap),
            null);
        /// <summary>
        /// Bindable Property of <see cref="CalloutClickedCommand"/>
        /// </summary>
        public static readonly BindableProperty CalloutClickedCommandProperty = BindableProperty.Create(
            nameof(CalloutClickedCommand),
            typeof(ICommand),
            typeof(TKCustomMap));
        /// <summary>
        /// Bindable Property of <see cref="Polygons"/>
        /// </summary>
        public static readonly BindableProperty PolygonsProperty = BindableProperty.Create(
            nameof(Polygons),
            typeof(IEnumerable<TKPolygon>),
            typeof(TKCustomMap));
        /// <summary>
        /// Bindable Property of <see cref="MapRegion"/>
        /// </summary>
        public static readonly BindableProperty MapRegionProperty = BindableProperty.Create(
            nameof(MapRegion),
            typeof(MapSpan),
            typeof(TKCustomMap),
            defaultBindingMode: BindingMode.TwoWay);
        /// <summary>
        /// Bindable Property of <see cref="Routes"/>
        /// </summary>
        public static readonly BindableProperty RoutesProperty = BindableProperty.Create(
            nameof(Routes),
            typeof(IEnumerable<TKRoute>),
            typeof(TKCustomMap));
        /// <summary>
        /// Bindable Property of <see cref="RouteClickedCommand"/>
        /// </summary>
        public static readonly BindableProperty RouteClickedCommandProperty = BindableProperty.Create(
            nameof(RouteClickedCommand),
            typeof(ICommand),
            typeof(TKCustomMap));
        /// <summary>
        /// Bindable Property of <see cref="RouteCalculationFinishedCommand"/>
        /// </summary>
        public static readonly BindableProperty RouteCalculationFinishedCommandProperty = BindableProperty.Create(
            nameof(RouteCalculationFinishedCommand),
            typeof(ICommand),
            typeof(TKCustomMap));
        /// <summary>
        /// Bindable Property of <see cref="RouteCalculationFailedCommand"/>
        /// </summary>
        public static readonly BindableProperty RouteCalculationFailedCommandProperty = BindableProperty.Create(
            nameof(RouteCalculationFailedCommand),
            typeof(ICommand),
            typeof(TKCustomMap));
        /// <summary>
        /// Bindable Property of <see cref="TilesUrlOptions"/>
        /// </summary>
        public static readonly BindableProperty TilesUrlOptionsProperty = BindableProperty.Create(
            nameof(TilesUrlOptions),
            typeof(TKTileUrlOptions),
            typeof(TKCustomMap));
        /// <summary>
        /// Bindable Property of <see cref="UserLocationChangedCommand"/>
        /// </summary>
        public static readonly BindableProperty UserLocationChangedCommandProperty = BindableProperty.Create(
            nameof(UserLocationChangedCommand),
            typeof(ICommand),
            typeof(TKCustomMap));
        /// <summary>
        /// Gets/Sets the custom pins of the Map
        /// </summary>
        public IEnumerable<TKCustomMapPin> CustomPins
        {
            get { return (IEnumerable<TKCustomMapPin>)GetValue(CustomPinsProperty); }
            set { SetValue(CustomPinsProperty, value); }
        }
        /// <summary>
        /// Gets/Sets the currently selected pin on the map
        /// </summary>
        public TKCustomMapPin SelectedPin
        {
            get { return (TKCustomMapPin)GetValue(SelectedPinProperty); }
            set { SetValue(SelectedPinProperty, value); }
        }
        /// <summary>
        /// Gets/Sets the command when the map was clicked/tapped
        /// </summary>
        public ICommand MapClickedCommand
        {
            get { return (ICommand)GetValue(MapClickedCommandProperty); }
            set { SetValue(MapClickedCommandProperty, value); }
        }
        /// <summary>
        /// Gets/Sets the command when a long press was performed on the map
        /// </summary>
        public ICommand MapLongPressCommand
        {
            get { return (ICommand)GetValue(MapLongPressCommandProperty); }
            set { SetValue(MapLongPressCommandProperty, value); }
        }
        /// <summary>
        /// Gets/Sets the command when a pin drag ended. The pin already has the updated position set
        /// </summary>
        public ICommand PinDragEndCommand
        {
            get { return (ICommand)GetValue(PinDragEndCommandProperty); }
            set { SetValue(PinDragEndCommandProperty, value); }
        }
        /// <summary>
        /// Gets/Sets the command when a pin got selected
        /// </summary>
        public ICommand PinSelectedCommand
        {
            get { return (ICommand)GetValue(PinSelectedCommandProperty); }
            set { SetValue(PinSelectedCommandProperty, value); }
        }
        /// <summary>
        /// Gets/Sets the command when the pins are ready
        /// </summary>
        public ICommand PinsReadyCommand
        {
            get { return (ICommand)GetValue(PinsReadyCommandProperty); }
            set { SetValue(PinsReadyCommandProperty, value); }
        }
        /// <summary>
        /// Gets/Sets the current center of the map.
        /// </summary>
        public Position MapCenter
        {
            get { return (Position)GetValue(MapCenterProperty); }
            set { SetValue(MapCenterProperty, value); }
        }
        /// <summary>
        /// Gets/Sets if a change of <see cref="MapCenter"/> or <see cref="MapRegion"/> should be animated
        /// </summary>
        public bool IsRegionChangeAnimated
        {
            get { return (bool)GetValue(IsRegionChangeAnimatedProperty); }
            set { SetValue(IsRegionChangeAnimatedProperty, value); }
        }
        /// <summary>
        /// Gets/Sets the lines to display on the map
        /// </summary>
        public IEnumerable<TKPolyline> Polylines
        {
            get { return (IEnumerable<TKPolyline>)GetValue(PolylinesProperty); }
            set { SetValue(PolylinesProperty, value); }
        }
        /// <summary>
        /// Gets/Sets the circles to display on the map
        /// </summary>
        public IEnumerable<TKCircle> Circles
        {
            get { return (IEnumerable<TKCircle>)GetValue(CirclesProperty); }
            set { SetValue(CirclesProperty, value); }
        }
        /// <summary>
        /// Gets/Sets the command when a callout gets clicked. When this is set, there will be an accessory button visible inside the callout on iOS.
        /// Android will simply raise the command by clicking anywhere inside the callout, since Android simply renders a bitmap
        /// </summary>
        public ICommand CalloutClickedCommand
        {
            get { return (ICommand)GetValue(CalloutClickedCommandProperty); }
            set { SetValue(CalloutClickedCommandProperty, value); }
        }
        /// <summary>
        /// Gets/Sets the rectangles to display on the map
        /// </summary>
        public IEnumerable<TKPolygon> Polygons
        {
            get { return (IEnumerable<TKPolygon>)GetValue(PolygonsProperty); }
            set { SetValue(PolygonsProperty, value); }
        }
        /// <summary>
        /// Gets/Sets the visible map region
        /// </summary>
        public MapSpan MapRegion
        {
            get { return (MapSpan)GetValue(MapRegionProperty); }
            set { SetValue(MapRegionProperty, value); }
        }
        /// <summary>
        /// Gets/Sets the routes to calculate and display on the map
        /// </summary>
        public IEnumerable<TKRoute> Routes
        {
            get { return (IEnumerable<TKRoute>)GetValue(RoutesProperty); }
            set { SetValue(RoutesProperty, value); }
        }
        /// <summary>
        /// Gets/Sets the command when a route gets tapped
        /// </summary>
        public ICommand RouteClickedCommand
        {
            get { return (Command<TKRoute>)GetValue(RouteClickedCommandProperty); }
            set { SetValue(RouteClickedCommandProperty, value); }
        }
        /// <summary>
        /// Gets/Sets the command when a route calculation finished successfully
        /// </summary>
        public ICommand RouteCalculationFinishedCommand
        {
            get { return (ICommand)GetValue(RouteCalculationFinishedCommandProperty); }
            set { SetValue(RouteCalculationFinishedCommandProperty, value); }
        }
        /// <summary>
        /// Gets/Sets the command when a route calculation failed
        /// </summary>
        public ICommand RouteCalculationFailedCommand
        {
            get { return (ICommand)GetValue(RouteCalculationFailedCommandProperty); }
            set { SetValue(RouteCalculationFailedCommandProperty, value); }
        }
        /// <summary>
        /// Gets/Sets the options for displaying custom tiles via an url
        /// </summary>
        public TKTileUrlOptions TilesUrlOptions
        {
            get { return (TKTileUrlOptions)GetValue(TilesUrlOptionsProperty); }
            set { SetValue(TilesUrlOptionsProperty, value); }
        }
        /// <summary>
        /// Gets/Sets the command when the user location changed
        /// </summary>
        public ICommand UserLocationChangedCommand
        {
            get { return (ICommand)GetValue(UserLocationChangedCommandProperty); }
            set { SetValue(UserLocationChangedCommandProperty, value); }
        }
        /// <summary>
        /// Gets/Sets the avaiable functions on the map/renderer
        /// </summary>
        public IRendererFunctions MapFunctions
        {
            get { return (IRendererFunctions)GetValue(MapFunctionsProperty); }
            private set { SetValue(MapFunctionsPropertyKey, value); }
        }
        /// <summary>
        /// Gets/Sets if traffic information should be displayed
        /// </summary>
        public bool ShowTraffic
        {
            get { return (bool)GetValue(ShowTrafficProperty); }
            set { SetValue(ShowTrafficProperty, value); }
        }
        /// <summary>
        /// Creates a new instance of <c>TKCustomMap</c>
        /// </summary>
        public TKCustomMap()
            : base()
        { }
        /// <summary>
        /// Creates a new instance of <c>TKCustomMap</c>
        /// </summary>
        /// <param name="region">The initial region of the map</param>
        public TKCustomMap(MapSpan region)
            : base(region)
        {
            MapCenter = region.Center;
        }
        /// <summary>
        /// Creates a new instance of <see cref="TKCustomMap"/>
        /// </summary>
        /// <param name="initialLatitude">The initial latitude value</param>
        /// <param name="initialLongitude">The initial longitude value</param>
        /// <param name="distanceInKilometers">The initial zoom distance in kilometers</param>
        public TKCustomMap(double initialLatitude, double initialLongitude, double distanceInKilometers) :
            base(MapSpan.FromCenterAndRadius(new Position(initialLatitude, initialLongitude), Distance.FromKilometers(distanceInKilometers)))
        {
        }
        /// <inheritdoc/>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == "VisibleRegion")
            {
                MapRegion = VisibleRegion;
            }
        }
        /// <summary>
        /// Returns the currently visible map as a PNG image
        /// </summary>
        /// <returns>Map as image</returns>
        public Task<byte[]> GetSnapshot() => MapFunctions.GetSnapshot();

        /// <summary>
        /// Moves the visible region to the specified <see cref="MapSpan"/>
        /// </summary>
        /// <param name="region">Region to move the map to</param>
        /// <param name="animate">If the region change should be animated or not</param>
        public void MoveToMapRegion(MapSpan region, bool animate = false)
        {
            MapFunctions.MoveToMapRegion(region, animate);
        }
        /// <summary>
        /// Fits the map region to make all given positions visible
        /// </summary>
        /// <param name="positions">Positions to fit inside the MapRegion</param>
        /// <param name="animate">If the camera change should be animated</param>
        public void FitMapRegionToPositions(IEnumerable<Position> positions, bool animate = false)
        {
            MapFunctions.FitMapRegionToPositions(positions, animate);
        }
        /// <summary>
        /// Fit all regions on the map
        /// </summary>
        /// <param name="regions">The regions to fit to the map</param>
        /// <param name="animate">Animation on/off</param>
        public void FitToMapRegions(IEnumerable<MapSpan> regions, bool animate = false)
        {
            MapFunctions.FitToMapRegions(regions, animate);
        }
        /// <summary>
        /// Converts an array of <see cref="Point"/> into geocoordinates
        /// </summary>
        /// <param name="screenLocations">The screen locations(pixel)</param>
        /// <returns>A collection of <see cref="Position"/></returns>
        public IEnumerable<Position> ScreenLocationsToGeocoordinates(params Point[] screenLocations)
        {
            return MapFunctions.ScreenLocationsToGeocoordinates(screenLocations);
        }
        /// <summary>
        /// Raises <see cref="PinSelected"/>
        /// </summary>
        /// <param name="pin">The selected pin</param>
        protected void OnPinSelected(TKCustomMapPin pin)
        {
            PinSelected?.Invoke(this, new TKGenericEventArgs<TKCustomMapPin>(pin));

            RaiseCommand(PinSelectedCommand, pin);
        }
        /// <summary>
        /// Raises <see cref="PinDragEnd"/>
        /// </summary>
        /// <param name="pin">The dragged pin</param>
        protected void OnPinDragEnd(TKCustomMapPin pin)
        {
            PinDragEnd?.Invoke(this, new TKGenericEventArgs<TKCustomMapPin>(pin));

            RaiseCommand(PinDragEndCommand, pin);
        }
        /// <summary>
        /// Raises <see cref="MapClicked"/>
        /// </summary>
        /// <param name="position">The position on the map</param>
        protected void OnMapClicked(Position position)
        {
            MapClicked?.Invoke(this, new TKGenericEventArgs<Position>(position));

            RaiseCommand(MapClickedCommand, position);
        }
        /// <summary>
        /// Raises <see cref="MapLongPress"/>
        /// </summary>
        /// <param name="position">The position on the map</param>
        protected void OnMapLongPress(Position position)
        {
            MapLongPress?.Invoke(this, new TKGenericEventArgs<Position>(position));

            RaiseCommand(MapLongPressCommand, position);
        }
        /// <summary>
        /// Raises <see cref="RouteClicked"/>
        /// </summary>
        /// <param name="route">The tapped route</param>
        protected void OnRouteClicked(TKRoute route)
        {
            RouteClicked?.Invoke(this, new TKGenericEventArgs<TKRoute>(route));

            RaiseCommand(RouteClickedCommand, route);
        }
        /// <summary>
        /// Raises <see cref="RouteCalculationFinished"/>
        /// </summary>
        /// <param name="route">The route</param>
        protected void OnRouteCalculationFinished(TKRoute route)
        {
            RouteCalculationFinished?.Invoke(this, new TKGenericEventArgs<TKRoute>(route));

            RaiseCommand(RouteCalculationFinishedCommand, route);
        }
        /// <summary>
        /// Raises <see cref="RouteCalculationFailed"/>
        /// </summary>
        /// <param name="error">The error</param>
        protected void OnRouteCalculationFailed(TKRouteCalculationError error)
        {
            RouteCalculationFailed?.Invoke(this, new TKGenericEventArgs<TKRouteCalculationError>(error));

            RaiseCommand(RouteCalculationFailedCommand, error);
        }
        /// <summary>
        /// Raises <see cref="UserLocationChanged"/>
        /// </summary>
        /// <param name="position">The position of the user</param>
        protected void OnUserLocationChanged(Position position)
        {
            UserLocationChanged?.Invoke(this, new TKGenericEventArgs<Position>(position));

            RaiseCommand(UserLocationChangedCommand, position);
        }
        /// <summary>
        /// Raises <see cref="PinsReady"/>
        /// </summary>
        protected void OnPinsReady()
        {
            PinsReady?.Invoke(this, new EventArgs());

            RaiseCommand(PinsReadyCommand, null);
        }
        /// <summary>
        /// Raises <see cref="CalloutClicked"/>
        /// </summary>
        protected void OnCalloutClicked(TKCustomMapPin pin)
        {
            CalloutClicked?.Invoke(this, new TKGenericEventArgs<TKCustomMapPin>(pin));

            RaiseCommand(CalloutClickedCommand, pin);
        }
        /// <summary>
        /// Raises a specific command
        /// </summary>
        /// <param name="command">The command to raise</param>
        /// <param name="parameter">Addition command parameter</param>
        void RaiseCommand(ICommand command, object parameter)
        {
            if (command != null && command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        void IMapFunctions.SetRenderer(IRendererFunctions renderer) => MapFunctions = renderer;

        /// <inheritdoc/>
        void IMapFunctions.RaisePinSelected(TKCustomMapPin pin) => OnPinSelected(pin);

        /// <inheritdoc/>
        void IMapFunctions.RaisePinDragEnd(TKCustomMapPin pin) => OnPinDragEnd(pin);

        /// <inheritdoc/>
        void IMapFunctions.RaiseMapClicked(Position position) => OnMapClicked(position);

        /// <inheritdoc/>
        void IMapFunctions.RaiseMapLongPress(Position position) => OnMapLongPress(position);

        /// <inheritdoc/>
        void IMapFunctions.RaiseUserLocationChanged(Position position)
        {
            OnUserLocationChanged(position);
        }
        /// <inheritdoc/>
        void IMapFunctions.RaiseRouteClicked(TKRoute route) => OnRouteClicked(route);

        /// <inheritdoc/>
        void IMapFunctions.RaiseRouteCalculationFinished(TKRoute route)
        {
            OnRouteCalculationFinished(route);
        }
        /// <inheritdoc/>
        void IMapFunctions.RaiseRouteCalculationFailed(TKRouteCalculationError route)
        {
            OnRouteCalculationFailed(route);
        }
        /// <inheritdoc/>
        void IMapFunctions.RaisePinsReady() => OnPinsReady();

        /// <inheritdoc/>
        void IMapFunctions.RaiseCalloutClicked(TKCustomMapPin pin) => OnCalloutClicked(pin);
    }
}