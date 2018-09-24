using System;
using System.Runtime.Serialization;
using CommandConsole.Parameters;

namespace CommandConsole.Exceptions
{
    public class ValidationException : ConsoleException
    {
        public Parameter Parameter { get; set; }

        public ValidationException(string message, Parameter parameter) : base(message)
        {
            Parameter = parameter;
        }

        public ValidationException(string message, Exception innerException, Parameter parameter) : base(message, innerException)
        {
            Parameter = parameter;
        }

        protected ValidationException(SerializationInfo info, StreamingContext context, Parameter parameter) : base(info, context)
        {
            Parameter = parameter;
        }
    }
}
