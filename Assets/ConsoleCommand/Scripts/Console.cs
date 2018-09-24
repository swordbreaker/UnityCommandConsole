using System;
using System.Collections.Generic;
using System.Linq;
using CommandConsole.ConsoleParser;
using UnityEngine;
using CommandConsole.Exceptions;
using CommandConsole.Parameters;
using Sprache;

namespace CommandConsole
{
    public class Console
    {
        public enum LogType
        {
            Info,
            Warning,
            Error
        }

        #region Fields
        /// <summary>
        /// Dictionary with all registered commands
        /// </summary>
        private readonly Dictionary<string, IConsoleCommand> _registeredCommands = new Dictionary<string, IConsoleCommand>();
        private readonly AutoCompleteManger _autoCompleteManger = new AutoCompleteManger();
        public readonly ConsoleHistoryManager HistoryManager = new ConsoleHistoryManager();

        //Colors used for the Log Method
        private readonly Color _infoColor = Color.black;
        private readonly Color _warningColor = Color.yellow;
        private readonly Color _errorColor = Color.red;

        private static Console _instance;

        /// <summary>
        /// Is the console active/open
        /// </summary>
        private bool _isAcitve;

        /// <summary>
        /// All parameters used by the reflection helper and the list parameters to know which parameter class should be used for which type.
        /// </summary>
        public static readonly Dictionary<Type, Type> DefaultParameters = new Dictionary<Type, Type>()
        {
            {typeof(bool), typeof(BoolParameter) },
            {typeof(byte), typeof(ByteParamter) },
            {typeof(char), typeof(CharParameter) },
            {typeof(decimal), typeof(DecimalParameter) },
            {typeof(double), typeof(DoubleParameter) },
            {typeof(float), typeof(FloatParameter) },
            {typeof(long), typeof(LongParameter) },
            {typeof(sbyte), typeof(SbyteParameter) },
            {typeof(short), typeof(ShortParameter) },
            {typeof(uint), typeof(UintParameter) },
            {typeof(int), typeof(IntParameter) },
            {typeof(ulong), typeof(UlongParameter) },
            {typeof(ushort), typeof(UshortParameter) },
            {typeof(string), typeof(StringParameter) },
            {typeof(Vector2), typeof(Vectro2Parameter) },
            {typeof(Vector3), typeof(Vector3Parameter) },
            {typeof(Color), typeof(ColorParameter) },
            {typeof(bool[]), typeof(ArrayParameter<bool>) },
            {typeof(byte[]), typeof(ArrayParameter<byte>) },
            {typeof(char[]), typeof(ArrayParameter<char>) },
            {typeof(decimal[]), typeof(ArrayParameter<decimal>) },
            {typeof(double[]), typeof(ArrayParameter<double>) },
            {typeof(float[]), typeof(ArrayParameter<float>) },
            {typeof(long[]), typeof(ArrayParameter<long>) },
            {typeof(sbyte[]), typeof(ArrayParameter<sbyte>) },
            {typeof(short[]), typeof(ArrayParameter<short>) },
            {typeof(uint[]), typeof(ArrayParameter<uint>) },
            {typeof(int[]), typeof(ArrayParameter<int>) },
            {typeof(ulong[]), typeof(ArrayParameter<ulong>) },
            {typeof(ushort[]), typeof(ArrayParameter<ushort>) },
            {typeof(string[]), typeof(ArrayParameter<string>) },
            {typeof(Vector2[]), typeof(ArrayParameter<Vector2>) },
            {typeof(Vector3[]), typeof(ArrayParameter<Vector3>) },
            {typeof(Color[]), typeof(ArrayParameter<Color>) },
            {typeof(List<bool>), typeof(ListParameter<bool>) },
            {typeof(List<byte>), typeof(ListParameter<byte>) },
            {typeof(List<char>), typeof(ListParameter<char>) },
            {typeof(List<decimal>), typeof(ListParameter<decimal>) },
            {typeof(List<double>), typeof(ListParameter<double>) },
            {typeof(List<float>), typeof(ListParameter<float>) },
            {typeof(List<long>), typeof(ListParameter<long>) },
            {typeof(List<sbyte>), typeof(ListParameter<sbyte>) },
            {typeof(List<short>), typeof(ListParameter<short>) },
            {typeof(List<uint>), typeof(ListParameter<uint>) },
            {typeof(List<int>), typeof(ListParameter<int>) },
            {typeof(List<ulong>), typeof(ListParameter<ulong>) },
            {typeof(List<ushort>), typeof(ListParameter<ushort>) },
            {typeof(List<string>), typeof(ListParameter<string>) },
            {typeof(List<Vector2>), typeof(ListParameter<Vector2>) },
            {typeof(List<Vector3>), typeof(ListParameter<Vector3>) },
            {typeof(List<Color>), typeof(ListParameter<Color>) },
        };
        #endregion

