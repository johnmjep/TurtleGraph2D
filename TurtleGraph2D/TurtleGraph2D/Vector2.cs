using System;

namespace TurtleGraph2D
{
    /// <summary>
    /// Simple structure to hold a 2D vector
    /// </summary>
    public struct Vector2
    {
        #region Fields
        public double X;
        public double Y;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="v">Vector to copy</param>
        public Vector2(Vector2 v) 
            : this(v.X, v.Y) 
        { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">Vector X value</param>
        /// <param name="y">Vector Y value</param>
        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Sets the X and Y values of this Vector2
        /// </summary>
        /// <param name="v">Input Vector</param>
        public void Set(Vector2 v)
        {
            Set(v.X, v.Y);
        }

        /// <summary>
        /// Sets the X and Y values of this Vector2
        /// </summary>
        /// <param name="x">Input X value</param>
        /// <param name="y">Input Y value</param>
        public void Set(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Returns the distance from this Vector to another Vector2
        /// </summary>
        /// <param name="x">Vector to measure to</param>
        /// <returns>Distance as double</returns>
        public double Distance(Vector2 v)
        {
            return Math.Sqrt(Math.Pow((v.X - this.X), 2) + Math.Pow((v.Y - this.Y), 2));
        }

        /// <summary>
        /// Overrides base ToString method
        /// </summary>
        /// <returns>String representation of this object</returns>
        public override string ToString()
        {
            return string.Format("Vector2 | X: {0}, Y {1}", X, Y);
        }
        #endregion Methods
    }
}
