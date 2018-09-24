using System;
using System.Collections.Generic;
using System.Linq;
using CommandConsole.ConsoleParser;

namespace CommandConsole.Parameters
{
    public class IListParameter<T> : Parameter
    {
        public IListParameter(string name, bool optional = false) : base(name, optional)
        {
        }

        protected override object ParseValue(IValue value)
        {
            return GetList(value);
        }

        protected IEnumerable<T> GetList(IValue value)
        {
            var vList = (VList)value;
            var paramType = Console.DefaultParameters[typeof(T)];
            var parameter = ReflectionHelper.ConstructParameter(paramType, "element", false);
            return vList.Variables.Select(value1 => (T)parameter.Parse(value1));
        }

        public override bool CanParse(IValue value)
        {
            var vList = value as VList;
            return (vList != null);
        }

        public override Type GetParamType()
        {
            return typeof(IList<T>);
        }

        public override string GetSyntax()
        {
            var parType = Console.DefaultParameters[typeof(T)];
            var par = ReflectionHelper.ConstructParameter(parType, "element", false);
            return string.Format("{{{0} {0} ... {0}}}", par.GetSyntax());
        }
    }
}
