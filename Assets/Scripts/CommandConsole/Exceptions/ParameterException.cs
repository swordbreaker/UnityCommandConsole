using System;
using System.Runtime.Serialization;
using CommandConsole.Parameters;

namespace CommandConsole.Exceptions
{
    public class ParameterException : ConsoleException
    {
        public IParameter Parameter { get; set; }

        public ParameterException(string message, IParameter parameter) : base(message)
        {
            Parameter = parameter;
        }

        public ParameterException(string message, Exception innerException, IParameter parameter) : base(message, innerException)
        {
            Parameter = parameter;
        }

        public ParameterException(SerializationInfo info, StreamingContext context, IParameter parameter) : base(info, context)
        {
            Parameter = parameter;
        }
    }
}
