using System;

namespace CommandConsole.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ConsoleNumericParameterAttribute : ConsoleParameterAttribute
    {
        public float From { get; set; }
        public float Till { get; set; }

        public ConsoleNumericParameterAttribute(float from, float till)
        {
            From = from;
            Till = till;
        }
    }
}