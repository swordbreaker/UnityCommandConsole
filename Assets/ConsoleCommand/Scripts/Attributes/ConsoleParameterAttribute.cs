using System;

namespace CommandConsole.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ConsoleParameterAttribute : Attribute
    {
        public Type ParameterType { get; set; }
        public string ParameterName { get; set; }
    }
}