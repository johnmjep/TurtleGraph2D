using System;
using System.Collections.Generic;
using System.Linq;

namespace TurtleGraph2D
{
    /// <summary>
    /// Turtle Commands Enumeration
    /// </summary>
    public enum Commands
    {
        Forward = 'F',
        ForwardBlank = 'f',
        Backward = 'B',
        BackwardBlank = 'b',
        TurnLeft = '-',
        TurnRight = '+',
        SetPosition = 'S',
        SetHeading = 'H',
        PushStack = '[',
        PopStack = ']',
        PenUp = 'U',
        PenDown = 'D',
        StampPoint = '@',
        Invalid = 'X',
    }

    /// <summary>
    /// Class to hold a Turtle Command and provide static functions for parsing raw turtle strings
    /// </summary>
    public class TurtleCommand
    {        
        #region Instance Fields
        public Commands Cmd { get; private set; }
        public double[] Parameters { get; private set; }
        #endregion Instance Fields

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cmd">Turtle Command</param>
        public TurtleCommand(Commands cmd) 
            : this(cmd, null) { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cmd">Turtle Command</param>
        /// <param name="parameters">Command Parameters</param>
        public TurtleCommand(Commands cmd, double[] parameters)
        {
            this.Cmd = cmd;
            this.Parameters = parameters;
        }
        #endregion Constructors

        #region Static Fields
        private static readonly char[] _VALID_COMMANDS = { 'F', 'f', 'B', 'b', '-', '+', 'S', 'H', '[', ']', 'U', 'D', '@' };
        private static readonly Dictionary<char, Commands> _COMMAND_TABLE = new Dictionary<char, Commands>()
        {
            { (char)Commands.Backward, Commands.Backward },
            { (char)Commands.BackwardBlank, Commands.BackwardBlank },
            { (char)Commands.Forward, Commands.Forward },
            { (char)Commands.ForwardBlank, Commands.ForwardBlank },
            { (char)Commands.TurnLeft, Commands.TurnLeft },
            { (char)Commands.TurnRight, Commands.TurnRight },
            { (char)Commands.SetPosition, Commands.SetPosition },
            { (char)Commands.SetHeading, Commands.SetHeading },
            { (char)Commands.PushStack, Commands.PushStack },
            { (char)Commands.PopStack, Commands.PopStack },
            { (char)Commands.PenUp, Commands.PenUp },
            { (char)Commands.PenDown, Commands.PenDown },
            { (char)Commands.StampPoint, Commands.StampPoint },
            { (char)Commands.Invalid, Commands.Invalid }
        };
        private const char _PARAMETER_START_DELIMITER = '(';
        private const char _PARAMETER_END_DELIMITER = ')';
        private const char _PARAMETER_DELIMITER = ',';
        private const int _START_INDEX = 0;
        public const int ITEM_NOT_FOUND = -1;
        #endregion Static Fields

        #region Static Methods
        /// <summary>
        /// Finds the first valid command from a string of commands
        /// </summary>
        /// <param name="rawCommandString">Input command string</param>
        /// <returns>Strongly typed Turtle Command and strips command from raw input string</returns>
        public static TurtleCommand GetFirstCommandAndStrip(ref string rawCommandString)
        {
            TurtleCommand tCmd;
            int firstCmdIndex = FindIndexOfNextCommand(rawCommandString, _START_INDEX);
            if (firstCmdIndex != ITEM_NOT_FOUND)
            {
                int nextCmdIndex = FindIndexOfNextCommand(rawCommandString, firstCmdIndex + 1);
                string thisCmd = rawCommandString.Substring(firstCmdIndex, nextCmdIndex - firstCmdIndex);
                rawCommandString = rawCommandString.Substring(nextCmdIndex);
                if (CommandHasParameters(thisCmd))
                {
                    tCmd = new TurtleCommand(GetCommandFromRaw(thisCmd), GetParametersFromRaw(thisCmd));
                }                
                else
                {
                    tCmd = new TurtleCommand(GetCommandFromRaw(thisCmd));
                }
            }
            else
            {
                tCmd = new TurtleCommand(Commands.Invalid);
            }
            return tCmd;
        }        

        /// <summary>
        /// Determines if a string contais a valid turtle command
        /// </summary>
        /// <param name="rawCommandString">Input command string</param>
        /// <returns>True if string contains a valid command</returns>
        public static bool ContainsValidCommand(string rawCommandString)
        {
            bool containsValid = false;
            if (FindIndexOfNextCommand(rawCommandString, _START_INDEX) != ITEM_NOT_FOUND)
            {
                containsValid = true;
            }
            return containsValid;
        }

        /// <summary>
        /// Determines if a command string contains parameters
        /// </summary>
        /// <param name="cmd">Input cmd string</param>
        /// <returns>True if command contains parameters</returns>
        private static bool CommandHasParameters(string cmd)
        {
            bool hasParameters = false;
            if (cmd.Contains(_PARAMETER_START_DELIMITER) &&
                cmd.Contains(_PARAMETER_END_DELIMITER))
            {
                hasParameters = true;
            }
            return hasParameters;
        }

        /// <summary>
        /// Finds the index of the next command in a string given a starting point
        /// </summary>
        /// <param name="rawCommandString">Input command string</param>
        /// <param name="startIndex">Index to start searching from</param>
        /// <returns>Index of next command from starting point</returns>
        private static int FindIndexOfNextCommand(string rawCommandString, int startIndex)
        {
            bool withinBrackets = false;
            int index = ITEM_NOT_FOUND;
            for (int i = startIndex; i < rawCommandString.Length; i++)
            {
                // Ignore any commands within brakets (otherwise negative parameters cause problems)
                if (!withinBrackets && rawCommandString[i] == _PARAMETER_START_DELIMITER)
                {
                    withinBrackets = true;
                }
                else if (withinBrackets && rawCommandString[i] == _PARAMETER_END_DELIMITER)
                {
                    withinBrackets = false;
                }
                if(_VALID_COMMANDS.Contains(rawCommandString[i]) && !withinBrackets)
                {
                    index = i;
                    break;
                }
            }
            if (index == ITEM_NOT_FOUND && rawCommandString != "")
            {
                index = rawCommandString.Length;
            }
            return index;
        }

        /// <summary>
        /// Gets Enumerated Command from raw string
        /// </summary>
        /// <param name="rawCommand">Input string</param>
        /// <returns>Enumerated Command</returns>
        public static Commands GetCommandFromRaw(string rawCommand)
        {
            Commands cmd = Commands.Invalid;
            if (rawCommand.Length > 0 && _VALID_COMMANDS.Contains(rawCommand[0]))
            {
                cmd = _COMMAND_TABLE[rawCommand[_START_INDEX]]; 
            }
            return cmd;
        }

        /// <summary>
        /// Gets Parameters from string as a double array
        /// </summary>
        /// <param name="rawCommand">Input string</param>
        /// <returns>Parameters as double array</returns>
        private static double[] GetParametersFromRaw(string rawCommand)
        {
            double[] parameters;
            int parameterLength = rawCommand.IndexOf(_PARAMETER_END_DELIMITER) - rawCommand.IndexOf(_PARAMETER_START_DELIMITER);
            string rawParameters = rawCommand.Substring(rawCommand.IndexOf(_PARAMETER_START_DELIMITER), parameterLength);
            string trimmedParameters = rawParameters.Replace(_PARAMETER_START_DELIMITER.ToString(), "")
                                                    .Replace(_PARAMETER_END_DELIMITER.ToString(), "");

            if (!HasNonDoubleParameters(trimmedParameters))
            {
                try
                {
                    parameters = Array.ConvertAll(trimmedParameters.Split(_PARAMETER_DELIMITER), double.Parse);
                }
                catch (FormatException exc)
                {
                    Console.WriteLine("XXX Encountered Format Exception parsing command parameters");
                    Console.WriteLine("XXX String: {0}", rawCommand);
                    Console.WriteLine("XXX Message: {0}", exc.Message);
                    Console.WriteLine("XXX Source: {0}", exc.Source);
                    Console.WriteLine("XXX Stack: {0}", exc.StackTrace);
                    parameters = null;
                }
                catch (Exception exc)
                {
                    throw exc;
                } 
            }
            else
            {
                parameters = null;
            }
            return parameters;
        }

        /// <summary>
        /// Determines if a parameter set contains any non-Double parameters
        /// </summary>
        /// <param name="parameters">Comma delimited parameter set</param>
        /// <returns>True if contains non-Double</returns>
        private static bool HasNonDoubleParameters(string parameters)
        {
            bool nonDouble = false;
            double dOut;
            foreach (string s in parameters.Split(_PARAMETER_DELIMITER))
            {
                if (double.TryParse(s, out dOut) == false)
                {
                    nonDouble = true;
                }
            }
            return nonDouble;
        }
        #endregion Static Methods
    }
}

