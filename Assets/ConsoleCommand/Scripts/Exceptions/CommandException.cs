using System;
using System.Runtime.Serialization;

namespace CommandConsole.Exceptions
{
    public class CommandException : ConsoleException
    {
        public ConsoleCommand Command { get; set; }

        public CommandException(string message, ConsoleCommand command) : base(message)
        {
            Command = command;
        }

        public CommandException(string message, Exception innerException, ConsoleCommand command) : base(message, innerException)
        {
            Command = command;
        }

        public CommandException(SerializationInfo info, StreamingContext context, ConsoleCommand command) : base(info, context)
        {
            Command = command;
        }
    }
}
