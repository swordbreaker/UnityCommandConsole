using System.Linq;
using CommandConsole.ConsoleParser;

namespace CommandConsole.Parameters
{
    public class ArrayParameter<T> : IListParameter<T>
    {
        public ArrayParameter(string name, bool optional = false) : base(name, optional)
        {
        }

        protected override object ParseValue(IValue v)
        {
            return GetList(v).ToArray();
        }
    }
}
