namespace TurtleGraph2D
{
    /// <summary>
    /// Useful extensions for Turtle
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Extension method for double to convert degrees to radians
        /// </summary>
        /// <param name="value">Angle in degrees</param>
        /// <returns>Angle in radians</returns>
        public static double ToRadians(this double value)
        {
            return value * 0.0174533;
        }

        /// <summary>
        /// Extension method for double to convert radians to degrees
        /// </summary>
        /// <param name="value">Angle in radians</param>
        /// <returns>Angle in degrees</returns>
        public static double ToDegrees(this double value)
        {
            return value * 57.2958;
        }
    }
}