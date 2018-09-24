using System;

namespace CommandConsole.Parameters
{
    public class CharParameter : VariableParameter
    {

        public CharParameter(string name, bool optional = false) : base(name, optional)
        {
        }

        protected override object ParseValue(string value)
        {
            return char.Parse(value);
        }

        protected override bool CanParse(string value)
        {
            char tmp;
            return char.TryParse(value, out tmp);
        }

        public override Type GetParamType()
        {
            return typeof(char);
        }
    }
}
