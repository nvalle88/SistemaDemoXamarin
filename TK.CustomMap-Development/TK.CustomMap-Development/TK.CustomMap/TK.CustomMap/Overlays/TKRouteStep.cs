namespace TK.CustomMap.Overlays
{
    /// <summary>
    /// A Step of a route
    /// </summary>
    public sealed class TKRouteStep : TKBase, IRouteStepFunctions
    {
        double distance;
        string instructions;

        /// <summary>
        /// Gets the distance of the step
        /// </summary>
        public double Distance
        {
            get { return distance; }
            private set { this.SetField(ref distance, value); }
        }
        /// <summary>
        /// Gets the instructions of the step
        /// </summary>
        public string Instructions
        {
            get { return instructions; }
            private set { this.SetField(ref instructions, value); }
        }
        ///<inheritdoc/>
        void IRouteStepFunctions.SetDistance(double distance) => Distance = distance;

        ///<inheritdoc/>
        void IRouteStepFunctions.SetInstructions(string instructions) => Instructions = instructions;
    }
}