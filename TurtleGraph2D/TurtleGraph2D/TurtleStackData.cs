using BasicGraph;

namespace TurtleGraph2D
{
    /// <summary>
    /// Class to maintain turtle data on a stack
    /// </summary>
    class TurtleStackData
    {
        #region Fields
        public Vector2 Position;
        public double Heading;
        public Vertex<Vector2> PreviousVertex;
        public Vertex<Vector2> CurrentVertex;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position">Position of turtle</param>
        /// <param name="heading">Heading of turtle</param>
        /// <param name="previousVertex">Previous vertex of turtle</param>
        /// <param name="currentVertex">Current vertex of turtle</param>
        public TurtleStackData(Vector2 position, double heading, Vertex<Vector2> previousVertex,
                               Vertex<Vector2> currentVertex)
        {
            Position = position;
            Heading = heading;
            PreviousVertex = previousVertex;
            CurrentVertex = currentVertex;
        }
        #endregion Constructors
    }
}