using System.Linq;
using CommandConsole.ConsoleParser;

namespace CommandConsole.Parameters
{
    public class ListParameter<T> : IListParameter<T>
    {
        public ListParameter(string name, bool optional = false) : base(name, optional)
        {
        }

        protected override object ParseValue(IValue v)
        {
            return GetList(v).ToList();
        }
    }
}
