using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandConsole.ConsoleParser;
using CommandConsole.Exceptions;
using CommandConsole.Parameters;

namespace CommandConsole
{
    public class ConsoleCommand : IConsoleCommand
    {
        public const string DefaultReturnMessage = "Command executed";
        public delegate void ConsoleAction(object[] arguments);

        public ConsoleAction Action { get; private set; }
        public string CommandName { get; private set; }
        public IParameter[] Parameters { get; private set; }
        public string ReturnMessage { get; set; }

        public ConsoleCommand(string name, ConsoleAction action)
        {
            CommandName = name;
            Action = action;
            Parameters = new IParameter[0];
            ReturnMessage = DefaultReturnMessage;
        }

        public ConsoleCommand(string name, ConsoleAction action, IParameter[] parameters) : this(name, action)
        {
            Parameters = parameters;
            ReturnMessage = DefaultReturnMessage;
        }

        public ConsoleCommand(string name, ConsoleAction action, string returnMessage) : this(name, action)
        {
            ReturnMessage = returnMessage;
        }

        public ConsoleCommand(string name, ConsoleAction action, IParameter[] parameters, string returnMessage) : this(name, action, parameters)
        {
            ReturnMessage = returnMessage;
        }

        public void Execute(params IValue[] arguments)
        {
            if(arguments == null) arguments = new IValue[0];

            if (arguments.Length > Parameters.Length)
            {
                throw new CommandException("To many Arguments for this command", this);
            }

            var args = new List<object>();

            for (int i = 0; i < arguments.Length; i++)
            {
                args.Add(Parameters[i].Parse(arguments[i]));
            }

            foreach (var parameter in Parameters.Skip(arguments.Length))
            {
                if (!parameter.Optional)
                {
                    throw new CommandException(string.Format("Parameter {0} is required: {1}", parameter.Name, GetCommandSyntax()), this);
                }
            }

            Action.Invoke(args.ToArray());
        }

        public string GetCommandSyntax()
        {
            var sb = new StringBuilder();
            sb.Append(CommandName);

            foreach (var parameter in Parameters)
            {
                sb.Append(" ");
                sb.Append(parameter.GetSyntax());
            }

            return sb.ToString();
        }
    }
}