        #region Properties
        public bool IsAcitve
        {
            get { return _isAcitve; }
            set
            {
                _isAcitve = value;
                if (_isAcitve)
                {
                    if (OnActivate != null) OnActivate.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    if (OnDeActivate != null) OnDeActivate.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public static Console Instance
        {
            get { return _instance ?? (_instance = new Console()); }
        }
        #endregion

        #region Events
        public class ConsoleLogEventArgs : EventArgs
        {
            public string Message { get; set; }

            public ConsoleLogEventArgs(string message)
            {
                Message = message;
            }
        }

        public event EventHandler<ConsoleLogEventArgs> OnLog;
        public event EventHandler OnActivate;
        public event EventHandler OnDeActivate;
        #endregion

        /// <summary>
        /// Creates the console new. The console will lose all registered commands. Use this on scene switch.
        /// </summary>
        /// <returns>The new instance of the Console</returns>
        public static Console CreateNew()
        {
            _instance = new Console();
            return _instance;
        }

        /// <summary>
        /// Registers a new console command.
        /// </summary>
        /// <param name="command">The command.</param>
        public void RegisterCommand(IConsoleCommand command)
        {
            if (_registeredCommands.ContainsKey(command.CommandName))
            {
                Debug.LogError(string.Format("Cannot register command a command whit the name {0} is already registered", command.CommandName));
                return;
            }

            _registeredCommands.Add(command.CommandName, command);
            var s = command.CommandName.Split('.');
            if (s.Length > 1)
            {
                _autoCompleteManger.Add(s[0] + ".", s[1]);
            }
            else
            {
                _autoCompleteManger.Add(command.CommandName);
            }
        }

        /// <summary>
        /// Registers a new class. The class will be searched through with reflection.
        /// </summary>
        /// <typeparam name="T">The Type of the Class</typeparam>
        /// <param name="instance">The instance of the class on which the console will invoke the methods.</param>
        /// <param name="className">A custom classname which will be the prefix for the command for example Class.DoSometing when null the name of the class will be used. Usefull if more than one instance of a class are registerd in the console.</param>
        /// <param name="importType">Import Type:
        /// Public : Import all Public Methods
        /// Marked (default) : Import only Methods with the ConsoleCommand attribute
        /// </param>
        public void RegisterClass<T>(object instance, string className = null, ReflectionHelper.ImportType importType = ReflectionHelper.ImportType.Marked)
        {
            foreach (var cmd in ReflectionHelper.GetCommands(typeof(T), instance, importType, className))
            {
                RegisterCommand(cmd);
            }
        }

        /// <summary>
        /// Remove a registered the command.
        /// </summary>
        /// <param name="command">The command.</param>
        public void DeregisterCommand(IConsoleCommand command)
        {
            DeregisterCommand(command.CommandName);
        }

        /// <summary>
        /// Remove a registered the command.
        /// </summary>
        /// <param name="commandName">The command name.</param>
        public void DeregisterCommand(string commandName)
        {
            if (_registeredCommands.ContainsKey(commandName))
            {
                _registeredCommands.Remove(commandName);
                var s = commandName.Split('.');
                if (s.Length > 1)
                {
                    _autoCompleteManger.Remove(s[0] + ".");
                }
                else
                {
                    _autoCompleteManger.Remove(commandName);
                }
            }
            else
            {
                Debug.LogError("Cannot deregister command, command was not registerd");
            }
        }

        /// <summary>
        /// Removes all commands registered from a class.
        /// </summary>
        /// <typeparam name="T">The Type of the Class</typeparam>
        /// <param name="instance">The instance of the class.</param>
        /// <param name="className">The registred class name</param>
        /// <param name="importType">Use the same import type here as you used on register</param>
        public void DeregisterClass<T>(object instance, string className = null, ReflectionHelper.ImportType importType = ReflectionHelper.ImportType.Marked)
        {
            foreach (var cmd in ReflectionHelper.GetCommands(typeof(T), instance, importType, className))
            {
                if(_registeredCommands.ContainsKey(cmd.CommandName)) DeregisterCommand(cmd.CommandName);
            }
        }

        /// <summary>
        /// Executes a command. Tries to parse the provided string into a command and executes it.
        /// </summary>
        /// <param name="arg">Provided string form the consle.</param>
        public void Execute(string arg)
        {
            var commandResult = CParser.CommandParser.TryParse(arg);

            if (commandResult.WasSuccessful)
            {
                var cmd = commandResult.Value;

                //Does the command exist?
                if (_registeredCommands.ContainsKey(cmd.Name))
                {
                    try
                    {
                        //Is the first parameter of the comment a question mark? Then show the command syntax
                        if (cmd.Parameters.FirstOrDefault() is QuestionMark)
                        {
                            Log(_registeredCommands[cmd.Name].GetCommandSyntax());
                        }
                        else
                        {
                            //Execute the command
                            _registeredCommands[cmd.Name].Execute(cmd.Parameters.ToArray());
                            if (_registeredCommands[cmd.Name].ReturnMessage != null)
                            {
                                Log(_registeredCommands[cmd.Name].ReturnMessage);
                            }
                        }
                    }
                    catch (ConsoleException e)
                    {
                        Log(e.Message, LogType.Error);
                    }
                }
                else
                {
                    Log("Command not found", LogType.Error);
                }
            }
            else
            {
                Log("Cannot parse your comment", LogType.Error);
            }
        }

        /// <summary>
        /// Find comments which match the pattern (substring of the comment form index 0)
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns>A list of matching commands</returns>
        public List<string> GetMatchingCommands(string pattern)
        {
            return _autoCompleteManger.GetAvailableCommands(pattern).ToList();
        }

        public void Help()
        {
            foreach (var cmd in _registeredCommands.Keys)
            {
                Log("-" + cmd);
            }
        }

        /// <summary>
        /// Logs the specified message to the console.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <param name="logType">Type of the log.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">logType - null</exception>
        public void Log(string msg, LogType logType = LogType.Info)
        {
            if (OnLog != null)
            {
                var color = "";
                switch (logType)
                {
                    case LogType.Info:
                        color = string.Format("<color=#{0}>", ColorUtility.ToHtmlStringRGBA(_infoColor));
                        break;
                    case LogType.Warning:
                        color = string.Format("<color=#{0}>", ColorUtility.ToHtmlStringRGBA(_warningColor));
                        break;
                    case LogType.Error:
                        color = string.Format("<color=#{0}>", ColorUtility.ToHtmlStringRGBA(_errorColor));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("logType", logType, null);
                }

                OnLog(this, new ConsoleLogEventArgs(color + msg + "</color>"));
            }
        }
    }
}
