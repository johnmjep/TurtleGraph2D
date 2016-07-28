using System;
using System.Collections.Generic;
using BasicGraph;

namespace TurtleGraph2D
{
    /// <summary>
    /// Turtle class, produces a Graph of type Vector2 from a command string
    /// </summary>
    public class Turtle
    {
        #region Fields
        private Graph<Vector2> _outputGraph;

        private Vector2 _position = new Vector2(0, 0);
        public Vector2 Position
        {
            get { return _position; }
        }

        public double Heading { get; private set; }
        public bool IsPenDown { get; private set; }

        private Dictionary<Commands, Action<double[]>> _cmdInterpreter;

        private Vertex<Vector2> _previousVertex = null;
        private Vertex<Vector2> _currentVertex = null;

        private Stack<TurtleStackData> _turtleDataStack = new Stack<TurtleStackData>();

        private bool _generateGraph = true;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Turtle()
        {
            InitialiseCmdInterpreter();
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Sets up command inter
        /// </summary>
        private void InitialiseCmdInterpreter()
        {
            _cmdInterpreter = new Dictionary<Commands, Action<double[]>>()
            {
                { Commands.Forward, Forward },
                { Commands.ForwardBlank, ForwardBlank },
                { Commands.Backward, Backward },
                { Commands.BackwardBlank, BackwardBlank },
                { Commands.TurnLeft, TurnLeft },
                { Commands.TurnRight, TurnRight },
                { Commands.SetPosition, SetPosition },
                { Commands.SetHeading, SetHeading },
                { Commands.PenUp, PenUp },
                { Commands.PenDown, PenDown },
                { Commands.StampPoint, StampPoint },
                { Commands.PushStack, PushStack },
                { Commands.PopStack, PopStack },
            };
        }

        /// <summary>
        /// Generates a Graph from a string of turtle commands
        /// </summary>
        /// <param name="rawTurtleCommands">String containing valid turtle commands</param>
        /// <returns>Graph representing turtle movement</returns>
        public Graph<Vector2> GenerateGraph(string rawTurtleCommands)
        {
            _outputGraph = new Graph<Vector2>();
            _generateGraph = true;
            ExecuteCommands(rawTurtleCommands);
            return _outputGraph;
        }

        /// <summary>
        /// Determines where the turtle is after a string of commands
        /// </summary>
        /// <param name="rawTurtleCommands"></param>
        /// <returns></returns>
        public Vector2 FindFinalPositionFrom(string rawTurtleCommands)
        {
            _generateGraph = false;
            ExecuteCommands(rawTurtleCommands);
            return _position;
        }

        /// <summary>
        /// Executes the turtle commands
        /// </summary>
        /// <param name="rawTurtleCommands">Input command string</param>
        private void ExecuteCommands(string rawTurtleCommands)
        {
            string rawCmds = rawTurtleCommands;
            TurtleCommand tCmd;
            while (TurtleCommand.ContainsValidCommand(rawCmds))
            {
                tCmd = TurtleCommand.GetFirstCommandAndStrip(ref rawCmds);
                if (tCmd.Cmd != Commands.Invalid)
                {
                    _cmdInterpreter[tCmd.Cmd](tCmd.Parameters);
                }
            }
        }

        /// <summary>
        /// Initialise Turtle
        /// </summary>
        /// <remarks>Must be called before any commands are issued</remarks>
        /// <param name="x">Starting X position</param>
        /// <param name="y">Starting Y position</param>
        public void Initialise(double x, double y)
        {
            _outputGraph = new Graph<Vector2>();
            _position.Set(x, y);
        }

        /// <summary>
        /// Move turtle forward by specified amount
        /// </summary>
        /// <param name="parameters">First index specifies distance</param>
        public void Forward(double[] parameters)
        {
            double theta = Heading.ToRadians();
            _position.X += parameters[0] * Math.Cos(theta);
            _position.Y += parameters[0] * Math.Sin(theta);

            if (IsPenDown)
            {
                StampPoint(null);
                AddEdge();
            }
        }

        /// <summary>
        /// Move turtle forward by specified amount and don't draw
        /// </summary>
        /// <param name="parameters">First index specifies distance</param>
        public void ForwardBlank(double[] parameters)
        {
            double theta = Heading.ToRadians();
            _position.X += parameters[0] * Math.Cos(theta);
            _position.Y += parameters[0] * Math.Sin(theta);
            _previousVertex = null;
            _currentVertex = null;
        }

        /// <summary>
        /// Move turtle backward by specified amount
        /// </summary>
        /// <param name="parameters">First index specifies distance</param>
        public void Backward(double[] parameters)
        {
            double theta = Heading.ToRadians();
            _position.X -= parameters[0] * Math.Cos(theta);
            _position.Y -= parameters[0] * Math.Sin(theta);

            if (IsPenDown)
            {
                StampPoint(null);
                AddEdge();
            }
        }

        /// <summary>
        /// Move turtle backward by specified amount and don't draw
        /// </summary>
        /// <param name="parameters">First index specifies distance</param>
        public void BackwardBlank(double[] parameters)
        {
            double theta = Heading.ToRadians();
            _position.X -= parameters[0] * Math.Cos(theta);
            _position.Y -= parameters[0] * Math.Sin(theta);
            _previousVertex = null;
            _currentVertex = null;
        }

        /// <summary>
        /// Turn turtle left by specified angle
        /// </summary>
        /// <param name="parameters">First index specifies angle in degrees</param>
        public void TurnLeft(double[] parameters)
        {
            Heading -= parameters[0];
            NormalizeHeading();
        }

        /// <summary>
        /// Turn turtle right by specified angle
        /// </summary>
        /// <param name="parameters">First index specifies angle in degrees</param>
        public void TurnRight(double[] parameters)
        {
            Heading += parameters[0];
            NormalizeHeading();
        }

        /// <summary>
        /// Normalises heading to between 0 and 360 degrees
        /// </summary>
        private void NormalizeHeading()
        {
            //Heading -= (int)Math.Floor(Heading) * Double.FullCircle;
            if (Heading >= 360.0)
            {
                Heading -= 360;
            }
            if (Heading < 0)
            {
                Heading += 360;
            }
        }

        /// <summary>
        /// Explicitly sets the turtle position
        /// </summary>
        /// <param name="parameters">index 0 species X, index 1 specifies Y</param>
        public void SetPosition(double[] parameters)
        {
            _position.Set(parameters[0], parameters[1]);

            if (IsPenDown)
            {
                StampPoint(null);
                AddEdge();
            }
        }

        /// <summary>
        /// Explicitly sets the turtle heading in degrees
        /// </summary>
        /// <param name="parameters">First index specifies heading in degrees</param>
        public void SetHeading(double[] parameters)
        {
            Heading = parameters[0];
            NormalizeHeading();
        }

        /// <summary>
        /// Lifts the pen (disables drawing)
        /// </summary>
        /// <param name="parameters">Not used</param>
        public void PenUp(double[] parameters)
        {
            IsPenDown = false;
        }

        /// <summary>
        /// Lowers the pen (enables drawing)
        /// </summary>
        /// <param name="parameters">Not used</param>
        public void PenDown(double[] parameters)
        {
            IsPenDown = true;
        }

        /// <summary>
        /// Adds a vertex to the graph at the current position
        /// </summary>
        /// <param name="parameters">Not used</param>
        public void StampPoint(double[] parameters)
        {
            if (_generateGraph)
            {
                _previousVertex = _currentVertex;
                _currentVertex = new Vertex<Vector2>(new Vector2(_position));
                _outputGraph.AddVertex(_currentVertex);                 
            }
        }

        /// <summary>
        /// Pushes current turtle state onto stack
        /// </summary>
        /// <param name="parameters">Not used</param>
        public void PushStack(double[] parameters)
        {
            _turtleDataStack.Push(new TurtleStackData(_position, Heading, _previousVertex, _currentVertex));
        }

        /// <summary>
        /// Pops turtle state off of stack
        /// </summary>
        /// <param name="parameters">Not used</param>
        public void PopStack(double[] parameters)
        {
            TurtleStackData tSD = _turtleDataStack.Pop();
            _position = tSD.Position;
            Heading = tSD.Heading;
            _previousVertex = tSD.PreviousVertex;
            _currentVertex = tSD.CurrentVertex;
        }

        /// <summary>
        /// Adds a directed adge from the previous vertex to the current vertex
        /// </summary>
        private void AddEdge()
        {
            if (IsPenDown && _previousVertex != null && _currentVertex != null)
            {
                _outputGraph.AddDirectedEdge(_previousVertex, _currentVertex);
            }            
        }        
        #endregion Methods
    }
}
