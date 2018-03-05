using System;

namespace CommandConsole.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ConsoleCommandAttribute : Attribute
    {
        public string Name { get; set; }
        public string ReturnMessage { get; set; }
        public bool Global { get; set; }

        public ConsoleCommandAttribute()
        {
            ReturnMessage = ConsoleCommand.DefaultReturnMessage;
        }
    }
}
